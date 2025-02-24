using System.Collections.Generic;

namespace AIT.Helpers
{
    public enum Feature
    {
        SERIALIZATION_SESSION
    }

    public class AitFeatureManager
    {
        private static readonly object m_Locker = new object();
        private static readonly AitFeatureManager m_Instatnce = new AitFeatureManager();
        private AitFeatureManager() { }

        public static AitFeatureManager Instance
        {
            get
            {
                lock (m_Locker)
                {
                    return m_Instatnce;
                }
            }
        }

        public readonly Dictionary<Feature, bool> Disable_Feature = new Dictionary<Feature, bool>
        {
            [Feature.SERIALIZATION_SESSION] = false
        };
    }
}
