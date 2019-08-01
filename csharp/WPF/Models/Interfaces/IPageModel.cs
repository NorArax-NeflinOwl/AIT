using System;
using System.Windows.Controls;
using WPF.UI.Controls;

namespace WPF.Models
{
    public interface IPageModel : IDisposable
    {
        TabItemHeaderControl Header { get; set; }
        Page Content { get; set; }
    }
}
