using AppSearch.MVC.Models;
using IWshRuntimeLibrary;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Xml;

namespace AppSearch.MVC.Helpers
{
    public class FileHelper
    {
        public static bool Exists(string? targetPath)
        {
            return !string.IsNullOrEmpty(targetPath) && System.IO.File.Exists(targetPath);
        }

        public static bool DirExists(string? dirPath)
        {
            return !string.IsNullOrEmpty(dirPath) && System.IO.Directory.Exists(dirPath);
        }

        public static void RemoveFile(string? targetPath)
        {
            if (Exists(targetPath))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                System.IO.File.Delete(targetPath);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        public static string? GetExecutiveFileFromSCCFile(string? targetPath)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            if (Exists(targetPath) && IsSCCFile(targetPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(targetPath);
                XmlNodeList? exefile = xmlDoc.SelectNodes("//environment/applications/exeFile");
                if (exefile != null && exefile.Count > 0)
                    return exefile[0]?.InnerText;
            }
#pragma warning restore CS8604 // Possible null reference argument.
            return null;
        }

        public static string? GetVersionAppFromSCCFile(string? targetPath)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            if (Exists(targetPath) && IsSCCFile(targetPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(targetPath);
                XmlNodeList? exefile = xmlDoc.SelectNodes("//environment/applications/version");
                if (exefile != null && exefile.Count > 0)
                    return exefile[0]?.InnerText;
            }
#pragma warning restore CS8604 // Possible null reference argument.
            return null;
        }

        public static void CreateShortcut(ConfigurationModel config, string targetFilePath)
        {
            string shortcutPath = System.IO.Path.Combine(config.GetAppDir(), System.IO.Path.GetFileNameWithoutExtension(targetFilePath) + ".lnk");

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetFilePath; 
            shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(targetFilePath); 
            shortcut.Save();
        }

        public static string GetShortcutTarget(ConfigurationModel config, string shortcutPath)
        {
            try
            {
                WshShell shell = new();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                return shortcut.TargetPath;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, config, LogginLevel.ERROR);
            }
            return string.Empty;
        }

        public static string[]? GetFiles(ConfigurationModel config)
        {
            return System.IO.Directory.GetFiles(config.GetAppDir());
        }

        public static ImageSource? ByteArrayToImageSource(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); // Optymalizacja dla UI
            }

            return bitmap;
        }

        public static IEnumerable<string>? GetAppListRecursive(ConfigurationModel config)
        {
            try
            {
                return GetAppListRecursive(config, new List<string>(), config.GetAppDir());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, config, LogginLevel.ERROR);
            }
            return null;
        }

        private static IEnumerable<string> GetAppListRecursive(ConfigurationModel config, List<string> list, string dirPath)
        {
            try
            {
                foreach (var subDir in System.IO.Directory.EnumerateDirectories(dirPath))
                {
                    list.Add(subDir);
                    GetAppListRecursive(config, list, subDir);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                LogHelper.WriteLine(ex, config, LogginLevel.ERROR);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, config, LogginLevel.ERROR);
            }

            return list;
        }

        private static bool IsSCCFile(string filePath)
        {
            return Exists(filePath) && System.IO.Path.GetExtension(filePath).Equals(".scc", StringComparison.OrdinalIgnoreCase);
        }
    }
}
