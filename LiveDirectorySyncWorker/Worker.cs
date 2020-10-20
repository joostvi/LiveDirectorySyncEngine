using System;
using System.Threading;
using System.Threading.Tasks;
using GenericClassLibrary.Exceptions;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using LiveDirectorySyncEngineLogic.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiveDirectorySyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private SyncWorker worker;


        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SyncService started");
            //TODO implement async ISyncAction implementation with off line handling.
            using (IDBConnection connection = Container.GetDBConnection(_config))
            {
                ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository(connection);
                SyncSettings settings = syncSettingsRepository.Get(1);
                if (settings == null)
                {
                    throw new InvalidConfigurationException("Settings not found!");
                }
                worker = new SyncWorker(settings, Container.GetRealtimeNoneCacheSyncActionHandler(settings), Container.GetFileSystem(), cancellationToken);
            }
            try
            {
                worker.Start();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to start the sync service", ex);
                throw ex;
            }
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                worker.Stop();
                _logger.LogInformation("SyncService stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while stopping the service", ex);
            }
            return base.StopAsync(cancellationToken);
        }
    }

}
