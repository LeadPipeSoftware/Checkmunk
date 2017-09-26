using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Checkmunk.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration)
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Seq("http://localhost:5341/")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    IndexFormat = "checkmunkapi-index-{0:yyyy.MM}",
                    TemplateName = "CheckmunkApiTemplate",
                    TypeName = "CheckmunkApiEvent",
                    InlineFields = true,
                    AutoRegisterTemplate = true,
                })
                .WriteTo.ApplicationInsightsEvents("187ce542-7a26-44e3-ba58-177fa3fd38f6")
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting the API web host");

                var host = new WebHostBuilder()
                    .UseKestrel(opt =>
                    {
                        opt.Listen(IPAddress.Loopback, 5150);
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    ////.UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseConfiguration(configuration)
                    .UseSerilog()
                    .Build();

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The API web host terminated unexpectedly");

                return 1;
            }
            finally
            {
                Log.Information("Stopping the API web host");

                Log.CloseAndFlush();
            }
        }
    }
}