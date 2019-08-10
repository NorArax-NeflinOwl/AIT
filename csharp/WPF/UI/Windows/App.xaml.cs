using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Managers;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        public App()
        {
            using (new DBContext())
            {
                Thread.Sleep(10);
            }
            MainContext.Instance = new MainContext(this);

            Subscribe();
            BackgroundTasksManager.Instance.Initialize();
        }

        public void Subscribe()
        {
            if(MainWindow != null)
            {
                MainWindow.Closing += MainWindow_Closing;
            }
        }

        public void Dispose()
        {
            if (MainWindow != null)
            {
                MainWindow.Closing -= MainWindow_Closing;
            }

            GC.Collect();
        }

        public static void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var msgBox = MessageBox.Show(WPF.Properties.Resources.WANT_EXIT, WPF.Properties.Resources.QUESTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgBox == MessageBoxResult.Yes)
            {
                var collect = BackgroundTasksManager.Instance.Collect();
                collect.Wait();

                MainContext.Instance.Windows.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
