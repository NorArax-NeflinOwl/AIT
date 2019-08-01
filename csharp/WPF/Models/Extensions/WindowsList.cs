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

        public void Open(Window window)
        {
            app.MainWindow = window;
            window.Show();
            Add(window);
        }

        public void Close(Window window)
        {
            window.Close();
            Remove(window);
        }

        public void Clear(Window window)
        {
            foreach(var win in this)
                win.Close();

            Clear();

            app.MainWindow = window;
        }
    }
}
