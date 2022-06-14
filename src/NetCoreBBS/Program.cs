using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace NetCoreBBS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("serilog.log")
                    .CreateLogger();
            try
            {
                Log.Debug("init main");
                BuildWebHost(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                Log.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .UseSerilog()
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder
                     //.UseUrls("http://*:8081").UseKestrel().UseContentRoot(Directory.GetCurrentDirectory())
                     .UseStartup<Startup>();
                 });
    }
}
