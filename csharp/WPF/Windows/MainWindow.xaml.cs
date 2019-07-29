using System;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;
using WPF.Windows.Properties;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; set; }

        public MainWindow()
        {
            Properties = new MainProperties();
            InitializeComponent();
            Subscribe();
        }

        public void Subscribe()
        {
            KeyUp += MainWindow_KeyUp;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            KeyUp -= MainWindow_KeyUp;
            GC.Collect();
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key.ToString();
            MainContext.Instance.KeyLogger.Add($"{DateTime.Now} Key[{key}]");
        }
    }
}
