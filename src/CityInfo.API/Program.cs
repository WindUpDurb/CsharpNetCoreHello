using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CityInfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                //Kestrel is a cross-platform HTTP web server
                .UseKestrel()
                //Specifies to use content root defined by web host
                .UseContentRoot(Directory.GetCurrentDirectory()) 
                .UseIISIntegration()
                //Below specifies startup type to be used by webhost
                .UseStartup<Startup>()
                //Builds an IWeb host instance
                .Build();
            //Runs the Web Application 
            host.Run();
        }
    }
}
