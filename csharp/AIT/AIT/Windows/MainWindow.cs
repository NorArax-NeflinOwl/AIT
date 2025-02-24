using AITLib.Constants;
using AITLib.Helpers;
using AIT.Helpers;
using AIT.Interfaces;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using AIT.Models;
using System.Text;
using System.Diagnostics;
using AIT.Pages;

namespace AIT.Windows
{
    public partial class MainWindow : Window, ISubscribe, IDisposable
    {
        private readonly LogInWindow m_LoginWindow;
        private Action<bool, object> m_Question;
        private Dictionary<string, Page> m_PagesDic;

        public Action Refresh { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public MainWindow(Window login)
        {
            m_LoginWindow = login as LogInWindow;

            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            try
            {
                var person = AitSessionManager.GetSession.Person;
                if(AitDebugHelper.RunningInDebugMode())
                {
                    aitUserFullName.Text = AitAdminManager.Title(person);
                }
                else
                {
                    aitUserFullName.Text = $"{person.PersonalDetails.FName} {person.PersonalDetails.LName}";
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(AitStrings.INIT_CRASH);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
            }

            if(AitFeatureManager.Instance.Disable_Feature[Feature.SERIALIZATION_SESSION])
                CheckCache();
            else
                InitDefaultDic();
        }

        public void Subscribe()
        {
            aitOpenSideMenu.Click += AitOpenSideMenu_Click;
            aitCloseSideMenu.Click += AitCloseSideMenu_Click;
            aitLogOutBtn.Click += AitLogOutBtn_Click;
            aitMenuList.SelectionChanged += AitMenuList_SelectionChanged;
            aitAccountBtn.Click += AitAccountBtn_Click;
            aitSettingsBtn.Click += AitSettingsBtn_Click;
            aitHelpBtn.Click += AitHelpBtn_Click;

            m_Question += SaveQuestion;
            m_Question += InitQuestion;
        }

        public void LogOut()
        {
            Visibility = Visibility.Collapsed;
            AitSessionManager.GetSession.Clear();
            m_LoginWindow.Visibility = Visibility.Visible;
            m_LoginWindow.aitProgressGrid.Visibility = Visibility.Collapsed;

            try
            {
                AitFileManager.DeleteFile(Path.Combine(AitStrings.LOCALCACHE_SUBPATH, AitStrings.REMEMEBER_ME_FILE + AitStrings.AIT_EXT));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(AitStrings.CACHE_CRASH);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
            }
        }

        public void CloseApp()
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });
                Environment.Exit(0);
            }
        }

        private void AitHelpBtn_Click(object sender, RoutedEventArgs e)
        {
            aitMainWindowPage.Content = m_PagesDic[AitStrings.HELP];
            Title = aitTitleMainWindow.Text = AitStrings.HELP;
        }

        private void AitSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            aitMainWindowPage.Content = m_PagesDic[AitStrings.SETTINGS];
            Title = aitTitleMainWindow.Text = AitStrings.SETTINGS;
        }

        private void AitAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            aitMainWindowPage.Content = m_PagesDic[AitStrings.ACCOUNT];
            Title = aitTitleMainWindow.Text = AitStrings.ACCOUNT;
        }

        private void AitMenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(aitMenuList.SelectedIndex)
            {
                case 0:
                    aitMainWindowPage.Content = m_PagesDic[AitStrings.DASHBOARD];
                    Title = aitTitleMainWindow.Text = AitStrings.DASHBOARD;
                    break;
                case 1:
                    aitMainWindowPage.Content = m_PagesDic[AitStrings.SCHEDULER];
                    Title = aitTitleMainWindow.Text = AitStrings.SCHEDULER;
                    break;
                case 2:
                    aitMainWindowPage.Content = m_PagesDic[AitStrings.VAT_CAL];
                    Title = aitTitleMainWindow.Text = AitStrings.VAT_CAL;
                    break;
                case 3:
                    aitMainWindowPage.Content = m_PagesDic[AitStrings.CALENDAR];
                    Title = aitTitleMainWindow.Text = AitStrings.CALENDAR;
                    break;
                case 4:
                    OpenQuickNotesExplorer();
                    new QuickNoteWindow(Refresh);
                    break;
            }
            aitMenuList.SelectedIndex = -1;
        }

        private void AitLogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }

        private void AitCloseSideMenu_Click(object sender, RoutedEventArgs e)
        {
            aitOpenSideMenu.Visibility = Visibility.Visible;
            aitCloseSideMenu.Visibility = Visibility.Collapsed;
        }

        private void AitOpenSideMenu_Click(object sender, RoutedEventArgs e)
        {
            aitOpenSideMenu.Visibility = Visibility.Collapsed;
            aitCloseSideMenu.Visibility = Visibility.Visible;
        }
        
        private void SaveQuestion(bool obj, object param)
        {
            if (obj && param == null)
            {
                if(AitFeatureManager.Instance.Disable_Feature[Feature.SERIALIZATION_SESSION])
                {
                    try
                    {
                        var path = Path.Combine(AitStrings.LOCALCACHE_SUBPATH, AitStrings.CACHE_FILE);
                        var manager = new AitFileManager();
                        var hash = AitCryptJsonManager.Instance.Serialize(m_PagesDic);
                        manager.WriteToFile(path, hash);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                        var stringBuilder = new StringBuilder();
                        stringBuilder.Append(AitStrings.CACHE_CRASH);
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                        var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                        error.Show();
                    }
                }
                else
                {
                    var msg = string.Format(AitStrings.DISABLE_FEATURE, AitStrings.SERIALIZATION_SESSION);
                    var dialog = new DialogWindow(new AitDialogModel(DialogType.INFORMATION, msg), null, AitStrings.CLOSE);
                    dialog.Show();
                    return;
                }
            }
            CloseApp();
        }

        private void InitQuestion(bool obj, object param)
        {
            if(obj && param is Dictionary<string, Page> dic)
            {
                m_PagesDic = dic;
            }
            else if(!obj && param is Dictionary<string, Page>)
            {
                InitDefaultDic();
            }
        }

        public void Unsubscribe()
        {
            aitOpenSideMenu.Click -= AitOpenSideMenu_Click;
            aitCloseSideMenu.Click -= AitCloseSideMenu_Click;
            aitLogOutBtn.Click -= AitLogOutBtn_Click;
            aitMenuList.SelectionChanged -= AitMenuList_SelectionChanged;
            aitAccountBtn.Click -= AitAccountBtn_Click;
            aitSettingsBtn.Click -= AitSettingsBtn_Click;
            aitHelpBtn.Click -= AitHelpBtn_Click;
            m_Question -= InitQuestion;
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.Collect();
        }

        protected override void OnClosed(EventArgs e)
        {
            if(AitSessionManager.GetSession.HasChanges)
            {
                var msg = AitStrings.SAVE_QUESTION;
                var dialog = new DialogWindow(new AitDialogModel(DialogType.QUESTION, msg), m_Question);
                dialog.Show();
            }
            else
            {
                CloseApp();
            }
            base.OnClosed(e);
        }

        private void InitDefaultDic()
        {
            m_PagesDic = new Dictionary<string, Page>
            {
                [AitStrings.ACCOUNT] = new AccountPage(this),
                [AitStrings.CALENDAR] = new CalendarPage(this),
                [AitStrings.DASHBOARD] = new DashboardPage(this),
                [AitStrings.HELP] = new HelpPage(this),
                [AitStrings.SCHEDULER] = new SchedulerPage(this),
                [AitStrings.SETTINGS] = new SettingsPage(this),
                [AitStrings.VAT_CAL] = new VatCalculatorPage(this),
                [AitStrings.QUICK_NOTES_EXPLORER] = new QuickNoteExplorerPage(this),
            };
        }

        private void CheckCache()
        {
            try
            {
                var manager = new AitFileManager();
                var path = Path.Combine(AitStrings.LOCALCACHE_SUBPATH, AitStrings.CACHE_FILE + AitStrings.AIT_EXT);
                var content = manager.ReadFile(path);
                var dic = AitCryptJsonManager.Instance.Deserialize<Dictionary<string, Page> >(content);
                if (dic != null)
                {
                    var msg = AitStrings.INIT_QUESTION;
                    var dialog = new DialogWindow(new AitDialogModel(DialogType.QUESTION, msg), m_Question, dic);
                    dialog.Show();
                }
                else
                {
                    InitDefaultDic();
                }
                return;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(AitStrings.CACHE_CRASH);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
            }
            InitDefaultDic();
        }

        private void OpenQuickNotesExplorer()
        {
            aitMainWindowPage.Content = m_PagesDic[AitStrings.QUICK_NOTES_EXPLORER];
            Title = aitTitleMainWindow.Text = AitStrings.QUICK_NOTES_EXPLORER;
        }
    }
}
