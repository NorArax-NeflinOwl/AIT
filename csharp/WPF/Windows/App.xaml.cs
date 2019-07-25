using System;
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

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach(Window window in MainContext.Instance.Windows)
            {
                if(window.IsActive)
                    window.Close();
            }
            MainContext.Instance.Windows.Clear();
        }
    }
}
