using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WPF.Enums;
using WPF.Models;
using WPF.Properties;

namespace WPF.Managers
{
    public class ExceptionManager
    {
        private static readonly string m_LoggerDir = FileManager.CombinePath(new string[] { Resources.LOGDIR_SUBPATH });
        private static BlockingCollection<LogModel> m_Logger;

        private ExceptionManager()
        {
            m_Logger = new BlockingCollection<LogModel>(100);
            FileManager.CreateDirectory(FileManager.CombinePath(Resources.LOGDIR_SUBPATH));
        }

        private static readonly object m_Locker = new object();
        private static ExceptionManager m_Instance;
        public static ExceptionManager Instance
        {
            get
            {
                lock (m_Locker)
                {
                    if (m_Instance == null)
                        m_Instance = new ExceptionManager();
                    return m_Instance;
                }
            }
        }

        public async void LogToFile(LogModel newLog)
        {
            m_Logger.Add(newLog);
            await Task.Factory.StartNew(() => {
                while (!m_Logger.IsCompleted)
                {
                    LogModel log = null;
                    try
                    {
                        log = m_Logger.Take();

                        if (log != null)
                        {
                            var random = new Random();
                            var ext = GetExt(log.Type);
                            var fileName = log.Title + "-" + DateTime.Now.Date.ToString("yyyy-MM-dd-") + (random.Next() % 1000).ToString() + ext;
                            var filePath = Path.Combine(m_LoggerDir, fileName);
                            if (!File.Exists(filePath))
                                using (File.Create(filePath)) { }

                            using (var stream = File.AppendText(filePath))
                            {
                                stream.WriteLine(log.Message);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            });
        }

        private string GetExt(FileTypeEnum title)
        {
            switch(title)
            {
                case FileTypeEnum.EXCEPTION:
                    return Resources.ERR_EXT;
                case FileTypeEnum.INFORMATION:
                    return Resources.LOG_EXT;
                case FileTypeEnum.NOTE:
                    return Resources.NOTE_EXT;
                case FileTypeEnum.TRACE:
                    return Resources.TRC_EXT;
                default:
                    return Resources.TXT_EXT;
            }
        }

        public void LogExceptionToFile(Exception e)
        {
            LogToFile(new LogModel
            {
                Type = FileTypeEnum.EXCEPTION,
                Title = e.Message,
                Message = e.StackTrace
            });
#if DEBUG
            throw e;
#endif
        }
    }
}
