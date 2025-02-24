using AIT.DataBases;
using AIT.Helpers;
using AIT.Interfaces;
using AIT.Models;
using AIT.Pages.PartlyPages;
using AIT.Windows;
using AITLib.Constants;
using AITLib.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class AccountPage : Page, ISubscribe, IDisposable
    {
        private AccountFieldFrame frame;
        private Action<bool, object> m_Question;
        private readonly MainWindow mainWindow;

        public AccountPage()
        {
            InitializeComponent();
            Init();
        }

        public AccountPage(Window window)
        {
            mainWindow = window as MainWindow;
            InitializeComponent();
            Init();
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.Collect();
        }

        public async void Init()
        {
            mainWindow.aitProgressGrid.Visibility = Visibility.Visible;
            frame = new AccountFieldFrame();

            aitAccountFrame.Content = frame;
            frame.PersonAccountInit();

            Subscribe();
            await Task.Delay(TimeSpan.FromSeconds(1));
            mainWindow.aitProgressGrid.Visibility = Visibility.Collapsed;
        }

        public void Subscribe()
        {
            aitSaveBtn.Click += AitSaveBtn_Click;
            aitDeleteAccountBtn.Click += AitDeleteAccountBtn_Click;
            m_Question += DeleteAccount;
        }

        private async void DeleteAccount(bool obj, object param)
        {
            if(obj && param == null)
            {
                try
                {
                    var person = AitSessionManager.GetSession.Person;
                    if (person != null)
                    {
                        AitDBContextInstance.Instance.AitPersons.Remove(person);
                        await AitDBContextInstance.Instance.SaveChangesAsync();
                    }
                    mainWindow.LogOut();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                    AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(AitStrings.DELETE_ACCOUNT_CRASH);
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                    var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                    error.Show();
                }
            }
        }

        private void AitDeleteAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            var msg = AitStrings.DELETE_ACC_QUE;
            var dialog = new DialogWindow(new AitDialogModel(DialogType.QUESTION, msg), m_Question);
            dialog.Show();
        }

        private async void AitSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.aitProgressGrid.Visibility = Visibility.Visible;
            if(frame.ControlsValidation())
            {
                await frame.SavePersonAccountChanges();
                frame.Refresh();
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            mainWindow.aitProgressGrid.Visibility = Visibility.Collapsed;
        }

        public void Unsubscribe()
        {
            aitSaveBtn.Click -= AitSaveBtn_Click;
            aitDeleteAccountBtn.Click -= AitDeleteAccountBtn_Click;
            m_Question -= DeleteAccount;
        }
    }
}
