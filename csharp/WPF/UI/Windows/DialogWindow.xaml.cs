using System;
using System.Windows;
using WPF.Models.Interfaces;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDisposableExtended, IPropertizableControl
    {
        public IProperties Properties { get; set; }
        public bool IsDisposed { get; set; }

        public DialogWindow(IWindowsProperties properties)
        {
            Properties = properties;
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            KeyUp -= App.MainWindow_KeyUp;
            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
        }

        public void Subscribe()
        {
            KeyUp += App.MainWindow_KeyUp;
        }
    }
}
