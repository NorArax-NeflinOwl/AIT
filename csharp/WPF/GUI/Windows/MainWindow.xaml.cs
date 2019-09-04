using System;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;
using WPF.GUI.Windows.Properties;
using WPF.Managers;
using WPF.GUI.Pages.Properties;
using WPF.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.GUI.Controls;
using WPF.Models.Enums;

namespace WPF.GUI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposableExtended, IPropertizableControl
    {
        public IProperties Properties { get; }

        public TabControlManager MainTabControlManager { get; set; }

        public bool IsDisposed { get; set; }

        public MainWindow()
        {
            Properties = new MainProperties(this);
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            CenterWindowOnScreen();
            SetProgressVisibility(true, WPF.Properties.Resources.INIT, true);

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
            MainViewShowAllMenu.Header = WPF.Properties.Resources.SHOWALL_HEADER;
            MainViewShowAllMenu.IsEnabled = true;
            MainViewNewNoteMenu.Header = WPF.Properties.Resources.NEWNOTE_HEADER;

            MainNavigateMenu.Header = WPF.Properties.Resources.NAV_HEADER;
            MainNavigateDashboardMenu.Header = WPF.Properties.Resources.DASHBOARD_HEADER;
            MainNavigateDashboardMenu.IsEnabled = true;
            MainNavigateNoteManagerMenu.Header = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            MainNavigateNoteManagerMenu.IsEnabled = true;

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

            var nick = string.Empty;
            using (var context = PDBContext.Instance.Context)
            {
                var data = context.UsersDatas.Where(q => !string.IsNullOrEmpty(q.AssignedTo) && q.AssignedTo.Equals(PDBContext.Instance.AccountID) == true).FirstOrDefault();
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.Nick))
                        nick = data.Nick;
                    else
                        nick = data.FullName;
                }
                else
                    nick = context.Accounts.Find(PDBContext.Instance.AccountID)?.Login;
            }

            var properties = new NoteManagerProperties();
            MainTabControlManager = new TabControlManager(MainTabControl);
            MainTabControlManager.Add(properties);

            SetTitle(properties.Header?.Header?.Text);

            MainAccountLogIn.Text = "Login: " + nick;
        }

        public void Subscribe()
        {
            Closing += App.MainWindow_Closing;
            KeyUp += MainWindow_KeyUp;

            MainTabControlManager.TabControl.SelectionChanged += MainTabControl_SelectionChanged;

            MainViewShowAllMenu.Click += MainViewShowAllMenu_Click;

            MainFileCloseAllWindowsMenu.Click += MainFileCloseAllWindowsMenu_Click;
            MainFileSettingsMenu.Click += MainFileSettingsMenu_Click;
            MainFileLogOutMenu.Click += MainFileLogOutMenu_Click;
            MainFileExitMenu.Click += MainFileExitMenu_Click;

            MainNavigateDashboardMenu.Click += MainNavigateDashboardMenu_Click;
            MainNavigateNoteManagerMenu.Click += MainNavigateNoteManagerMenu_Click;

            SetProgressVisibility(false);
        }

        public void Dispose()
        {
            Closing -= App.MainWindow_Closing;
            KeyUp -= MainWindow_KeyUp;

            MainTabControlManager.TabControl.SelectionChanged -= MainTabControl_SelectionChanged;

            MainViewShowAllMenu.Click -= MainViewShowAllMenu_Click;

            MainFileCloseAllWindowsMenu.Click -= MainFileCloseAllWindowsMenu_Click;
            MainFileSettingsMenu.Click -= MainFileSettingsMenu_Click;
            MainFileLogOutMenu.Click -= MainFileLogOutMenu_Click;
            MainFileExitMenu.Click -= MainFileExitMenu_Click;

            MainNavigateDashboardMenu.Click -= MainNavigateDashboardMenu_Click;
            MainNavigateNoteManagerMenu.Click -= MainNavigateNoteManagerMenu_Click;

            MainTabControlManager.Clear();
            IsDisposed = true;
            GC.Collect();
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            var index = 0;
            if(MainTabControlManager.TabControl.SelectedIndex > index)
            {
                index = MainTabControlManager.TabControl.SelectedIndex;
            }

            var tabitem = MainTabControlManager.TabControl.Items[index] as TabItem;
            var ctrl = tabitem?.Header as TabItemHeaderControl;

            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}] release on [{ctrl?.Header?.Text}] page in MainWindow");
        }

        private void MainViewShowAllMenu_Click(object sender, RoutedEventArgs e)
        {
            SetProgressVisibility(true, WPF.Properties.Resources.SHOWALL_HEADER);
            if(MainContext.Instance.Windows.Show())
                MainContext.Instance.Windows.Open(new PopupProperties(WPF.Properties.Resources.INFORMATION, WPF.Properties.Resources.REOPEN_SOME_WINDOW, 2), false);
            else
                MainContext.Instance.Windows.Open(new PopupProperties(WPF.Properties.Resources.INFORMATION, WPF.Properties.Resources.REOPEN_ZERO_WINDOW, 2), false);

            SetProgressVisibility(false);
        }

        private void MainNavigateNoteManagerMenu_Click(object sender, RoutedEventArgs e)
        {
            SetProgressVisibility(true, WPF.Properties.Resources.NOTEMANAGER_HEADER);
            MainTabControlManager.Add(new NoteManagerProperties());
            SetProgressVisibility(false);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var info = string.Empty;
            try
            {
                foreach (NoteListViewItemControl item in e.AddedItems)
                {
                    if (item != null)
                        info += "[" + item.Note.Type.ToString() + "] " + item.Note.Name.ToString() + ", ";
                }
                info = info.Substring(0, info.Length - 2);
            }
            catch (Exception) { }

            SetProgressVisibility(true, info);
            var index = MainTabControlManager.TabControl.SelectedIndex;
            if(index >= 0)
            {
                if (MainTabControlManager.TabControl.Items[index] is TabItem properties)
                {
                    var tabName = (properties.Header as TabItemHeaderControl)?.Header?.Text;
                    SetTitle(tabName);

                    LogManager.Instance.LogToFile(new LogInfoModel
                    {
                        Type = FileTypesEnum.TRACE,
                        MessageInfo = new MessageInfoModel("Set focus to page " + tabName)
                    });
                }
            }
            SetProgressVisibility(false);
        }

        private void MainNavigateDashboardMenu_Click(object sender, RoutedEventArgs e)
        {
            SetProgressVisibility(true, WPF.Properties.Resources.DASHBOARD_HEADER);
            MainTabControlManager.Add(new DashboardProperties());
            SetProgressVisibility(false);
        }

        private void MainFileExitMenu_Click(object sender, RoutedEventArgs e)
        {
            SetProgressVisibility(true);
            Close();
        }

        private void MainFileLogOutMenu_Click(object sender, RoutedEventArgs e)
        {
            SetProgressVisibility(true, WPF.Properties.Resources.LOGOUT_HEADER);
            var login = string.Empty;
            using(var context = PDBContext.Instance.Context)
            {
                var host = context.UsersHosts.Where(q => PDBContext.Instance.AccountID.Equals(q.AssignedTo)).FirstOrDefault();
                if(host != null)
                {
                    host.IsLoggedIn = false;
                    host.Update();
                }

                login = context.Accounts.Find(PDBContext.Instance.AccountID)?.Login;
            }
            PDBContext.Instance.AccountID = null;

            MainContext.Instance.Windows.Open(new PopupProperties(WPF.Properties.Resources.INFORMATION, WPF.Properties.Resources.LOGOUT_SUCC, 2), false);
            SetProgressVisibility(false);
            MainContext.Instance.Windows.Clear(new LoginProperties(login));

        }

        private void MainFileSettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            SetProgressVisibility(true, WPF.Properties.Resources.SETTINGS_HEADER);
            MainTabControlManager.Add(new SettingsProperties());
            SetProgressVisibility(false);
        }

        private void MainFileCloseAllWindowsMenu_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Clear((IWindowsProperties) Properties);
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

        private void SetProgressVisibility(bool show, string info = "", bool proccess = false)
        {
            if (show)
            {
                MainStatus.Text = string.Format(proccess ? WPF.Properties.Resources.STATUS_PROCESSING_S : WPF.Properties.Resources.STATUS_OPENING_S, info);
                MainProgressGrid.Visibility = Visibility.Visible;
            }
            else
            {
                Dispatcher.Invoke(async () =>
                {
                    await Task.Delay(1000);
                    MainStatus.Text = string.Format(WPF.Properties.Resources.STATUS_READY, info);
                    MainProgressGrid.Visibility = Visibility.Collapsed;
                });
            }
        }
    }
}
