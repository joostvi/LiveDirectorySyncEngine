using LiveDirectorySyncEngineLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveDirectorySyncEngineConsoleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnStopSyncApp.IsEnabled = false;
        }

        private RealtimeSyncWorker worker;
        private void BtnRunSyncApp_Click(object sender, RoutedEventArgs e)
        {
            worker = new RealtimeSyncWorker();
            worker.Start();
            btnRunSyncApp.IsEnabled = false;
            btnStopSyncApp.IsEnabled = true;
        }

        private void btnStopSyncApp_Click(object sender, RoutedEventArgs e)
        {
            worker.Stop();
            btnRunSyncApp.IsEnabled = true;
            btnStopSyncApp.IsEnabled = false;
        }
    }
}
