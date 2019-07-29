using System.Windows;
using WPF.Models.Enums;
using WPF.Properties;

namespace WPF.Windows.Properties
{
    public class InitProperties : IWindowsProperties
    {
        public string Title { get; set; } = Resources.INIT_WINDOW;
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.INIT;
        public PagesNameEnum PageName { get; set; } = PagesNameEnum.MAIN;
        public ResizeMode ResizeMode { get; set; } = ResizeMode.NoResize;
        public double Width { get; set; } = 500;
        public double Heigth { get; set; } = 300;
        public bool Topmost { get; set; } = true;
        public WindowStyle WindowStyle { get; set; } = WindowStyle.None;
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
    }
}
