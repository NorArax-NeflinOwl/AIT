using System;
using System.ComponentModel;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.UI.Windows.Properties;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        public App()
        {
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
