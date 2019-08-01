using System;
using System.Windows.Controls;
using WPF.Models;
using WPF.Properties;
using WPF.UI.Controls;

namespace WPF.UI.Pages.Properties
{
    public class DashboardProperties : IPageModel
    {
        public TabItemHeaderControl Header { get; set; } = new TabItemHeaderControl(Resources.DASHBOARD_HEADER);
        public Page Content { get; set; } = new DashboardPage();

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
