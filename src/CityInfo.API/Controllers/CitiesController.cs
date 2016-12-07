using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    //[controller] will route api/cities to 
    //the cities controller
    //[Route("api/[controller]")]
    [Route("/api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public JsonResult GetCity(int id)
        {
            return new JsonResult(
                    CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id)
                );
        }

    }
}
