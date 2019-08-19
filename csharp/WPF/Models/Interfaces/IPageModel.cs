using System;
using System.Windows.Controls;
using WPF.Models.Interfaces;
using WPF.UI.Controls;

namespace WPF.Models
{
    public interface IPageModel : IDisposableExtended
    {
        TabItemHeaderControl Header { get; set; }
        Page Content { get; set; }
    }
}
