using System;
using System.Windows.Controls;
using WPF.Models;
using WPF.Properties;
using WPF.UI.Controls;

namespace WPF.UI.Pages.Properties
{
    public class SettingsProperties : IPageModel
    {
        public TabItemHeaderControl Header { get; set; } = new TabItemHeaderControl(Resources.SETTINGS_HEADER);
        public Page Content { get; set; } = new SettingPage();

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
