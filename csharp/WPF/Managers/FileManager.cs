using System;
using System.Diagnostics;
using System.IO;
using WPF.Models.Interfaces;
using WPF.Properties;

namespace WPF.Managers
{
    public class FileManager : IDisposableExtended
    {
        private static string m_AppDirectoryPath;

        public bool IsDisposed { get; set; }

        public FileManager()
        {
            Initialize();
        }

        public static void Initialize()
        {
            m_AppDirectoryPath = Environment.CurrentDirectory;
            CreateDirectory(Resources.LOCALCACHE_SUBPATH);
            CreateDirectory(Resources.LOGDIR_SUBPATH);
        }

        public static void CreateDBFile(string path, string dbName)
        {
            try
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                DirectoryInfo info = new DirectoryInfo(path);
                info.Attributes = FileAttributes.Hidden;

                if (!File.Exists(Path.Combine(path, dbName)))
                {
                    using (File.Create(Path.Combine(path, dbName))) { }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
#if DEBUG
                throw e;
#endif
            }
        }

        public static string CombinePath(params string[] subPaths)
        {
            var path = string.Empty;
            if(subPaths == null)
                path = Path.Combine(Environment.CurrentDirectory, Resources.LOCALCACHE_SUBPATH);

            foreach (string subPath in subPaths)
            {
                path = Path.Combine(path, subPath);
            }
            return path;
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

#if DEBUG
                throw ex;
#endif
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
#if DEBUG
                throw ex;
#endif
            }
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
#if DEBUG
                throw ex;
#endif
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
#if DEBUG
                throw ex;
#endif
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
#if DEBUG
                throw ex;
#endif
            }
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }
    }
}
