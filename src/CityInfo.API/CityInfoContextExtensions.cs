using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextExtensions
    {
        //decorated with this which tells compiler it extends CityInfoContext
        public static void EnsureSeedDataForConext(this CityInfoContext context)
        {
            if (context.Cities.Any()) return;

            //initialize seed data:
            var cities = new List<City>()
            {
                new City()
                {
                    Name = "New York City",
                    Description = "The one with the big park",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "The most visited urban park in the US."
                        }
                    }
                },
                new City()
                {
                    Name = "Paris",
                    Description = "The one with the tower",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Shit Tower",
                            Description = "The most visited tower."
                        }
                    }
                }
            };

            //add cities to context using AddRange
            //the entities are now tracked by the context
            context.Cities.AddRange(cities);
            //calling SaveChanges will effectively execute the statements on the database
            //remember to execute this extension method in startup class
            context.SaveChanges();

        }
    }
}
