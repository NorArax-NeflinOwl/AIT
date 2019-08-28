using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.Properties;

namespace WPF.GUI.Windows.Properties
{
    public class RegistrationProperties : IWindowsProperties
    {
        public string Title { get; set; } = Resources.REGISTER_TITLE;
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.REGISTRATION;
        public IPageModel PagePropertie { get; set; }
        public ResizeMode ResizeMode { get; set; }
        public double Width { get; set; }
        public double Heigth { get; set; }
        public bool Topmost { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; }
        public Window Window { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsDisposed { get; set; }

        public RegistrationProperties()
        {
            Window = new RegistrationWindow();
        }
        public RegistrationProperties(Window window)
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
