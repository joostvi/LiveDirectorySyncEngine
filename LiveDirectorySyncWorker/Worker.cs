using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericClassLibrary.Exceptions;
using LiveDirectorySyncEngineLogic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using LiveDirectorySyncEngineLogic.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiveDirectorySyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private SyncWorker worker;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            using (_logger.BeginScope("test {1}", "bla" ))
            {
                _logger.LogInformation("SyncService started");
            }
            //TODO implement async ISyncAction implementation with off line handling.
            using (IDBConnection connection = Container.GetDBConnection())
            {
                ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository(connection);
                SyncSettings settings = syncSettingsRepository.Get(1);
                if (settings == null)
                {
                    throw new InvalidConfigurationException("Settings not found!");
                }
                worker = new SyncWorker(settings, Container.GetRealtimeNoneCacheSyncActionHandler(settings), Container.GetFileSystem());
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
