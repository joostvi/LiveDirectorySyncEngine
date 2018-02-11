using LiveDirectorySyncEngineLogic;
using System;
using System.Windows;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.Generic.Log;
using LiveDirectorySyncEngineConsoleApp.Logging;
using System.Collections.Generic;

namespace LiveDirectorySyncEngineConsoleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SyncWorker _Worker;
        private BindingContext _bindingContext;

        public MainWindow()
        {
            InitializeComponent();
            btnStopSyncApp.IsEnabled = false;
            LoadSettings();
        }

        private void LoadSettings()
        {
            ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository();
            SyncSettings settings = syncSettingsRepository.Load();
            _bindingContext = new BindingContext(settings);
            this.DataContext = _bindingContext;

            ResetLoggers(settings);
            Log.Info("Started application");
        }

        private void ResetLoggers(SyncSettings settings)
        {
            Log.RemoveAll();
            Log.Level = settings.LogLevel;
            Log.AddLogger(new ScreenLogger(AddLog));
            if (settings.LogPath?.Length > 0)
            {
                Log.AddLogger(new FileLogger(settings.LogPath, "DirectorySync"));
            }
        }

        private void BtnRunSyncApp_Click(object sender, RoutedEventArgs e)
        {
            _Worker = new SyncWorker(_bindingContext.Settings, Container.GetRealtimeNoneCacheSyncActionHandler(_bindingContext.Settings), Container.GetFileSystem());

            try
            {
                _Worker.Start();
                btnRunSyncApp.IsEnabled = false;
                btnStopSyncApp.IsEnabled = true;
            }
            catch (InvalidInputException ex)
            {
                Log.Error("Failed to start sync: ", ex);
                MessageBox.Show(ex.Message);
                EnableSyncStart();
            }
        }

        private void BtnStopSyncApp_Click(object sender, RoutedEventArgs e)
        {
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
            ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository();
            SettingsValidator validator = new SettingsValidator(Container.GetFileSystem().Directory);
            try
            {
                
                validator.IsValid(_bindingContext.Settings);
                syncSettingsRepository.Save(_bindingContext.Settings);
                ResetLoggers(_bindingContext.Settings);
            }
            catch (InvalidInputException inpEx)
            {
                MessageBox.Show(inpEx.Message, "Invalid input");
            }
        }

        #region logging
        public void UpdateLogText(string line)
        {
            LogContent.Text += line;
        }

        public delegate void UpdateLogTextDelegate(string line);
        //see https://msdn.microsoft.com/en-us/library/system.windows.threading.dispatcher(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-3

        public void AddLog(object sender, ScreenLogEventArgs logThis)
        {
            string line = "\r\n" + DateTime.Now.ToString() + " " + logThis.Level.ToString() + ": " + logThis.Value;
            this.Dispatcher.Invoke(new UpdateLogTextDelegate(UpdateLogText), line);
        }
        #endregion
    }

    public class LogLevel
    {
        public EnumLogLevel Level { get; }
        public string Description => Level.ToString();

        public LogLevel(EnumLogLevel level)
        {
            Level = level;
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

            LogLevels = new List<LogLevel>();
            Array valuesAsArray = Enum.GetValues(typeof(EnumLogLevel));
            foreach(int value in valuesAsArray)
            {
                LogLevels.Add(new LogLevel((EnumLogLevel)value));
            }
        }
    }
}
