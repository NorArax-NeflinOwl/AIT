using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Managers.Helpers;
using WPF.Models.Enums;
using WPF.Models.Extensions;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Interfaces;
using WPF.GUI.Windows.Properties;
using System.Windows.Input;
using System.Windows.Controls;

namespace WPF.GUI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, IDisposableExtended, IPropertizableControl
    {
        public IProperties Properties { get; }
        public bool IsDisposed { get; set; }

        public LoginWindow(string login = "")
        {
            Properties = new LoginProperties(this);
            InitializeComponent();
            Init();
            Subscribe();

            if(!string.IsNullOrEmpty(login))
                LoginInputTextBox.Text = login;
        }

        public void Init()
        {
            WindowCentralizer.CenterWindowOnScreen(this);
            LoginImage.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\GUI\\Icons\\logo4x3.png"));
            LoginInputTextBox.Focus();

            // TODO Set login fields hits from WPF.Properties.Resources
        }

        public void Subscribe()
        {
            KeyUp += LoginWindow_KeyUp;
            Closing += App.MainWindow_Closing;
            LoginButton.Click += LoginButton_Click;
            LoginRegButton.Click += LoginRegButton_Click;
        }

        private void LoginWindow_KeyUp(object sender, KeyEventArgs e)
        {
            App.MainWindow_KeyUp(sender, e);
            if (e.Key.Equals(Key.Enter))
            {
                if (LoginInputTextBox.IsFocused)
                {
                    LoginInputPasswordBox.Focus();
                }
                else if (LoginInputPasswordBox.IsFocused)
                {
                    LoginButton_Click(null, null);
                }
            }
        }

        private void LoginRegButton_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Open(new RegistrationProperties());
            MainContext.Instance.Windows.Hide(((IWindowsProperties)Properties).WindowName);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginProgressBar.Visibility = Visibility.Visible;
            var login = LoginInputTextBox.Text;
            var password = string.IsNullOrEmpty(LoginInputPasswordBox.Password) ? "" : Generators.GenerateSha256Hash(LoginInputPasswordBox.Password);
            var rememberMe = LoginRememberCheckBox.IsChecked;

            try
            {
                if (string.IsNullOrEmpty(login))
                    throw new AitAccountExceptions.LoginException(WPF.Properties.Resources.LOGIN_EMPTY);
                
                if (string.IsNullOrEmpty(password))
                    throw new AitAccountExceptions.PasswordException(WPF.Properties.Resources.PASS_EMPTY); 

                using (var context = PDBContext.Instance.Context)
                {
                    var accounts = context.Accounts.Where(q => q.Login.Equals(login) && !q.Permition.Equals(PermitionAccountEnum.NONE)).ToList();
                    if (!accounts.Any())
                        throw new AitAccountExceptions.LoginException(WPF.Properties.Resources.LOGIN_NOEXIST); 

                    foreach (var account in accounts)
                    {
                        if (password.Equals(account.Password))
                        {
                            if (!account.IsActive)
                                throw new AitAccountExceptions.AccoutnNotActivatedException(WPF.Properties.Resources.ACC_NOACTIVATED); 

                            if(account.Permition <= (int)PermitionAccountEnum.BLOCK)
                                throw new AitAccountExceptions.AccoutnNotActivatedException(WPF.Properties.Resources.ACC_BLOCED);

                            PDBContext.Instance.AccountID = account.ID;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                        throw new AitAccountExceptions.PasswordException(WPF.Properties.Resources.PASS_NOFIND); 

                    var host = context.UsersHosts.Where(q => PDBContext.Instance.AccountID.Equals(q.AssignedTo) && HardwareManager.GetComputerName().Equals(q.HostName)).FirstOrDefault();
                    if (host != null)
                    {
                        if (!host.IsActive)
                            throw new AitAccountExceptions.HostException(WPF.Properties.Resources.HOST_LOCKED);

                        if (rememberMe != null)
                            host.IsLoggedIn = (bool)rememberMe;

                        host.Update();

                        var hostDate = context.HostDatas.Where(q => !string.IsNullOrEmpty(q.AssignedTo) && host.ID.Equals(q.AssignedTo)).FirstOrDefault();
                        if(hostDate == null)
                        {
                            CreateHostData(context, host);
                        }
                    }
                    else
                    {
                        var newhost = new AitUserHostModel(context)
                        {
                            AssignedTo = PDBContext.Instance.AccountID,
                            HostName = PDBContext.Instance.DeviceInfo.Software?.ComputerName,
                            IsLoggedIn = (bool)rememberMe
                        };
                        newhost.Insert();

                        CreateHostData(context, newhost);
                    }
                }

                if (string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                    throw new Exception(WPF.Properties.Resources.SAMETHING_WRONG);


                MainContext.Instance.Windows.Open(new PopupProperties(WPF.Properties.Resources.INFORMATION, WPF.Properties.Resources.LOGIN_SUCC, 2), false);

                DispatcherExtension.Invoke(async () =>
                {
                    await Task.Delay(1000);
                    MainContext.Instance.Windows.Open(new MainProperties());
                    MainContext.Instance.Windows.Hide(((IWindowsProperties)Properties).WindowName);
                });

            }
            catch(Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }
            LoginProgressBar.Visibility = Visibility.Collapsed;
        }

        private void CreateHostData(DBContext context, AitUserHostModel host)
        {
            var newHostDate = new AitHostDataModel(context)
            {
                UserHost = host,
                ComputerName = PDBContext.Instance.DeviceInfo.Software?.ComputerName,
                CurrentLanguage = PDBContext.Instance.DeviceInfo.Software?.CurrentLanguage,
                OSInfo = PDBContext.Instance.DeviceInfo.Software?.OSInfo,
                ProcessorInfo = PDBContext.Instance.DeviceInfo.Hardware?.ProcessorInfo,
                PhysicalMemory = PDBContext.Instance.DeviceInfo.Hardware?.PhysicalMemory
            };
            newHostDate.Insert();
        }

        public void Dispose()
        {
            KeyUp -= LoginWindow_KeyUp;
            Closing -= App.MainWindow_Closing;
            LoginButton.Click -= LoginButton_Click;
            LoginRegButton.Click -= LoginRegButton_Click;

            IsDisposed = true;
            GC.Collect();
        }
    }
}
