using System.Windows;
using WPF.Models.Enums;

namespace WPF.Windows.Properties
{
    public interface IWindowsProperties
    {
        string Title { get; set; }
        WindowsNameEnum WindowName { get; set; }
        PagesNameEnum PageName { get; set; }
        ResizeMode ResizeMode { get; set; }
        double Width { get; set; }
        double Heigth { get; set; }
        bool Topmost { get; set; }
        WindowStyle WindowStyle { get; set; }
        WindowStartupLocation WindowStartupLocation { get; set; }
    }
}
