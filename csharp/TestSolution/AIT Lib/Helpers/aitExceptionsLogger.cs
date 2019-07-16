using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using AIT_Lib.FilesManagers;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using AIT_Lib.Constant;

namespace AIT_Lib.Helpers
{
    public enum aitLogTitle
    {
        EXCEPTION,
        INFORMATION,
    }

    public class aitLog
    {
        public aitLogTitle Title { get; set; }
        public string Message { get; set; }
    }

    public class aitExceptionsLogger
    {
        private static string m_LoggerDir = aitFileManager.CombinePath(new string[] { "Log" });
        private static BlockingCollection<aitLog> m_Logger;

        private aitExceptionsLogger() 
        {
            m_Logger = new BlockingCollection<aitLog>(100);
            aitFileManager.CreateDirectory(aitFileManager.CombinePath(aitStrings.LOGDIR_SUBPATH));
        }

        private static object m_Locker = new object();
        private static aitExceptionsLogger m_Instance;
        public static aitExceptionsLogger Instance
        {
            get
            {
                lock (m_Locker)
                {
                    if (m_Instance == null)
                        m_Instance = new aitExceptionsLogger();
                    return m_Instance;
                }
            }
        }

        public void LogToFile(aitLog newLog)
        {
            m_Logger.Add(newLog);
            Task.Factory.StartNew( () => {
                while (!m_Logger.IsCompleted)
                {
                    aitLog log = null;
                    try
                    {
                        log = m_Logger.Take();

                        if (log != null)
                        {
                            var fileName = log.Title.ToString() + "-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + aitStrings.LOG_EXT;
                            var filePath = Path.Combine(m_LoggerDir, fileName);
                            if (!File.Exists(filePath))
                                using (File.Create(filePath)) ;
                            using (var stream = File.AppendText(filePath))
                            {
                                stream.WriteLine(log.Message);
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            });
        }
    }
}
