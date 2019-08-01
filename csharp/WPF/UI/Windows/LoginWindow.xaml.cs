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

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; }

        public LoginWindow()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            CenterWindowOnScreen();
            LoginImage.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\UI\\Icons\\logo4x3.png"));
        }

        public void Subscribe()
        {
            LoginButton.Click += LoginButton_Click;
            LoginRegButton.Click += LoginRegButton_Click;
        }

        private void LoginRegButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginInputTextBox.Text;
            var password = Generators.GenerateSha256Hash(LoginInputPasswordBox.Password);
            var rememberMe = LoginRememberCheckBox.IsChecked;

            try
            {
                if (string.IsNullOrEmpty(login))
                    throw new Exception("Login is empty!"); //TODO resources
                if (string.IsNullOrEmpty(password))
                    throw new Exception("Password is empty!"); //TODO resources

                using (var context = PDBContext.Instance.Context)
                {
                    var accounts = context.Accounts.Where(q => q.Login.Equals(login)).ToList();
                    if (!accounts.Any())
                        throw new Exception("Account with this login not find!"); //TODO resources

                    foreach (var account in accounts)
                    {
                        if (password.Equals(account.Password))
                        {
                            if (!account.IsActive)
                                throw new Exception("Account is not activated!"); //TODO resources

                            PDBContext.Instance.AccountID = account.ID;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                        throw new Exception("Account with this password not find!"); //TODO resources

                    var host = context.UsersHosts.Where(q => PDBContext.Instance.AccountID.Equals(q.AssignedTo) && HardwareManager.GetComputerName().Equals(q.HostName)).FirstOrDefault();
                    if (host != null)
                    {
                        if (!host.IsActive)
                            throw new Exception("Your host was locked by admin!"); //TODO resources

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
                    throw new Exception("Samethings go wrong!"); //TODO resources

                MainContext.Instance.Windows.Open(new LoginWindow());
                MainContext.Instance.Windows.Close(this);
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
