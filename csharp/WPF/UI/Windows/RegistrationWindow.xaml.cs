using System;
using System.Linq;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers.Helpers;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; }

        public RegistrationWindow()
        {
            Properties = new RegistrationProperties(this);
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            RegButton.Click -= RegButton_Click;
            GC.Collect();
        }

        public void Init()
        {
            CenterWindowOnScreen();
        }

        public void Subscribe()
        {
            RegButton.Click += RegButton_Click;
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

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            var login = RegLoginTextBox.Text;
            var password = RegPasswordBox.Password;
            var correctPassowrd = RegRepPasswordBox.Password.Equals(password);
            var email = RegEmailTextBox.Text;
            var nick = RegNickTextBox.Text;
            var first = RegFirstNameTextBox.Text;
            var middle = RegMidleNameTextBox.Text;
            var last = RegLastNameTextBox.Text;
            var bday = RegBDayPicker.SelectedDate;

            using(var context = PDBContext.Instance.Context)
            {
                if (correctPassowrd)
                {
                    AitAccountModel acccout = null;
                    if (!context.Accounts.Any(q => q.Login.Equals(login)))
                    {
                        acccout = new AitAccountModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.ACC),
                            Login = login,
                            Password = Generators.GenerateSha256Hash(password),
                            Email = email
                        };
                        acccout.Insert();
                    }
                    else
                    {
                        throw new Exception("Login is already exists in system, please pass another");
                    }

                    var userDate = new AitUserDataModel(context)
                    {
                        ID = Generators.RecordIDGenerator(TableInerfixEnum.USD),
                        AssignedTo = acccout?.ID,
                        Nick = nick,
                        FirstName = first,
                        MiddleName = middle,
                        LastName = last,
                        Birthday = bday
                    };
                    userDate.Insert();

                    context.SaveChanges();

                    // TODO Generate activation code -> save code in db -> send activation email

                    // ShowActivationPanel(); - if all correct
                }
                else
                {
                    throw new Exception("Passowd and repeat password is not identical");
                }
            }

            /*
             * TODO Move to Login window if user activated your account
             * 
            var window = MainContext.Instance.Windows.Window(WindowsNameEnum.LOGIN);
            if(window != null)
            {
                window.Properties.Add("Login", login);
            }

            MainContext.Instance.Windows.Show(WindowsNameEnum.LOGIN);
            MainContext.Instance.Windows.Close(Properties.WindowName);*/
        }

        private void ShowActivationPanel()
        {

        }
    }
}
