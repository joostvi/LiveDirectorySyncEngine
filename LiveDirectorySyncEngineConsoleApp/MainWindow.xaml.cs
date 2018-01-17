﻿using LiveDirectorySyncEngineLogic;
using System;
using System.Windows;
using LiveDirectorySyncEngineLogic.Settings;
using LiveDirectorySyncEngineLogic.Generic.Log;
using LiveDirectorySyncEngineConsoleApp.Logging;

namespace LiveDirectorySyncEngineConsoleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SyncWorker _Worker;

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
            Source.Text = settings.SourcePath;
            Target.Text = settings.TargetPath;
            Log.Level = EnumLogLevel.Info;  //TODO Make optional
            Log.AddLogger(new ScreenLogger(AddLog));
            Log.Info("Started application");
        }

        private void BtnRunSyncApp_Click(object sender, RoutedEventArgs e)
        {
            SyncSettings settings = GetSettings();
            _Worker = new SyncWorker(settings, Container.GetRealtimeNoneCacheSyncActionHandler(settings),Container.GetFileSystem());

            try
            {
                _Worker.Start();
                btnRunSyncApp.IsEnabled = false;
                btnStopSyncApp.IsEnabled = true;
            }
            catch(InvalidInputException ex)
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

        private SyncSettings GetSettings()
        {
            return new SyncSettings(Source.Text, Target.Text, EnumLogLevel.Error);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ISyncSettingsRepository syncSettingsRepository = Container.GetSyncSettingsRepository();
            SyncSettings syncSetting = GetSettings();
            SettingsValidator validator = new SettingsValidator(Container.GetFileSystem().Directory);
            try
            {
                validator.IsValid(syncSetting);
                syncSettingsRepository.Save(syncSetting);
            }
            catch(InvalidInputException inpEx)
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
            this.Dispatcher.Invoke( new UpdateLogTextDelegate(UpdateLogText), line);
        }
        #endregion
    }
}
