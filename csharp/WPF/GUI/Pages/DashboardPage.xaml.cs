using System;
using System.Windows.Controls;
using WPF.Models.Interfaces;

namespace WPF.GUI.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page, IDisposableExtended, IPropertizableControl
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; set; }

        public void Dispose()
        {

            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe()
        {
            throw new System.NotImplementedException();
        }
    }
}
