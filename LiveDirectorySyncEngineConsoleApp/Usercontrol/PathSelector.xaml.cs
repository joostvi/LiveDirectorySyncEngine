using System.Windows;
using System.Windows.Forms;

namespace LiveDirectorySyncEngineConsoleApp.Usercontrol
{
    /// <summary>
    /// Interaction logic for PathSelector.xaml
    /// </summary>
    public partial class PathSelector : System.Windows.Controls.UserControl
    {
        public PathSelector()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
DependencyProperty.Register("Path", typeof(string), typeof(PathSelector), new UIPropertyMetadata(null, Text_PropertyChanged));

        private static void Text_PropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ((PathSelector)source).txtPath.Text = (string)e.NewValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = txtPath.Text;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Path = dialog.SelectedPath;
            }
        }

        public string Path
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
