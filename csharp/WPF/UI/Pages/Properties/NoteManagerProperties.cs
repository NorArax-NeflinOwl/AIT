using System;
using System.Windows.Controls;
using WPF.Models;
using WPF.Properties;
using WPF.UI.Controls;

namespace WPF.UI.Pages.Properties
{
    public class NoteManagerProperties : IPageModel
    {
        public TabItemHeaderControl Header { get; set; } = new TabItemHeaderControl(Resources.NOTEMANAGER_HEADER);
        public Page Content { get; set; } = new NoteManagerPage();
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }
    }
}
