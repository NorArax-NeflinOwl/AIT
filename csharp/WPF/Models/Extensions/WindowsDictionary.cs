using System.Collections.Generic;
using System.Linq;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.Models.Extensions
{
    public class WindowsDictionary : List<IWindowsProperties>
    {
        private readonly App app;

        public WindowsDictionary(App app) : base()
        {
            this.app = app;
        }

        public void Open(IWindowsProperties properties, bool save = true)
        {
            if(save)
            {
                if (!this.Any(q => q.WindowName.Equals(properties?.WindowName)))
                {
                    Add(properties);
                }

                if (properties?.WindowName.Equals(WindowsNameEnum.MAIN) == true)
                {
                    app.MainWindow = properties?.Window;
                }
            }

            properties?.Window.Show();
        }

        public void Close(WindowsNameEnum key)
        {
            var prop = this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
            prop.Window.Close();
            Remove(prop);

            if(prop != null)
            {
                prop.Dispose();
            }
        }

        public void Hide(WindowsNameEnum key)
        {
            this.Where(q => q.WindowName.Equals(key)).FirstOrDefault()?.Window.Hide();
        }

        public void Show(WindowsNameEnum key)
        {
            var window = this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
            if (window != null)
                window.Window.Show();
            else
                Open(CreatePropertiesFromEnum(key));
        }

        public IWindowsProperties Window(WindowsNameEnum key)
        {
            return this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
        }

        public void Clear(IWindowsProperties properties)
        {
            Exit();
            Open(properties);
        }

        public void Exit()
        {
            foreach (var win in this)
                win.Window.Close();

            Clear();
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
