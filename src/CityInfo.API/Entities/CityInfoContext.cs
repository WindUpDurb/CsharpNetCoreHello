using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    //make sure to register this context so that is available via dependency injection
    public class CityInfoContext : DbContext
    {

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            //Ensures database is created if it isn't created
            //Database.EnsureCreated();

            //We ensured our database can migrate to most recent version 
            //if it isn't available
            Database.Migrate();
        }

        //DbSets can be used to query and save instances of its entity type.
        //Linked queries against the Dbsets will be translated into queries against the datbase.
        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        //we need to provide a connection stream to our db context to connect to a database,
        //so we configure the db context by overriding the OnConfiguring method
        //But an alternative, more ideal approach is above in the constructor
     /*   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //tells db context it is being used to connect to a SQL database
            optionsBuilder.UseSqlServer("connectionstring");

            base.OnConfiguring(optionsBuilder);
        }
     */
    }
}
