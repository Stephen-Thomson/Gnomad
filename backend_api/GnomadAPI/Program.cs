/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Where main lives (nice and cozy)
*
************************************************************************************************/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TravelCompanionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            createHostBuilder(args).Build().Run();
        }

        //Sets up the program for Main.
        public static IHostBuilder createHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(5000);
                        options.ListenAnyIP(5001, listen =>
                        {
                            listen.UseHttps("mycertificate.pfx", "gnome");
                        });
                    });
                });
    }
}