using LiveDirectorySyncEngineLogic;
using System;
using System.Windows;
using LiveDirectorySyncEngineLogic.Settings;
using GenericClassLibrary.Logging;
using System.Collections.Generic;
using LiveDirectorySyncEngineLogic.Generic.DataAccess;
using GenericClassLibrary.Validation;
using System.Threading;

namespace LiveDirectorySyncEngineConsoleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SyncWorker _Worker;
        private BindingContext _bindingContext;
        private readonly CancellationTokenSource _source = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
            btnStopSyncApp.IsEnabled = false;
            LoadSettings();
        }

        private void LoadSettings()
        {
            using (IDBConnection connection = LiveDirectorySyncEngineLogic.Container.GetDBConnection())
            {
                ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository(connection);
                SyncSettings settings = syncSettingsRepository.Get(1);
                if (settings == null)
                {
                    settings = new SyncSettings();
                }
                _bindingContext = new BindingContext(settings);
                this.DataContext = _bindingContext;

                ResetLoggers(settings);
                Logger.Info("Started application");
            }
        }

        private void ResetLoggers(SyncSettings settings)
        {
            Logger.RemoveAll();
            Logger.Level = settings.LogLevel;
            //Logger.AddLogger(new ScreenLogger(AddLog));
            LogContent.Reset();
            if (settings.LogPath?.Length > 0)
            {
                Logger.AddLogger(new FileLogger(settings.LogPath, "DirectorySync"));
            }
        }

        private void BtnRunSyncApp_Click(object sender, RoutedEventArgs e)
        {
            _Worker = new SyncWorker(_bindingContext.Settings, Container.GetRealtimeNoneCacheSyncActionHandler(_bindingContext.Settings), Container.GetFileSystem(), _source.Token);

            try
            {
                _Worker.Start();
                btnRunSyncApp.IsEnabled = false;
                btnStopSyncApp.IsEnabled = true;
            }
            catch (InvalidInputException ex)
            {
                Logger.Error("Failed to start sync: ", ex);
                MessageBox.Show(ex.Message);
                EnableSyncStart();
            }
        }

        private void BtnStopSyncApp_Click(object sender, RoutedEventArgs e)
        {
            _source.Cancel();
            _Worker.Stop();
            EnableSyncStart();
        }

        private void EnableSyncStart()
        {
            btnRunSyncApp.IsEnabled = true;
            btnStopSyncApp.IsEnabled = false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (IDBConnection connection = LiveDirectorySyncEngineLogic.Container.GetDBConnection())
                {
                    ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository(connection);
                    SettingsValidator validator = new SettingsValidator(Container.GetFileSystem().Directory);
                    validator.IsValid(_bindingContext.Settings);
                    syncSettingsRepository.Store(_bindingContext.Settings);
                    ResetLoggers(_bindingContext.Settings);
                    Logger.Info("Settings stored");
                }
            }
            catch (InvalidInputException inpEx)
            {
                MessageBox.Show(inpEx.Message, "Invalid input");
            }
        }


    }

    public class BindingContext
    {
        private readonly SyncSettings _settings;
        public List<LogLevel> LogLevels { get; }
        public SyncSettings Settings => _settings;

        public BindingContext(SyncSettings settings)
        {
            _settings = settings;

            LogLevels = new LogLevelList();
        }
    }
}
