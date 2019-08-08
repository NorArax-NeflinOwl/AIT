using System;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;
using System.Windows.Media.Imaging;
using WPF.Managers;
using WPF.UI.Pages.Properties;
using WPF.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.UI.Controls;
using WPF.Models.Enums;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; }

        private TabControlManager MainTabControlManager { get; set; }

        public MainWindow()
        {
            Properties = new MainProperties(this);
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            MainStatus.Text = "Status: Load...";
            MainProgressBar.Value = 0;
            MainProgressBarDesc.Text = "0/5";

            CenterWindowOnScreen();
            MainErrorImage.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\UI\\Icons\\dialog_error.png"));

            MainProgressBar.Value = 20;
            MainProgressBarDesc.Text = "1/5";

            MainFileMenu.Header = WPF.Properties.Resources.FILE_HEADER;
            MainFileCloseAllWindowsMenu.Header = WPF.Properties.Resources.CLOSEALL_HEADER;
            MainFileCloseAllWindowsMenu.IsEnabled = true;
            MainFileSettingsMenu.Header = WPF.Properties.Resources.SETTINGS_HEADER;
            MainFileSettingsMenu.IsEnabled = true;
            MainFileLogOutMenu.Header = WPF.Properties.Resources.LOGOUT_HEADER;
            MainFileLogOutMenu.IsEnabled = true;
            MainFileExitMenu.Header = WPF.Properties.Resources.EXIT_HEADER;
            MainFileExitMenu.IsEnabled = true;

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

            MainProgressBar.Value = 40;
            MainProgressBarDesc.Text = "2/5";

            var nick = string.Empty;
            using(var context = PDBContext.Instance.Context)
            {
                var data = context.UsersDatas.Where(q => !string.IsNullOrEmpty(q.AssignedTo) && q.AssignedTo.Equals(PDBContext.Instance.AccountID) == true).FirstOrDefault();
                if (data != null)
                    nick = data.Nick;
                else
                    nick = context.Accounts.Find(PDBContext.Instance.AccountID)?.Login;
            }

            MainProgressBar.Value = 60;
            MainProgressBarDesc.Text = "3/5";

            var properties = new DashboardProperties();
            MainTabControlManager = new TabControlManager(MainTabControl);
            MainTabControlManager.Add(properties);

            SetTitle(properties.Header?.Header?.Text);

            MainProgressBar.Value = 80;
            MainProgressBarDesc.Text = "3/5";

            MainAccountLogIn.Text = "Login: " + nick;
            MainStatus.Text = "Status: Ready";

            MainProgressBar.Value = 90;
            MainProgressBarDesc.Text = "4/5";

            Dispatcher.Invoke(async () =>
            {
                await Task.Delay(300);
            });

            MainProgressBar.Value = 0;
            MainProgressBarDesc.Text = string.Empty;
            MainProgressBarDesc.Visibility = Visibility.Collapsed;
        }

        public void Subscribe()
        {
            Closing += App.MainWindow_Closing;
            KeyUp += MainWindow_KeyUp;

            MainTabControlManager.TabControl.SelectionChanged += MainTabControl_SelectionChanged;

            MainFileCloseAllWindowsMenu.Click += MainFileCloseAllWindowsMenu_Click;
            MainFileSettingsMenu.Click += MainFileSettingsMenu_Click;
            MainFileLogOutMenu.Click += MainFileLogOutMenu_Click;
            MainFileExitMenu.Click += MainFileExitMenu_Click;

            MainNavigateDashboardMenu.Click += MainNavigateDashboardMenu_Click;
        }

        public void Dispose()
        {
            Closing -= App.MainWindow_Closing;
            KeyUp -= MainWindow_KeyUp;

            MainTabControlManager.TabControl.SelectionChanged -= MainTabControl_SelectionChanged;

            MainFileCloseAllWindowsMenu.Click -= MainFileCloseAllWindowsMenu_Click;
            MainFileSettingsMenu.Click -= MainFileSettingsMenu_Click;
            MainFileLogOutMenu.Click -= MainFileLogOutMenu_Click;
            MainFileExitMenu.Click -= MainFileExitMenu_Click;

            MainNavigateDashboardMenu.Click -= MainNavigateDashboardMenu_Click;

            MainTabControlManager.Clear();
            GC.Collect();
        }

        public void RefreshErrorInfo(int errorhandleCounter)
        {
            if(errorhandleCounter > 0)
            {
                MainErrorImage.Visibility = Visibility.Visible;
                MainErrorCounter.Visibility = Visibility.Visible;
                MainErrorCounter.Text = string.Format(WPF.Properties.Resources.ERROR_HANDLEINFO, errorhandleCounter);
            }
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            var tabitem = MainTabControl.SelectedContent as IPageModel;
            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}] on [{tabitem?.Header}] page");
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = MainTabControlManager.TabControl.SelectedIndex;
            if(index >= 0)
            {
                var properties = MainTabControlManager.TabControl.Items[index] as TabItem;
                if (properties != null)
                {
                    var tabName = (properties.Header as TabItemHeaderControl)?.Header?.Text;
                    SetTitle(tabName);

                    LogManager.Instance.LogToFile(new LogInfoModel
                    {
                        Type = FileTypesEnum.TRACE,
                        Message = "Set focus to page " + tabName
                    });
                }
            }
        }

        private void MainNavigateDashboardMenu_Click(object sender, RoutedEventArgs e)
        {
            MainTabControlManager.Add(new DashboardProperties());
        }

        private void MainFileExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainFileLogOutMenu_Click(object sender, RoutedEventArgs e)
        {
            var login = string.Empty;
            using(var context = PDBContext.Instance.Context)
            {
                var host = context.UsersHosts.Where(q => PDBContext.Instance.AccountID.Equals(q.AssignedTo)).FirstOrDefault();
                if(host != null)
                {
                    host.IsLoggedIn = false;
                    host.Update();
                    context.SaveChanges();
                }

                login = context.Accounts.Find(PDBContext.Instance.AccountID)?.Login;
            }
            PDBContext.Instance.AccountID = null;
            MainContext.Instance.Windows.Clear(new LoginProperties(login));
        }

        private void MainFileSettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            MainTabControlManager.Add(new SettingsProperties());
        }

        private void MainFileCloseAllWindowsMenu_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Clear(Properties);
        }

        private void SetTitle(string tabName)
        {
            Title = WPF.Properties.Resources.APP_NAME + " [" + tabName + "]";
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
