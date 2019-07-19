using System;

namespace WPF.Managers
{
    public class FileManager
    {
        public static void LogExceptionToFile(Exception e)
        {
            // TODO implement file logger
#if DEBUG
            throw e;
#endif
        }
    }
}
