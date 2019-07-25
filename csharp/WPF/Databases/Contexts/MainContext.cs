using System.Collections.Generic;
using System.Windows;

namespace WPF.Databases.Contexts
{
    public class MainContext
    {
        private readonly static object locker = new object();
        private readonly static MainContext instance = new MainContext();

        public static bool IsInit = false;
        public static MainContext Instance { get { lock (locker) return instance; } }

        public Window MainWindow { get; set; }
        public IList<Window> Windows { get; private set; }
        public IList<string> KeyLogger { get; private set; }

        private MainContext()
        {
            Windows = new List<Window>();
            KeyLogger = new List<string>();
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
