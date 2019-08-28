using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.GUI.Windows.Properties
{
    public class MainProperties : IWindowsProperties
    {
        public string Title { get; set; }
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.MAIN;
        public IPageModel PagePropertie { get; set; }
        public ResizeMode ResizeMode { get; set; } = ResizeMode.CanResizeWithGrip;
        public double Width { get; set; } = 1200;
        public double Heigth { get; set; } = 800;
        public bool Topmost { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
        public Window Window { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsDisposed { get; set; }

        public MainProperties()
        {
            Window = new MainWindow();
        }
        public MainProperties(Window window)
        {
            Window = window;
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        public void CopyProperties(IDictionary<string, object> properties)
        {
            Properties = properties;
        }
    }
}
