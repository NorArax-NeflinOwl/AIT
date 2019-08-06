using System.Collections.Generic;
using System.Linq;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.Models.Extensions
{
    public class WindowsDictionary : List<IWindowsProperties>
    {
        private readonly App app;

        public WindowsDictionary(App app) : base()
        {
            this.app = app;
        }

        public void Open(IWindowsProperties properties)
        {
            if(!this.Any(q => q.WindowName.Equals(properties.WindowName)))
            {
                Add(properties);
            }

            if(properties.WindowName.Equals(WindowsNameEnum.MAIN))
            {
                app.MainWindow = properties.Window;
            }

            properties.Window.Show();
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
            this.Where(q => q.WindowName.Equals(key)).FirstOrDefault()?.Window.Show();
        }

        public IWindowsProperties Window(WindowsNameEnum key)
        {
            return this.Where(q => q.WindowName.Equals(key)).FirstOrDefault();
        }

        public void Clear(IWindowsProperties properties)
        {
            foreach(var win in this)
                win.Window.Close();

            Clear();
            Open(properties);
        }
    }
}
