using System;
using System.IO;
using GenericClassLibrary.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GenericClassLibrary.Logging.net.core;

namespace LiveDirectorySyncWorker
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(BuildConfig)
            .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
            .ConfigureLogging(LoggerBuilder);
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            //sequence of AddJsonFile does mean 
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true) //settings found here override appsettings.json
                .AddEnvironmentVariables(); //settings found here override appsettings*.
        }

        static void LoggerBuilder(ILoggingBuilder builder)
        {
            Logger.Level = EnumLogLevel.Debug; //Setting yes no?
            builder.ClearProviders(); //Remove any default provider otherwise every thing will be logged to the console twice.
            builder.AddProvider(new LogProvider());
            builder.AddMyConsoleLogger();  //This is my own console logger.
        }
    }
}
