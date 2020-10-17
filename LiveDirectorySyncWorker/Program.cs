using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenericClassLibrary.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace LiveDirectorySyncWorker
{
    public class Program
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
                .AddEnvironmentVariables(); //setings found here override appsettings*.
        }

        static void LoggerBuilder(ILoggingBuilder builder)
        {
            builder.AddConsole().AddProvider(new LogProvider());
        }


        public class LogProvider : ILoggerProvider
        {
            public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
            {
                return new DotNetCoreLogger(this, categoryName);
            }

            public void Dispose()
            {
                //nothing todo
            }


            IExternalScopeProvider fScopeProvider;

            internal IExternalScopeProvider ScopeProvider
            {
                get
                {
                    if (fScopeProvider == null)
                        fScopeProvider = new LoggerExternalScopeProvider();
                    return fScopeProvider;
                }
            }
        }

        public class DotNetCoreLogger : Microsoft.Extensions.Logging.ILogger
        {
            public LogProvider LogProvider { get; }
            public string CategoryName { get; }
            public DotNetCoreLogger(LogProvider logProvider, string categoryName)
            {
                //Logger.AddLogger(new ConsoleLogger());
                //Logger.Level = EnumLogLevel.Debug;

                LogProvider = logProvider;
                CategoryName = categoryName;
            }

            private class ScopeTest<TState> : IDisposable
            {
                public TState State { get; }
                public ScopeTest(TState state)
                {
                    State = state;
                }
                public void Dispose()
                {

                }
            }
            public IDisposable BeginScope<TState>(TState state)
            {
                return LogProvider.ScopeProvider.Push(state);
            }

            public bool IsEnabled(MsLogLevel logLevel)
            {
                return Logger.IsEnabled(ToMyLogLevel(logLevel));
            }

            private EnumLogLevel ToMyLogLevel(MsLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MsLogLevel.Information:
                        return EnumLogLevel.Info;
                    case MsLogLevel.Error:
                        return EnumLogLevel.Error;
                    case MsLogLevel.Warning:
                        return EnumLogLevel.Warning;
                    case MsLogLevel.Debug:
                        return EnumLogLevel.Debug;
                    case MsLogLevel.Critical:
                        return EnumLogLevel.Critical;
                    default:
                        return EnumLogLevel.None;
                }
            }

            public void Log<TState>(MsLogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                string message = string.Empty;
                if (formatter != null)
                {
                    message = formatter.Invoke(state, exception);
                }
                string value = $"CategoryName: {CategoryName}, EventId: {eventId.Id}/{eventId.Name}, Message: {message}";

                string scopeData = string.Empty;
                if (LogProvider.ScopeProvider != null)
                {
                    LogProvider.ScopeProvider.ForEachScope((value, loggingProps) =>
                        {
                            if (value is string)
                            {
                                scopeData = value.ToString();
                            }
                            else if (value is IEnumerable<KeyValuePair<string, object>> props)
                            {
                                foreach (var pair in props)
                                {
                                    scopeData += $"{pair.Key} - {pair.Value}";
                                }
                            }
                        },
                        state);
                }
                Logger.Log(ToMyLogLevel(logLevel), scopeData + "\r\n" + value);
            }
        }
    }
}
