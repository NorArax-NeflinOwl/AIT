using System;
using System.Linq;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers.Helpers;
using WPF.Models.Enums;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, IDisposable, IPropertizableWindow
    {
        private AitAccountModel account;
        private AitFilesModel userActivatedCodeFile;

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
            Closing -= App.MainWindow_Closing;
            RegButton.Click -= RegButton_Click;
            RegActButton.Click -= RegActButton_Click;
            RegActRepSendButton.Click -= RegActRepSendButton_Click;
            GC.Collect();
        }

        public void Init()
        {
            CenterWindowOnScreen();
            RegLoginTextBox.Focus();
        }

        public void Subscribe()
        {
            Closing += App.MainWindow_Closing;
            RegButton.Click += RegButton_Click;
            RegAct2Button.Click += RegAct2Button_Click;
            RegActButton.Click += RegActButton_Click;
            RegActRepSendButton.Click += RegActRepSendButton_Click;
        }

        private void RegAct2Button_Click(object sender, RoutedEventArgs e)
        {
            var login = RegLoginTextBox.Text;
            if (string.IsNullOrEmpty(login))
                throw new AitAccountExceptions.LoginException(WPF.Properties.Resources.LOGIN_EMPTY);

            using (var context = PDBContext.Instance.Context)
            {
                account = context.Accounts.Where(q => q.Login.Equals(login)).FirstOrDefault();
                if (account == null)
                    throw new AitAccountExceptions.LoginException(WPF.Properties.Resources.LOGIN_NOEXIST); 

                userActivatedCodeFile = context.Files.Where(q => q.Creator.Equals(account.ID) && q.Type.Equals(FileTypesEnum.ACTIVATION_CODE)).FirstOrDefault();
                if (userActivatedCodeFile == null)
                    throw new AitAccountExceptions.CodeException(WPF.Properties.Resources.CODE_NOTFIND); 
            }

            RegMainGrid.Visibility = Visibility.Collapsed;
            RegActGrid.Visibility = Visibility.Visible;
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

        private void RegActRepSendButton_Click(object sender, RoutedEventArgs e)
        {
            MailSender.SendActivationCodeTo(account.Email, userActivatedCodeFile.Content);
        }

        private void RegActButton_Click(object sender, RoutedEventArgs e)
        {
            var code = RegActCodeTextBox.Text;

            if (string.IsNullOrEmpty(code))
                throw new AitAccountExceptions.CodeException(WPF.Properties.Resources.CODE_EMPTY);
            if (!userActivatedCodeFile.Content.ToLower().Equals(code.ToLower()))
                throw new AitAccountExceptions.CodeException(WPF.Properties.Resources.CODE_INCORECT);

            account = (AitAccountModel)account.Clone();
            account.IsActive = true;
            account.Update();
            account.Context.SaveChanges();
            
            MainContext.Instance.Windows.Open(new LoginProperties(account.Login));
            MainContext.Instance.Windows.Close(Properties.WindowName);
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
                    if (!context.Accounts.Any(q => q.Login.Equals(login)))
                    {
                        account = new AitAccountModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.ACC),
                            Login = login,
                            Password = Generators.GenerateSha256Hash(password),
                            Email = email
                        };
                        account.Insert();
                    }
                    else
                    {
                        throw new AitAccountExceptions.LoginException(WPF.Properties.Resources.LOGIN_EXIST);
                    }

                    if(account != null)
                    {
                        var userDate = new AitUserDataModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.USD),
                            AssignedTo = account.ID,
                            Nick = nick,
                            FirstName = first,
                            MiddleName = middle,
                            LastName = last,
                            Birthday = bday
                        };
                        userDate.Insert();

                        userActivatedCodeFile = new AitFilesModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                            Creator = account.ID,
                            Name = string.Format(WPF.Properties.Resources.ACT_CODE_FOR, account.Login),
                            Type = FileTypesEnum.ACTIVATION_CODE,
                            Content = Generators.GenerateActivateCode(account.ToString())
                        };
                        userActivatedCodeFile.Insert();

                        context.SaveChanges();

                        if (MailSender.SendActivationCodeTo(account.Email, userActivatedCodeFile.Content))
                            ShowActivationPanel();
                        else
                            throw new AitAccountExceptions.EmailException(WPF.Properties.Resources.WEB_CONNECTION_ERROR);
                    }
                }
                else
                {
                    throw new AitAccountExceptions.PasswordException(WPF.Properties.Resources.PASS_REPPASS_INCORECT);
                }
            }
        }

        private void ShowActivationPanel()
        {
            RegMainGrid.Visibility = Visibility.Collapsed;
            RegActGrid.Visibility = Visibility.Visible;
            RegActCodeTextBox.Focus();
        }
    }
}
