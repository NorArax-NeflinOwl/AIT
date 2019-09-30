using System.Collections.Generic;
using System.IO;
using System.Linq;
using WPF.Models;

namespace WPF.Managers.Helpers
{

    public class FileFinder
    {
        public static List<FileResultModel> GetPaths(string dirPath, string searchString, string extension)
        {
            return GetPath(dirPath, searchString, extension);
        }

        public static List<FileResultModel> GetPaths(string dirPath, string searchString)
        {
            return GetPath(dirPath, searchString, "*");
        }

        private static List<FileResultModel> GetPath(string dirPath, string searchingString, string extension)
        {
            if (Directory.Exists(dirPath))
            {
                var result = new List<FileResultModel>();
                var filesPaths = Directory.GetFiles(dirPath, extension, SearchOption.AllDirectories).ToList();
                if (filesPaths?.Any() == true)
                {
                    foreach (var filePath in filesPaths)
                    {
                        if(!IsFileLocked(filePath))
                        {
                            using (StreamReader sr = File.OpenText(filePath))
                            {
                                var lineNumber = 0;
                                while (sr.Peek() >= 0)
                                {
                                    var lineContent = sr.ReadLine();
                                    if (lineContent.ToLower().Contains(searchingString.ToLower()))
                                        result.Add(new FileResultModel(filePath, lineNumber, lineContent));

                                    lineNumber++;
                                }
                            }
                        }

                        if (filePath.ToLower().Contains(searchingString.ToLower()))
                            result.Add(new FileResultModel(filePath));
                    }
                }

                return result.Distinct().ToList();
            }
            return null;
        }

        private static bool IsFileLocked(string path)
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }
}
