using System.Windows;
using WPF.Models.Enums;

namespace WPF.Windows.Properties
{
    public class MainProperties : IWindowsProperties
    {
        public string Title { get; set; }
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.MAIN;
        public PagesNameEnum PageName { get; set; } = PagesNameEnum.MAIN;
        public ResizeMode ResizeMode { get; set; } = ResizeMode.CanResizeWithGrip;
        public double Width { get; set; } = 1200;
        public double Heigth { get; set; } = 800;
        public bool Topmost { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
    }
}
