using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using AITLib.Constants;

namespace AITLib.Helpers
{
    public enum AitLogTitle
    {
        EXCEPTION,
        INFORMATION,
    }

    public class AitLog
    {
        public AitLogTitle Title { get; set; }
        public string Message { get; set; }
    }

    public class AitExceptionsLogger
    {
        private static string m_LoggerDir = AitFileManager.CombinePath(new string[] { "Logs" });
        private static BlockingCollection<AitLog> m_Logger;

        private AitExceptionsLogger() 
        {
            m_Logger = new BlockingCollection<AitLog>(100);
            AitFileManager.CreateDirectory(AitFileManager.CombinePath(AitStrings.LOGDIR_SUBPATH));
        }

        private static object m_Locker = new object();
        private static AitExceptionsLogger m_Instance;
        public static AitExceptionsLogger Instance
        {
            get
            {
                lock (m_Locker)
                {
                    if (m_Instance == null)
                        m_Instance = new AitExceptionsLogger();
                    return m_Instance;
                }
            }
        }

        public async void LogToFile(AitLog newLog)
        {
            m_Logger.Add(newLog);
            await Task.Factory.StartNew( () => {
                while (!m_Logger.IsCompleted)
                {
                    AitLog log = null;
                    try
                    {
                        log = m_Logger.Take();

                        if (log != null)
                        {
                            var random = new Random();
                            var fileName = log.Title.ToString() + "-" + DateTime.Now.Date.ToString("yyyy-MM-dd-") + (random.Next()%1000).ToString() + AitStrings.LOG_EXT;
                            var filePath = Path.Combine(m_LoggerDir, fileName);
                            if (!File.Exists(filePath))
                                File.Create(filePath);

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
