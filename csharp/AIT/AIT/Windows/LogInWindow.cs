using AITLib.Constants;
using AIT.DataBases;
using AIT.Helpers;
using AIT.Interfaces;
using AIT.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using AITLib.Helpers;
using System.Text;
using System.Threading.Tasks;
using AIT.DataBases.DBModel;

namespace AIT.Windows
{
    public partial class LogInWindow : Window, ISubscribe, IDisposable
    {
        public LogInWindow()
        {
            InitializeComponent();

            Init();
            Subscribe();
        }

        public void Init()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            aitLoginTextButton.Text = AitStrings.LOGIN;
            aitRegistrationTextButton.Text = AitStrings.REGISTRATION;

            aitLoginTextBox.Focus();

            if (CheckMemory(out AitPerson person) && person != null)
            {
                IsEnabled = false;
                aitProgressGrid.Visibility = Visibility.Visible;

                AutoLogin(person);
                aitProgressGrid.Visibility = Visibility.Collapsed;
            }
        }

        public void Subscribe()
        {
            aitLoginButton.Click += AitLoginButton_Click;
            aitRegistrationButton.Click += AitRegistrationButton_Click;
        }

        private async void AitLoginButton_Click(object sender, RoutedEventArgs e)
        {
            aitProgressGrid.Visibility = Visibility.Visible;
            try
            {
                bool error = false;
                string msg = string.Empty;
                if (string.IsNullOrEmpty(aitPasswordBox.Password))
                {
                    msg = AitStrings.EMPTY_PASSWORD;
                    error = true;
                }
                if (string.IsNullOrEmpty(aitLoginTextBox.Text))
                {
                    msg = AitStrings.EMPTY_LOGIN;
                    error = true;
                }

                if(error)
                {
                    if(AitDebugHelper.RunningInDebugMode())
                    {
                        AitPerson admin = await AitAdminManager.CreateAdminIfNotExist();
                        AutoLogin(admin);
                    }
                    else
                    {
                        var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                        var dialog = new DialogWindow(dialogModel);
                        dialog.Show();
                    }
                    return;
                }

                var persons = AitDBContextInstance.Instance.AitPersons.Where(p => p.Login == aitLoginTextBox.Text).ToDictionary(n => n.ID);

                if (persons != null && persons.Count > 0)
                {
                    foreach (var person in persons)
                    {
                        if (AitGenerators.VerifyMD5Hash(aitPasswordBox.Password, person.Value.Password))
                        {
                            if (aitRememberMeCheckBox.IsChecked == true)
                            {
                                AitRememberHelper.CreateMemoryLogin(person.Value);
                            }
                            else
                            {
                                AitFileManager.DeleteFile(Path.Combine(AitStrings.LOCALCACHE_SUBPATH, AitStrings.REMEMEBER_ME_FILE + AitStrings.AIT_EXT));
                            }

                            LogInSuccessful(person.Key);
                            return;
                        }
                        else
                        {
                            msg = AitStrings.WRONG_PASSWORD;
                            var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                            var dialog = new DialogWindow(dialogModel);
                            dialog.Show();
                        }
                    }
                }
                else
                {
                    msg = AitStrings.LOGIN_NOT_FOUND;
                    var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                    var dialog = new DialogWindow(dialogModel);
                    dialog.Show();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(AitStrings.LOGIN_CRASH);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
            }
            finally
            {
                aitProgressGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void AutoLogin(AitPerson person)
        {
            aitLoginTextBox.Text = person.Login;
            aitPasswordBox.Password = AitStrings.SIMPLE_PASSWORD;
            LogInSuccessful(person.ID);
        }

        private void LogInSuccessful(int id)
        {
            AitSessionManager.GetSession.CreateSession(id);
            //await Task.Delay(TimeSpan.FromSeconds(0.1));
            var mainWindows = new MainWindow(this);
            mainWindows.Show();
            Visibility = Visibility.Collapsed;
        }

        private void AitRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var registration = new RegistrationWindow();
            registration.Show();
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.Collect();
        }

        public void Unsubscribe()
        {
            aitLoginButton.Click -= AitLoginButton_Click;
            aitRegistrationButton.Click -= AitRegistrationButton_Click;
        }

        private bool CheckMemory(out AitPerson person)
        {
            try
            {
                var manager = new AitFileManager();
                var path = Path.Combine(AitStrings.LOCALCACHE_SUBPATH, AitStrings.REMEMEBER_ME_FILE + AitStrings.AIT_EXT);
                var content = manager.ReadFile(path);

                var per = AitCryptJsonManager.Instance.Deserialize<AitPerson>(content);
                if (per != null)
                {
                    var persons = AitDBContextInstance.Instance.AitPersons.Where(p => p.Login == per.Login).ToDictionary(n => n.ID);

                    if (persons != null && persons.Count > 0)
                    {
                        foreach (var p in persons)
                        {
                            if (per.Password.Equals(p.Value.Password))
                            {
                                person = per;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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
            person = null;
            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            try
            {
                Application.Current.Shutdown();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });
                Environment.Exit(0);
            }
        }
    }
}
