using System;
using System.Windows.Controls;
using WPF.Models.Interfaces;

namespace WPF.UI.Pages
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page, IDisposableExtended, IPropertizableControl
    {
        public SettingPage()
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
