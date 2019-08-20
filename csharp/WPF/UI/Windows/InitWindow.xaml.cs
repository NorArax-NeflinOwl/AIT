using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.UI.Windows
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
            CenterWindowOnScreen();
            InitMessage.Text = WPF.Properties.Resources.APP_START;
            InitImage.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\UI\\Icons\\logo4x3.png"));

            Dispatcher.Invoke(async () =>
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

                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds < 2000)
                    {
                        await Task.Delay(3000);
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
            IsDisposed = true;
            GC.Collect();
        }
    }
}
