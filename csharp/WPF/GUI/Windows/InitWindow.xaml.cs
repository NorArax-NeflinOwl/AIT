using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;
using WPF.GUI.Windows.Properties;
using WPF.Managers.Helpers;
using WPF.Databases.Models;
using System.Configuration;
using System.Collections.Generic;

namespace WPF.GUI.Windows
{
    /// <summary>
    /// Interaction logic for InitWindow.xaml
    /// </summary>
    public partial class InitWindow : Window, IDisposableExtended, IPropertizableControl
    {
        public IProperties Properties { get; }
        public bool IsDisposed { get; set; }

        public InitWindow()
        {
            Properties = new InitProperties(this);
            MainContext.Instance.Windows.Open((IWindowsProperties)Properties);

            InitializeComponent();
            Init();
            Focus();
        }

        public void Subscribe()
        {
        }

        public void Init()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            WindowCentralizer.CenterWindowOnScreen(this);
            InitMessage.Text = WPF.Properties.Resources.APP_START;
            InitImage.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\GUI\\Icons\\logo4x3.png"));

            DispatcherExtension.Invoke(async () =>
            {
                await Task.Delay(200);
                while (BackgroundTasksManager.Instance.Completed != BackgroundTasksManager.Instance.Count)
                {
                    if (BackgroundTasksManager.Instance.Completed != 0)
                    {
                        InitMessage.Text = WPF.Properties.Resources.APP_INIT;
                    }
                }
                InitMessage.Text = WPF.Properties.Resources.APP_STARTCOMPLETED;
                await Task.Delay(200);

                var host = HardwareManager.GetComputerName();
                using(var context = PDBContext.Instance.Context)
                {
                    var userHost = context.UsersHosts.Where(q => host.Equals(q.HostName) && q.IsActive && q.IsLoggedIn && !string.IsNullOrEmpty(q.AssignedTo)).FirstOrDefault();

                    DeserializeSession(context, userHost?.AssignedTo);

                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds < 1000)
                    {
                        await Task.Delay(1000);
                    }

                    if (userHost != null)
                    {
                        PDBContext.Instance.AccountID = userHost.AssignedTo;
                        MainContext.Instance.Windows.Open(new MainProperties());
                    }
                    else
                    {
                        MainContext.Instance.Windows.Open(new LoginProperties());
                    }
                    MainContext.Instance.Windows.Close(((IWindowsProperties) Properties).WindowName);
                }
            });
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        private void DeserializeSession(DBContext context, string accountID)
        {
            if(!string.IsNullOrEmpty(accountID))
            {
                AitAccountModel fileCreator = null;
                var creator = ConfigurationManager.AppSettings["TasksManager"].ToString();
                if (!string.IsNullOrEmpty(creator))
                {
                    fileCreator = context.Accounts.Where(q => q.ID.Equals(creator)).FirstOrDefault();
                }

                var sessionFile = context.Files.Where(q => !string.IsNullOrEmpty(q.Creator) && q.Creator.Equals(creator)
                                                                && !string.IsNullOrEmpty(q.AssignedTo) && q.AssignedTo.Equals(accountID)
                                                                && q.Name.Equals("SessionDictionary")).FirstOrDefault();

                if(sessionFile != null)
                {
                    var dictionary = CryptoJsonManager.Instance.Deserialize<Dictionary<string, string>>(sessionFile.Content);
                    if(dictionary != null)
                    {
                        PDBContext.Instance.SessionDictionary = dictionary;
                    }
                    sessionFile.Delete();
                }
            }
        }
    }
}
