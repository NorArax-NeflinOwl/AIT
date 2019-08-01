using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.Models.Interfaces;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for InitWindow.xaml
    /// </summary>
    public partial class InitWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; }

        public InitWindow()
        {
            MainContext.Instance.Windows.Open(this);

            InitializeComponent();
            Init();
        }

        public void Subscribe()
        {
        }

        public void Init()
        {
            CenterWindowOnScreen();
            InitMessage.Text = WPF.Properties.Resources.APP_START;
            InitImage.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\UI\\Icons\\logo4x3.png"));

            Dispatcher.Invoke(async () =>
            {
                InitProgressBar.Visibility = Visibility.Visible;
                await Task.Delay(200);
                while (BackgroundTasksManager.Instance.Completed != BackgroundTasksManager.Instance.Count)
                {
                    var multiple = 100 / BackgroundTasksManager.Instance.Count;
                    var count = BackgroundTasksManager.Instance.Count;
                    var completed = BackgroundTasksManager.Instance.Completed;

                    if (completed != 0)
                    {
                        InitMessage.Text = WPF.Properties.Resources.APP_INIT;
                        //InitProgressBar.Value = (count / completed) * multiple;
                    }
                }
                //InitProgressBar.Value = 100;
                InitMessage.Text = WPF.Properties.Resources.APP_STARTCOMPLETED;
                await Task.Delay(200);

                var host = HardwareManager.GetComputerName();
                using(var context = PDBContext.Instance.Context)
                {
                    var userHost = context.UsersHosts.Where(q => host.Equals(q.HostName) && q.IsActive && q.IsLoggedIn && !string.IsNullOrEmpty(q.AssignedTo)).FirstOrDefault();
                    if (userHost != null)
                    {
                        PDBContext.Instance.AccountID = userHost.AssignedTo;
                        MainContext.Instance.Windows.Open(new MainWindow());
                    }
                    else
                    {
                        MainContext.Instance.Windows.Open(new LoginWindow());
                    }
                    InitProgressBar.Visibility = Visibility.Collapsed;
                    MainContext.Instance.Windows.Close(this);
                }
            });
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

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
