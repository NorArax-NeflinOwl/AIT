using System.Windows.Controls;
using WPF.UI.Controls;

namespace WPF.Models
{
    public interface IPageModel
    {
        TabItemHeaderControl Header { get; set; }
        Page Content { get; set; }
    }
}
