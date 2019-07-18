using System.Collections.Generic;
using System.Windows;

namespace WPF.Contexts
{
    public class MainContext
    {
        private static object locker = new object();
        private static MainContext instance = new MainContext();

        public static MainContext Instance { get { lock (locker) return instance; } }

        public IList<Window> Windows { get; private set; }

        private MainContext()
        {
            Windows = new List<Window>();
        }
    }
}
