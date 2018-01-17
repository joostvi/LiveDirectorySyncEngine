using LiveDirectorySyncEngineLogic.Settings;
using System;
using System.Text;
using System.Windows;

namespace LiveDirectorySyncEngineConsoleApp
{

    ///http://www.wpf-tutorial.com/wpf-application/working-with-app-xaml/
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void Application_Startup(object sender, StartupEventArgs e)
        {

            AddExceptionHandlerNoneUIThread();
            // Create the startup window
            MainWindow wnd = new MainWindow();
            // Show the window
            wnd.Show();
        }

        /// <summary>
        /// see https://www.codeproject.com/Articles/90866/Unhandled-Exception-Handler-For-WPF-Applications
        /// </summary>
        #region "Unhandled exception"

        private void AddExceptionHandlerNoneUIThread()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            ShowUnhandledException(e);
        }

        private static bool IsCriticalException(Exception e)
        {
            if (e is InvalidInputException) return false;
            return true;
        }

        private static void ShowUnhandledException(Exception e)
        {
            if (!IsCriticalException(e))
            {
                MessageBox.Show(e.Message, "Input exception!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("An unexpected exception occured! The applcation will be closed.");
            sb.AppendLine("Exception: " + e.Message);
            sb.AppendLine("Type: " + e.GetType());
            MessageBox.Show(sb.ToString(), "Unexpected exception!", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void Application_DispatcherUnhandledException(object sender,
                       System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //If you do not set e.Handled to true, the application will close due to crash.
            ShowUnhandledException(e.Exception);
            e.Handled = true;
            if (IsCriticalException(e.Exception))
            {
                this.MainWindow.Close();
            }
        }
        #endregion
    }
}
