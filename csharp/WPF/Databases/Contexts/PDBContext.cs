using System.Collections.Generic;

namespace WPF.Databases.Contexts
{
    public class PDBContext
    {

        private static readonly object locker = new object();
        private static readonly PDBContext instance = new PDBContext();

        private PDBContext()
        {
        }

        public static PDBContext Instance
        {
            get
            {
                lock(locker)
                {
                    return instance;
                }
            }
        }

        public DBContext Context
        {
            get
            {
                return new DBContext();
            }
        }
    }
}
