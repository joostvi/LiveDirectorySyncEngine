using NDatabase.Api;

namespace LiveDirectorySyncEngineLogic.Generic.DataAccess.NDatabaseImplementation
{
    public static class NDatabaseSetup
    {
        public static void Configure(Log.ILogger logger)
        {
            OdbConfiguration.EnableLogging();
            var customLogger = new NDatabaseLogger();
            OdbConfiguration.RegisterLogger(customLogger);
        }
    }
}
