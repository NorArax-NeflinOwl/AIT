using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.GUI.Windows.Properties
{
    class DialogProperties : IWindowsProperties
    {
        public string Title { get; set; }
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.DIALOG;
        public IPageModel PagePropertie { get; set; }
        public ResizeMode ResizeMode { get; set; } = ResizeMode.CanResize;
        public double Width { get; set; } = 700;
        public double Heigth { get; set; } = 250;
        public bool Topmost { get; set; } = true;
        public WindowStyle WindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
        public Window Window { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsDisposed { get; set; }

        public DialogProperties()
        {
            Window = new DialogWindow(this);
        }

        public DialogProperties(Window window)
        {
            Window = window;
        }

        public DialogProperties(DialogTypeEnum type, string key, object obj)
        {
            Properties.Add(nameof(DialogTypeEnum), type);
            Properties.Add(key, obj);

            Window = new DialogWindow(this);
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
