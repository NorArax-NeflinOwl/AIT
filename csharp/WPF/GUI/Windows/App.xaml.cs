using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.Models.Interfaces;
using WPF.GUI.Windows.Properties;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposableExtended
    {
        private static bool IsClosed;

        public bool IsDisposed { get; set; }

        public App()
        {
            string path = string.Empty;
#if DEBUG
            var dirPath = @"D:\AIT\csharp\WPF\Databases";
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() { DefaultExt = ".db" };
            if (!Directory.Exists(dirPath))
            {
                openFileDialog.InitialDirectory = dirPath;
            }
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                path = openFileDialog.FileName;
            }
#endif

            using (new DBContext(path))
            {
                Thread.Sleep(10);
            }
            MainContext.Instance = new MainContext(this);

            Subscribe();
            BackgroundTasksManager.Instance.Initialize();

            MainContext.Instance.Windows.Open(new InitProperties());
        }

        public void Subscribe()
        {
            if(MainWindow != null)
            {
                MainWindow.Closing += MainWindow_Closing;
                MainWindow.KeyUp += MainWindow_KeyUp;
            }
        }

        public static void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}] release on [{MainContext.Instance.Windows?.App?.MainWindow?.Name}] page");
        }

        public void Dispose()
        {
            if (MainWindow != null)
            {
                MainWindow.Closing -= MainWindow_Closing;
                MainWindow.KeyUp -= MainWindow_KeyUp;
            }

            IsDisposed = true;
            GC.Collect();
        }

        public static void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if(!IsClosed)
            {
                var msgBox = MessageBox.Show(WPF.Properties.Resources.WANT_EXIT, WPF.Properties.Resources.QUESTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (msgBox == MessageBoxResult.Yes)
                {
                    IsClosed = true;
                    MainContext.Instance.Windows.Hide("for close app");
                    BackgroundTasksManager.Instance.Collect().Wait();

                    MainContext.Instance.Windows.Exit();
                    Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
