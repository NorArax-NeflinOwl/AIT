using System;
using System.Windows.Controls;
using WPF.Models;
using WPF.Properties;
using WPF.GUI.Controls;

namespace WPF.GUI.Pages.Properties
{
    public class DashboardProperties : IPageModel
    {
        public TabItemHeaderControl Header { get; set; } = new TabItemHeaderControl(Resources.DASHBOARD_HEADER);
        public Page Content { get; set; } = new DashboardPage();
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }
    }
}
