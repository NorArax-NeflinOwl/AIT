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
            if (MessageBox.Show(WPF.Properties.Resources.WANT_EXIT, WPF.Properties.Resources.QUESTION, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BackgroundTasksManager.Instance.Collect();
                MainContext.Instance.Windows.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
