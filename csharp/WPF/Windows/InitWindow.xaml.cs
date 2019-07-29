using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.Models.Interfaces;
using WPF.Windows.Properties;

namespace WPF.Windows
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
            Properties = new InitProperties();

            InitializeComponent();
            Init();
        }

        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            InitMessage.Text = WPF.Properties.Resources.APP_START;
            InitImage.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\Icons\\logo4x3.png"));

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

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
