using System.IO;

namespace AppSearch.MVC.Helpers
{
    public class FileHelper
    {
        public static void RemoveFile(string? targetPath)
        {
            if (!string.IsNullOrEmpty(targetPath) && File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
        }
    }
}
