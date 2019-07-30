using WPF.Models;
using WPF.Properties;

namespace WPF.Pages.Properties
{
    public class DashboardProperties : IPageModel
    {
        public string Header { get; set; } = Resources.DASHBOARD_HEADER;
        public object Content { get; set; } = new DashboardPage();
    }
}
