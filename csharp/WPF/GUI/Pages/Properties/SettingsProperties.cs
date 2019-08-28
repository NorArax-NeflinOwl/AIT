using System;
using System.Windows.Controls;
using WPF.Models;
using WPF.Properties;
using WPF.GUI.Controls;

namespace WPF.GUI.Pages.Properties
{
    public class SettingsProperties : IPageModel
    {
        public TabItemHeaderControl Header { get; set; } = new TabItemHeaderControl(Resources.SETTINGS_HEADER);
        public Page Content { get; set; } = new SettingPage();
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }
    }
}
