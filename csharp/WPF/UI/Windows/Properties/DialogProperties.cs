using System;
using System.Collections.Generic;
using System.Windows;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.UI.Windows.Properties
{
    class DialogProperties : IWindowsProperties
    {
        public string Title { get; set; }
        public WindowsNameEnum WindowName { get; set; } = WindowsNameEnum.DIALOG;
        public IPageModel PagePropertie { get; set; }
        public ResizeMode ResizeMode { get; set; } = ResizeMode.CanResize;
        public double Width { get; set; } = 300;
        public double Heigth { get; set; } = 200;
        public bool Topmost { get; set; } = true;
        public WindowStyle WindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
        public Window Window { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public DialogProperties()
        {
            Window = new DialogWindow(this);
        }

        public DialogProperties(Exception exception)
        {
            Window = new DialogWindow(this);
            Properties.Add(FileTypesEnum.EXCEPTION.ToString(), exception);
        }

        public DialogProperties(LogInfoModel logInfo, DialogTypeEnum type)
        {
            Window = new DialogWindow(this);

            Properties.Add("LOG_INFO", logInfo);
            Properties.Add("DIALOG_TYPE", type);
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
