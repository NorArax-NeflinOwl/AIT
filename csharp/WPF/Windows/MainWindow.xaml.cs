using System;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;
using WPF.Windows.Properties;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;
using WPF.Managers;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, IPropertizableWindow
    {
        public Visibility MainErrorImageVisibility { get { return LogManager.HandleErrorCounter != 0 ? Visibility.Visible : Visibility.Collapsed; } }
        public IWindowsProperties Properties { get; set; }

        public MainWindow()
        {
            Properties = new MainProperties();
            InitializeComponent();
            Subscribe();
            Init();
        }

        public void Subscribe()
        {
            KeyUp += MainWindow_KeyUp;
        }

        public void Init()
        {
            MainErrorImage.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\Icons\\dialog_error.png"));

            MainFileMenu.Header = WPF.Properties.Resources.FILE_HEADER;
            MainEditMenu.Header = WPF.Properties.Resources.EDIT_HEADER;
            MainViewMenu.Header = WPF.Properties.Resources.VIEW_HEADER;
            MainNavigateMenu.Header = WPF.Properties.Resources.NAV_HEADER;
            MainQueryMenu.Header = WPF.Properties.Resources.QUERY_HEADER;
            MainToolsMenu.Header = WPF.Properties.Resources.TOOLS_HEADER;
            MainSetupMenu.Header = WPF.Properties.Resources.SETUP_HEADER;
            MainHelpMenu.Header = WPF.Properties.Resources.HELP_HEADER;
        }

        public void Dispose()
        {
            KeyUp -= MainWindow_KeyUp;
            GC.Collect();
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}] on [{MainPage.Content.ToString()}] page");
        }
    }
}
