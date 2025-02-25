using IWshRuntimeLibrary;
using System.IO;
using System.Xml;

namespace AppSearch.MVC.Helpers
{
    public class FileHelper
    {
        public static void RemoveFile(string? targetPath)
        {
            if (!string.IsNullOrEmpty(targetPath) && System.IO.File.Exists(targetPath))
            {
                System.IO.File.Delete(targetPath);
            }
        }

        public static string? GetExecutiveFileFromSCCFile(string? targetPath)
        {
            if(!string.IsNullOrEmpty(targetPath) && System.IO.File.Exists(targetPath) && IsSccFile(targetPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(targetPath);
                XmlNodeList? exefile = xmlDoc.SelectNodes("//environment/applications/exeFile");
                if (exefile != null && exefile.Count > 0)
                    return exefile[0]?.InnerText;
            }
            return null;
        }

        public static string? GetVersionAppFromSCCFile(string? targetPath)
        {
            if (!string.IsNullOrEmpty(targetPath) && System.IO.File.Exists(targetPath) && IsSccFile(targetPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(targetPath);
                XmlNodeList? exefile = xmlDoc.SelectNodes("//environment/applications/version");
                if (exefile != null && exefile.Count > 0)
                    return exefile[0]?.InnerText;
            }
            return null;
        }

        public static bool IsShortCut(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".lnk", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsExcecutive(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".exe", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsSccFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".scc", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetTargerPath(string filePath)
        {
            string targetPath = filePath;
            if (IsShortCut(filePath) && System.IO.File.Exists(filePath))
            {
                targetPath = GetShortcutTarget(filePath);
            }
            return System.IO.File.Exists(targetPath) 
                ? targetPath : string.Empty;
        }

        private static string GetShortcutTarget(string shortcutPath)
        {
            WshShell shell = new();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            return System.IO.File.Exists(shortcut.TargetPath) 
                ? shortcut.TargetPath : string.Empty;
        }
    }
}
