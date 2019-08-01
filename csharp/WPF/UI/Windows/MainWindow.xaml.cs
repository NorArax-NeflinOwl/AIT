using System;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;
using WPF.Managers;
using WPF.UI.Pages.Properties;
using WPF.Models;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, IPropertizableWindow
    {
        public Visibility MainErrorImageVisibility { get { return LogManager.Instance.HandleErrorCounter != 0 ? Visibility.Visible : Visibility.Collapsed; } }
        public IWindowsProperties Properties { get; }

        private TabControlManager MainTabControlManager { get; set; }

        public MainWindow()
        {
            Properties = new MainProperties();
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            CenterWindowOnScreen();
            MainErrorImage.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\UI\\Icons\\dialog_error.png"));

            MainTabControlManager = new TabControlManager(MainTabControl);
            MainTabControlManager.Add(new DashboardProperties());

            MainFileMenu.Header = WPF.Properties.Resources.FILE_HEADER;
            MainFileCloseAllMenu.Header = WPF.Properties.Resources.CLOSEALL_HEADER;
            MainFileSettingsMenu.Header = WPF.Properties.Resources.SETTINGS_HEADER;
            MainFileLogOutMenu.Header = WPF.Properties.Resources.LOGDIR_SUBPATH;
            MainFileExitMenu.Header = WPF.Properties.Resources.EXIT_HEADER;

            MainEditMenu.Header = WPF.Properties.Resources.EDIT_HEADER;
            MainEditUndoMenu.Header = WPF.Properties.Resources.UNDO_HEADER;
            MainEditRedoMenu.Header = WPF.Properties.Resources.REDO_HEADER;

            MainViewMenu.Header = WPF.Properties.Resources.VIEW_HEADER;
            MainViewNewNoteMenu.Header = WPF.Properties.Resources.NEWNOTE_HEADER;

            MainNavigateMenu.Header = WPF.Properties.Resources.NAV_HEADER;
            MainNavigateDashboardMenu.Header = WPF.Properties.Resources.DASHBOARD_HEADER;
            MainNavigateDashboardMenu.IsEnabled = true;

            MainQueryMenu.Header = WPF.Properties.Resources.QUERY_HEADER;
            MainQueryBuilderMenu.Header = WPF.Properties.Resources.QUERYBULIDER_HEADER;

            MainToolsMenu.Header = WPF.Properties.Resources.TOOLS_HEADER;
            MainToolsCSHA256HashMenu.Header = WPF.Properties.Resources.CREATESHA256HASH_HEADER;
            MainToolsCryptPTMenu.Header = WPF.Properties.Resources.CRYPTPLAINTEXT_HEADER;
            MainToolsDecryptCTMenu.Header = WPF.Properties.Resources.DECRYPTCRYPTTEXT_HEADER;

            MainSetupMenu.Header = WPF.Properties.Resources.SETUP_HEADER;
            MainSetupChangeThemeMenu.Header = WPF.Properties.Resources.CHANGETHEME_HEADER;

            MainHelpMenu.Header = WPF.Properties.Resources.HELP_HEADER;
            MainHelpRegisterProductMenu.Header = WPF.Properties.Resources.REGISTERPRODUCT_HEADER;
            MainHelpAboutMenu.Header = WPF.Properties.Resources.ABOUT_HEADER;
        }

        public void Subscribe()
        {
            KeyUp += MainWindow_KeyUp;

            MainFileCloseAllMenu.Click += MainFileCloseAllMenu_Click;
            MainFileSettingsMenu.Click += MainFileSettingsMenu_Click;
            MainFileLogOutMenu.Click += MainFileLogOutMenu_Click;
            MainFileExitMenu.Click += MainFileExitMenu_Click;

            MainNavigateDashboardMenu.Click += MainNavigateDashboardMenu_Click;
        }

        public void Dispose()
        {
            KeyUp -= MainWindow_KeyUp;

            MainFileCloseAllMenu.Click -= MainFileCloseAllMenu_Click;
            MainFileSettingsMenu.Click -= MainFileSettingsMenu_Click;
            MainFileLogOutMenu.Click -= MainFileLogOutMenu_Click;
            MainFileExitMenu.Click -= MainFileExitMenu_Click;

            MainNavigateDashboardMenu.Click -= MainNavigateDashboardMenu_Click;

            MainTabControlManager.Clear();
            GC.Collect();
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            var tabitem = MainTabControl.SelectedContent as IPageModel;
            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}] on [{tabitem?.Header}] page");
        }

        private void MainNavigateDashboardMenu_Click(object sender, RoutedEventArgs e)
        {
            MainTabControlManager.Add(new DashboardProperties());
        }

        private void MainFileExitMenu_Click(object sender, RoutedEventArgs e)
        {
            // TODO Show dialog with question e.g. "Do you want close app?"
        }

        private void MainFileLogOutMenu_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Clear(new LoginWindow());
        }

        private void MainFileSettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            // TODO Open SettinsWindow
        }

        private void MainFileCloseAllMenu_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Clear(new MainWindow());
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
