using System;
using System.IO;
using System.Text;

namespace WPF.Managers.Helpers
{
    public class DirectoryInfoProvider
    {
        public static long GetDirectorySize(string dirPath)
        {
            return GetDirectorySize(new DirectoryInfo(dirPath));
        }

        public static long GetDirectorySize(DirectoryInfo dirInfo)
        {
            long size = 0;
            try
            {
                FileInfo[] files = dirInfo.GetFiles();
                foreach (FileInfo file in files)
                    size += file.Length;

                DirectoryInfo[] dirsInfo = dirInfo.GetDirectories();
                foreach (DirectoryInfo dir in dirsInfo)
                    size += GetDirectorySize(dir);
            }
            catch (Exception)
            { }

            return size;
        }

        public static string GetString2PrintDirectorySize(string dirPath)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(dirPath).AppendLine();
            builder.Append(GetString2PrintDirectorySize(new DirectoryInfo(dirPath)));
            return builder.ToString();
        }

        public static string GetString2PrintDirectorySize(DirectoryInfo dirInfo)
        {
            long totalSize = 0;
            StringBuilder builder = new StringBuilder();
            try
            {
                FileInfo[] files = dirInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    totalSize += file.Length;
                    builder.Append("File: ").Append(file.Name).Append(" - ").Append(GetSize(file.Length)).AppendLine();
                }

                DirectoryInfo[] dirsInfo = dirInfo.GetDirectories();
                foreach (DirectoryInfo dir in dirsInfo)
                {
                    var direcotrySize = GetDirectorySize(dir);
                    totalSize += direcotrySize;
                    builder.Append("Dir: ").Append(dir.Name).Append(" - ").Append(GetSize(direcotrySize)).AppendLine();
                }
            }
            catch (Exception)
            { }
            builder.Append("Total: ").Append(GetSize(totalSize)).AppendLine();

            return builder.ToString();
        }

        public static string GetSize(long size)
        {
            int i = 0;
            int pow = 1024;
            while(size > pow)
            {
                size /= pow;
                i++;
            }
            return size + " " + GetStringSize(i);
        }

        public static string GetStringSize(int i)
        {
            switch(i)
            {
                case 0:
                    return "Byte";
                case 1:
                    return "KB";
                case 2:
                    return "MB";
                case 3:
                    return "GB";
                case 4:
                    return "TB";
                default:
                    return "unknown";
            }
        }
    }
}
