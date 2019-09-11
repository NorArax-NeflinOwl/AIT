using System;
using System.Windows.Controls;
using WPF.Models.Interfaces;

namespace WPF.GUI.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page, IDisposableExtended, IPropertizableControl, ISerializableForSession
    {
        public bool IsDisposed { get; set; }
        public IProperties Properties { get; }

        public DashboardPage()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
        }

        public void Subscribe()
        {
        }

        public void SerializeSession()
        {
        }

        public void DeserializaSession()
        {
        }
    }
}
