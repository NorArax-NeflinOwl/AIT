using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.GUI.Windows.Properties
{
    public class NoteProperties : IWindowsProperties
    {
        public string Title { get; set; }
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.NOTE;
        public IPageModel PagePropertie { get; set; }
        public ResizeMode ResizeMode { get; set; } = ResizeMode.CanResizeWithGrip;
        public double Width { get; set; } = 600;
        public double Heigth { get; set; } = 400;
        public bool Topmost { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
        public Window Window { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsDisposed { get; set; }

        public NoteProperties()
        {
            Window = new NoteWindow();
        }

        public NoteProperties(Window window, string path)
        {
            Width = 400;
            Heigth = 300;
            Title = WPF.Properties.Resources.RESULT;
            Window = window ?? new NoteWindow(path);
        }

        public NoteProperties(Window window)
        {
            Title = WPF.Properties.Resources.NOTE;
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
