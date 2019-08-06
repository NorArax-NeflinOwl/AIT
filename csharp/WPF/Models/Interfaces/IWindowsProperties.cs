using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models.Enums;

namespace WPF.Models.Interfaces
{
    public interface IWindowsProperties : IDisposable
    {
        string Title { get; set; }
        WindowsNameEnum WindowName { get; set; }
        IPageModel PagePropertie { get; set; }
        ResizeMode ResizeMode { get; set; }
        double Width { get; set; }
        double Heigth { get; set; }
        bool Topmost { get; set; }
        WindowStyle WindowStyle { get; set; }
        WindowStartupLocation WindowStartupLocation { get; set; }
        Window Window { get; set; }
        IDictionary<string, object> Properties { get; set; }

        void CopyProperties(IDictionary<string, object> properties);
    }
}
