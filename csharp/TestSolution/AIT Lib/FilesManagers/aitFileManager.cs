using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using AIT_Lib.Constant;

namespace AIT_Lib.FilesManagers
{
    public class aitFileManager : IDisposable
    {
        private static string m_AppDirectoryPath;

        public aitFileManager() 
        {
            Init();
        }

        public static void Init()
        {
            m_AppDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            CreateDirectory(aitStrings.LOCALCACHE_SUBPATH);
        }

        public static string CombinePath(params string[] subPaths)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, aitStrings.LOCALCACHE_SUBPATH);
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
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static string CreateFile(string subPath, string fileName, string extension = aitStrings.AIT_EXT)
        {
            try
            {
                fileName = fileName + extension;
                var path = Path.Combine(m_AppDirectoryPath, subPath, fileName);
                if (!File.Exists(path))
                {
                    using(File.Create(path));
                }
                return path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return string.Empty;
        }

        public void WriteToFile(string subPath, string content)
        {
            var paths = subPath.Split('\\');
            var fileName = paths[paths.Length - 1];
            subPath = string.Empty;
            paths[0] += "\\";
            for(var i = 0; i < paths.Length - 1; i++)
            {
                subPath = Path.Combine(subPath, paths[i]);
                if (!Directory.Exists(subPath))
                    Directory.CreateDirectory(subPath);
            }
            var path = CreateFile(subPath, fileName);

            using (var stream = File.AppendText(path))
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
                        while ((fileContent += sr.ReadLine()) != null)
                        {
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

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
