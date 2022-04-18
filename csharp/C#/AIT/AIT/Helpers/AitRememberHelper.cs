using AITLib.Constants;
using System.IO;
using AIT.DataBases.DBModel;
using AITLib.Helpers;

namespace AIT.Helpers
{
    public class AitRememberHelper
    {
        public static void CreateMemoryLogin(AitPerson person)
        {
            var path = Path.Combine(AitStrings.LOCALCACHE_SUBPATH, AitStrings.REMEMEBER_ME_FILE);
            var manager = new AitFileManager();
            var hash = AitCryptJsonManager.Instance.Serialize(person);
            manager.WriteToFile(path, hash);
        }
    }
}
