using AIT.Helpers;
using System.Threading.Tasks;

namespace AIT.DataBases
{
    public class AitDBContextInstance : AitDBContext
    {
        private static readonly object m_Locker = new object();
        private static readonly AitDBContextInstance m_Instatnce = new AitDBContextInstance();
        private AitDBContextInstance() { }

        public static AitDBContextInstance Instance
        {
            get
            {
                lock(m_Locker)
                {
                    return m_Instatnce;
                }
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            AitSessionManager.GetSession.HasChanges = true;
            return base.SaveChangesAsync();
        }
    }
}
