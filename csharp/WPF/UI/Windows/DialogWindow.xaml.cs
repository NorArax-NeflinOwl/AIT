using System;
using System.Windows;
using WPF.Models.Interfaces;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDisposable, IPropertizableWindow
    {
        public IWindowsProperties Properties { get; set; }

        public DialogWindow(IWindowsProperties properties)
        {
            Properties = properties;
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {

            GC.Collect();
        }

        public void Init()
        {
        }

        public void Subscribe()
        {
        }
    }
}
