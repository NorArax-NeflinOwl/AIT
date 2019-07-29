using System.Collections.Generic;
using WPF.Models.Extensions;

namespace WPF.Databases.Contexts
{
    public class MainContext
    {
        private readonly static object locker = new object();
        private static MainContext instance;

        private static bool init;
        public static MainContext Instance
        {
            get { lock (locker) return instance; }
            set
            {
                if(!init)
                {
                    lock (locker)
                    {
                        instance = value;
                        init = true;
                    }
                }
            }
        }

        public WindowsList Windows { get; private set; }
        public IList<string> KeyLogger { get; private set; }

        private MainContext() { }

        public MainContext(App app)
        {
            Windows = new WindowsList(app);
            KeyLogger = new List<string>();
        }
    }
}
