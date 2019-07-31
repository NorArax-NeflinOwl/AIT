using System;
using System.IO;
using System.Reflection;
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
    public partial class InitWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; set; }

        public InitWindow()
        {
            MainContext.Instance.Windows.Add(this);

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
                await Task.Delay(300);
                while (BackgroundTasksManager.Instance.Completed != BackgroundTasksManager.Instance.Count)
                {
                    var multiple = 100 / BackgroundTasksManager.Instance.Count;
                    var count = BackgroundTasksManager.Instance.Count;
                    var completed = BackgroundTasksManager.Instance.Completed;

                    if (completed != 0)
                    {
                        InitMessage.Text = WPF.Properties.Resources.APP_INIT;
                        InitProgressBar.Value = (count / completed) * multiple;
                    }
                }
                InitProgressBar.Value = 100;
                InitMessage.Text = WPF.Properties.Resources.APP_STARTCOMPLETED;
                await Task.Delay(500);

                MainContext.Instance.Windows.Add(new MainWindow());
                MainContext.Instance.Windows.Remove(this);
            });
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
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
