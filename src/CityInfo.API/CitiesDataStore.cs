using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York",
                    Description = "The Big Apple.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "Where the bums at"
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Shoe City",
                            Description = "Where the shoes at"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Dojo",
                            Description = "Where the karate at"
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Bench",
                            Description = "Where the sitting at"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "La Puente",
                    Description = "Fucking Smells",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Liquor Store",
                            Description = "Where the dranks at"
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Pizza Loca",
                            Description = "Where the pizza at"
                        }
                    }
                }
            };
        }
    }
}
