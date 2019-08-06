using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.UI.Windows.Properties
{
    public class InitProperties : IWindowsProperties
    {
        public string Title { get; set; }
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.INIT;
        public IPageModel PagePropertie { get; set; }
        public ResizeMode ResizeMode { get; set; }
        public double Width { get; set; }
        public double Heigth { get; set; }
        public bool Topmost { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; }
        public Window Window { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public InitProperties()
        {
            Window = new InitWindow();
        }

        public InitProperties(Window window)
        {
            Window = window;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public void CopyProperties(IDictionary<string, object> properties)
        {
            Properties = properties;
        }
    }
}
