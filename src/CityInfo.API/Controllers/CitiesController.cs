using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Services;
using CityInfo.API.Models;
using AutoMapper;

namespace CityInfo.API.Controllers
{
    //[controller] will route api/cities to 
    //the cities controller
    //[Route("api/[controller]")]
    [Route("/api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            //previoiusly: return Ok(CitiesDataStore.Current.Cities);
            var cityEntities = _cityInfoRepository.GetCities();

            //pass in the type we want to get back (IEnumerable<CityWithoutPointsOfInterestDto>)
            //as parameter, we pass cityEntities that we fetched
            //this is an alternative to mapping with foreach below
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            /*
            var results = new List<CityWithoutPointsOfInterestDto>();
 
            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = cityEntity.Id,
                    Description = cityEntity.Description,
                    Name = cityEntity.Name
                });
            }
            */
            return Ok(results);
        }

        [HttpGet("{id}")]
        //includePointsOfInterest with a default value of false
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            if (city == null) return NotFound();

            //for example to : /api/cities/1?includePointsOfInterest=true
            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            /*
            var cityWithoutPointsOfInterestResult = new CityWithoutPointsOfInterestDto()
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };
            */
            return Ok(cityWithoutPointsOfInterestResult);

        }

    }
}
