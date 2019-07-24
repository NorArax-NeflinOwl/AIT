using System;
using System.Diagnostics;
using System.IO;
using WPF.Properties;

namespace WPF.Managers
{
    public class FileManager : IDisposable
    {
        private static string m_AppDirectoryPath;

        public FileManager()
        {
            Init();
        }

        public static void Init()
        {
            m_AppDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            CreateDirectory(Resources.LOCALCACHE_SUBPATH);
        }

        public static string CombinePath(params string[] subPaths)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.LOCALCACHE_SUBPATH);
            foreach (string subPath in subPaths)
            {
                path = Path.Combine(path, subPath);
            }
            return path;
        }

        public static void CreateDirectory(string subPath)
        {
            try
            {
                var path = Path.Combine(m_AppDirectoryPath, subPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static string CreateFile(string subPath, string fileName)
        {
            return CreateFile(subPath, fileName, Resources.AIT_EXT);
        }

        public static string CreateFile(string subPath, string fileName, string extension)
        {
            try
            {
                fileName += extension;
                var path = Path.Combine(m_AppDirectoryPath, subPath, fileName);
                if (!File.Exists(path))
                    using (File.Create(path)) { }

                return path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return string.Empty;
        }

        public void WriteToFile(string path, string content)
        {
            var paths = path.Split('\\');
            var fileName = paths[paths.Length - 1];
            path = string.Empty;
            paths[0] += "\\";
            for (var i = 0; i < paths.Length - 1; i++)
            {
                path = Path.Combine(path, paths[i]);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            var subPath = CreateFile(path, fileName);

            using (var stream = File.AppendText(subPath))
            {
                stream.WriteLine(content);
            }
        }

        public string ReadFile(string path)
        {
            var fileContent = string.Empty;
            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader sr = File.OpenText(path))
                    {
                        while (sr.Peek() >= 0)
                        {
                            fileContent += sr.ReadLine();
                            Debug.WriteLine(fileContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return fileContent;
        }

        public static void AppendFile(string path, string newLine)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.AppendAllLines(path, new[] { newLine });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void DeleteFile(string subPath)
        {
            try
            {
                var path = Path.Combine(m_AppDirectoryPath, subPath);
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
