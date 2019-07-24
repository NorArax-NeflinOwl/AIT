using System.Collections.Generic;
using System.Windows;

namespace WPF.Databases.Contexts
{
    public class MainContext
    {
        private static object locker = new object();
        private static MainContext instance = new MainContext();

        public static bool IsInit = false;
        public static MainContext Instance { get { lock (locker) return instance; } }

        public Window MainWindow { get; set; }
        public IList<Window> Windows { get; private set; }

        private MainContext()
        {
            Windows = new List<Window>();
        }

        public void Init(Window mainWindow)
        {
            if (!IsInit && mainWindow != null)
            {
                MainWindow = mainWindow;
                IsInit = true;
            }
        }
    }
}
