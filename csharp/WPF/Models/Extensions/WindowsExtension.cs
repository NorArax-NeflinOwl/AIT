using System;
using System.Collections.Generic;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Managers;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.GUI.Windows;
using WPF.GUI.Windows.Properties;

namespace WPF.Models.Extensions
{
    public class WindowsExtension : List<IWindowsProperties>
    {
        private readonly App app;

        public WindowsExtension(App app) : base()
        {
            this.app = app;
        }

        public App App { get => app; }

        public void DeserializeSession()
        {
            // TODO open windows from PDBContext.Instace.SessionDictionary;
        }

        public IWindowsProperties Open(IWindowsProperties properties)
        {
            Open(properties, true);
            return properties;
        }

        public void Open(IWindowsProperties properties, bool save = true)
        {
            if(save)
            {
                if (!this.Any(q => q.WindowName.Equals(properties?.WindowName)))
                {
                    Add(properties);
                    if (properties?.WindowName.Equals(WindowsNameEnum.MAIN) == true)
                    {
                        LogManager.Instance.LogToFile(new LogInfoModel
                        {
                            Type = FileTypesEnum.TRACE,
                            MessageInfo = new MessageInfoModel($"Set main window = " + properties?.WindowName.ToString())
                        });

                        app.MainWindow = properties?.Window;
                    }
                }
            }

            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                MessageInfo = new MessageInfoModel($"Open window " + properties?.WindowName)
            });

            properties?.Window.Show();
        }

        public void Close(WindowsNameEnum key)
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                MessageInfo = new MessageInfoModel($"Close window " + key.ToString())
            });

            var prop = this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
            if(prop != null)
            {
                prop.Window.Close();
                Remove(prop);
            }

            if(prop != null)
            {
                prop.Dispose();
            }
        }

        public void Hide(WindowsNameEnum key)
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                MessageInfo = new MessageInfoModel($"Hide window " + key.ToString())
            });

            this.Where(q => q.WindowName.Equals(key)).FirstOrDefault()?.Window.Hide();
        }

        public void Hide(string additionalMsg = "")
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                MessageInfo = new MessageInfoModel($"Hide all windows {additionalMsg}")
            }); 

            ForEach(q => q.Window.Hide());
        }

        public void HideAndDispose(WindowsNameEnum key)
        {
            var prop = this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();

            if(prop != null)
            {
                LogManager.Instance.LogToFile(new LogInfoModel
                {
                    Type = FileTypesEnum.TRACE,
                    MessageInfo = new MessageInfoModel($"Hide and dispose window " + key.ToString())
                });

                prop.Window.Hide();
                prop.Dispose();
            }
        }

        public void Show(WindowsNameEnum key)
        {
            var window = this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
            if (window != null)
            {
                LogManager.Instance.LogToFile(new LogInfoModel
                {
                    Type = FileTypesEnum.TRACE,
                    MessageInfo = new MessageInfoModel($"Show window " + key.ToString())
                });

                window.Window.Show();
            }
            else
                Open(CreatePropertiesFromEnum(key));
        }

        public bool Show()
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                MessageInfo = new MessageInfoModel($"Show all windows in application")
            });

            var anyNewOpen = false;
            foreach(var prop in this)
            {
                if(!prop.Window.IsActive)
                {
                    prop.Window.Show();
                    anyNewOpen = true;
                }
            }
            return anyNewOpen;
        }

        public static MainWindow GetMainWindow()
        {
            return MainContext.Instance.Windows.Where(q => q.WindowName.Equals(WindowsNameEnum.MAIN)).FirstOrDefault()?.Window as MainWindow;
        }

        public IWindowsProperties GetWindow(WindowsNameEnum key)
        {
            return this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
        }

        public void Clear(IWindowsProperties properties)
        {
            Exit(properties);
            Open(properties);
        }

        public void Exit(IWindowsProperties properties = null)
        {
            if(Count > 0)
            {
                try
                {
                    foreach (var win in this)
                    {
                        if(!win.IsDisposed)
                        {
                            if (properties == null || properties != null && !properties.WindowName.Equals(win.WindowName))
                            {
                                if (IsClosableWindow(win.WindowName))
                                    win.Window?.Hide();
                                else
                                    win.Window?.Close();
                            }
                        }
                    }
                }
                catch (Exception) { }
            }

            var msg = $"Close all windows";
            if(properties != null)
            {
                msg += $" without {properties.WindowName} window and hide MainWindow";
            }

            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                MessageInfo = new MessageInfoModel(msg)
            });

            Clear();
        }

        private bool IsClosableWindow(WindowsNameEnum key)
        {
            switch(key)
            {
                case WindowsNameEnum.LOGIN:
                case WindowsNameEnum.REGISTRATION:
                case WindowsNameEnum.MAIN:
                    return true;
            }
            return false;
        }

        private IWindowsProperties CreatePropertiesFromEnum(WindowsNameEnum key)
        {
            switch(key)
            {
                case WindowsNameEnum.INIT:
                    return new InitProperties();
                case WindowsNameEnum.LOGIN:
                    return new LoginProperties();
                case WindowsNameEnum.REGISTRATION:
                    return new RegistrationProperties();
                case WindowsNameEnum.MAIN:
                    return new MainProperties();
            }

            return null;
        }
    }
}
