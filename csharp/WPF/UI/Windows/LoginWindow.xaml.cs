using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Managers.Helpers;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; }

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
            CenterWindowOnScreen();
            LoginImage.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\UI\\Icons\\logo4x3.png"));
            LoginInputTextBox.Focus();
        }

        public void Subscribe()
        {
            LoginButton.Click += LoginButton_Click;
            LoginRegButton.Click += LoginRegButton_Click;
        }

        private void LoginRegButton_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Open(new RegistrationProperties());
            MainContext.Instance.Windows.Close(Properties.WindowName);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginProgressBar.Visibility = Visibility.Visible;
            var login = LoginInputTextBox.Text;
            var password = Generators.GenerateSha256Hash(LoginInputPasswordBox.Password);
            var rememberMe = LoginRememberCheckBox.IsChecked;

            try
            {
                if (string.IsNullOrEmpty(login))
                    throw new Exception(WPF.Properties.Resources.LOGIN_EMPTY);
                if (string.IsNullOrEmpty(password))
                    throw new Exception(WPF.Properties.Resources.PASS_EMPTY); 

                using (var context = PDBContext.Instance.Context)
                {
                    var accounts = context.Accounts.Where(q => q.Login.Equals(login)).ToList();
                    if (!accounts.Any())
                        throw new Exception(WPF.Properties.Resources.LOGIN_NOEXIST); 

                    foreach (var account in accounts)
                    {
                        if (password.Equals(account.Password))
                        {
                            if (!account.IsActive)
                                throw new Exception(WPF.Properties.Resources.ACC_NOACTIVATED); 

                            PDBContext.Instance.AccountID = account.ID;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                        throw new Exception(WPF.Properties.Resources.PASS_NOFIND); 

                    var host = context.UsersHosts.Where(q => PDBContext.Instance.AccountID.Equals(q.AssignedTo) && HardwareManager.GetComputerName().Equals(q.HostName)).FirstOrDefault();
                    if (host != null)
                    {
                        if (!host.IsActive)
                            throw new Exception(WPF.Properties.Resources.HOST_LOCKED);

                        if (rememberMe != null)
                            host.IsLoggedIn = (bool)rememberMe;
                        host.Update();
                    }
                    else
                    {
                        var newhost = new AitUserHostModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.USH),
                            AssignedTo = PDBContext.Instance.AccountID,
                            HostName = HardwareManager.GetComputerName()
                        };
                        newhost.Insert();
                    }
                    context.SaveChanges();
                }

                if (string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                    throw new Exception(WPF.Properties.Resources.SAMETHING_WRONG); 

                MainContext.Instance.Windows.Open(new MainProperties());
                LoginProgressBar.Visibility = Visibility.Collapsed;
                MainContext.Instance.Windows.Close(Properties.WindowName);
            }
            catch(Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);

                throw; //TODO create dialog with error;
            }
        }

        public void Dispose()
        {
            LoginButton.Click -= LoginButton_Click;
            LoginRegButton.Click -= LoginRegButton_Click;

            GC.Collect();
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
