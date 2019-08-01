using System.Collections.Generic;
using System.Windows;

namespace WPF.Models.Extensions
{
    public class WindowsList : List<Window>
    {
        private readonly App app;

        public WindowsList(App app) : base()
        {
            this.app = app;
        }

        public new void Add(Window window)
        {
            app.MainWindow = window;
            window.Show();
            base.Add(window);
        }

        public new void Remove(Window window)
        {
            window.Close();
            base.Remove(window);
        }
    }
}
