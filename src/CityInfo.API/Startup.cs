using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CityInfo.API.Services;
using Microsoft.Extensions.Configuration;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API
{
    public class Startup
    {

        public static IConfigurationRoot Configuration;

        //Startup constructor
        //We need to inject the hosting environment,
        //as we will need that to point the framework
        //to the root of our application
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                //tell it to find our settings file below:
                //and we pass in ContentRootPath from hosting env.
                .SetBasePath(env.ContentRootPath)
                //optional argument signifies whether or not the appSettings file is optional
                //reloadOnChange: should the settings file be reloaded if we change it?
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                //The order in which configuration sources is specified is important,
                //as this establishes the precedence with which settings will be applied
                //if they exist in multiple locations.
                //The last configuration specified wins
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional:true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            //Server is equal to database found in SQL Server Object Explorer
            //Under the View options
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=CityInfoDB;Trusted_Connection=True;";

            //below to register db contexts, and by default
            //it will be registered with a scoped lifetime
            //passing it a method overload
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseMvc();

            //app.Run((context) =>
            //{
            //    throw new Exception("Example Exception");
            //});

        /*    app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            */
        }
    }
}
