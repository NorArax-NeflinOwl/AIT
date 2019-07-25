using System;
using System.Diagnostics;
using System.Windows;
using WPF.Databases.Contexts;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        public MainWindow()
        {
            InitializeComponent();
            Subscribe();
        }

        public void Subscribe()
        {
            KeyUp += MainWindow_KeyUp;
        }

        public void Dispose()
        {
            KeyUp -= MainWindow_KeyUp;
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}]");
        }
    }
}
