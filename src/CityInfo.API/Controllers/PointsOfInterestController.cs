﻿using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using CityInfo.API.Services;
using AutoMapper;

namespace CityInfo.API.Controllers
{
    [Route("/api/cities")]
    public class PointsOfInterestController : Controller
    {

        private ILogger<PointsOfInterestController> _logger;
        //Injected Below:
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        //construct
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            //dependency injection is the preferred way of requeseting dependencies
            _logger = logger;
            //or can request from container directly by passing it in to below
            //HttpContext.RequestServices.GetService();
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {

                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);
                var pointsOfInterestForCityResults =
                    Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);
               
                return Ok(pointsOfInterestForCityResults);
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                // if (city == null)
                //{
                //   _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                //    return NotFound();
                //}
                //return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }



        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null) return NotFound();

            /*
            var pointOfInterestResult = new PointOfInterestDto()
            {
                Id = pointOfInterest.Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            */
            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return Ok(pointOfInterestResult);
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null) return NotFound();
            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterest == null) return NotFound();
            //return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null) return BadRequest();
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happend while handling your request.");
            }

            var createdPointOfInterestToReturn = Mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", 
                new { cityId = cityId, id = createdPointOfInterestToReturn.Id}, createdPointOfInterestToReturn);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null) return BadRequest();
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null) return NotFound();

            //If we pass in the source object as the first parameter
            //and the destination object as the second parameter,
            //automapper will override the values in the destination object
            //with the values in the source object
            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            //saving to persisit to database:
            if (!_cityInfoRepository.Save()) return StatusCode(500, "A problem happened while handling your request");

            //return 204 No Content, meaning the requets
            //completed successfully, but nothing is being returned
            //Or you can send a 200 with the updated resource
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();
           

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null) return NotFound();

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            if (!_cityInfoRepository.Save()) return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId)) return NotFound();

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null) return NotFound();

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_cityInfoRepository.Save()) return StatusCode(500, "A problem happened while handling your request.");

            _mailService.Send("Point of interest deleted.",
                $"Point of Interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }

    }
}
