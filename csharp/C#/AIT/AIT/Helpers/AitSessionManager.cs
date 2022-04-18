using AIT.DataBases;
using AIT.DataBases.DBModel;
using System;
using System.Linq;

namespace AIT.Helpers
{
    public class AitSessionManager
    {
        private static readonly object m_locker = new object();
        private static readonly AitSessionManager m_Instance = new AitSessionManager();

        private AitSessionManager() { }

        private AitPerson person = null;
        public AitPerson Person { get => person; }
        
        private string m_SessionID = string.Empty;
        public string SessionID { get => m_SessionID; }

        private bool hasChanges = false;
        public bool HasChanges 
        { 
            get => hasChanges; 
            set => hasChanges = value;
        }

        public static AitSessionManager GetSession
        {
            get
            {
                lock(m_locker)
                {
                    return m_Instance;
                }
            }
        }

        public void CreateSession(int id)
        {
            if (string.IsNullOrWhiteSpace(m_SessionID))
            {
                person = AitDBContextInstance.Instance.AitPersons.Single(p => p.ID.Equals(id));
                m_SessionID = person.GetHashCode().ToString();
            }
            else
                throw new Exception();
        }
        
        public void Clear()
        {
            person = null;
            m_SessionID = null;
            hasChanges = false;
        }
    }
}
