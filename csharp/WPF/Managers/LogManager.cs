using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using WPF.Models.Enums;
using WPF.Models;
using WPF.Properties;
using WPF.Databases.Contexts;
using WPF.Models.Extensions;

namespace WPF.Managers
{
    public class LogManager
    {
        private readonly JsonSerializerSettings m_Setting;
        private static readonly string m_LoggerDir = FileManager.CombinePath(new string[] { Environment.CurrentDirectory, Resources.LOGDIR_SUBPATH });
        private static BlockingCollection<LogInfoModel> m_Logger;
        private int HandleErrorCounter;

        private LogManager()
        {
            HandleErrorCounter = 0;
            m_Logger = new BlockingCollection<LogInfoModel>(100);
            FileManager.CreateDirectory(m_LoggerDir);
            m_Setting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        private static readonly object m_Locker = new object();
        private static LogManager m_Instance;
        public static LogManager Instance
        {
            get
            {
                lock (m_Locker)
                {
                    if (m_Instance == null)
                        m_Instance = new LogManager();
                    return m_Instance;
                }
            }
        }

        public async void LogToFile(LogInfoModel newLog)
        {
            m_Logger.Add(newLog);
            await Task.Factory.StartNew(() => {
                while (!m_Logger.IsCompleted)
                {
                    try
                    {
                        var log = m_Logger.Take();

                        if (log != null)
                        {
                            var random = new Random();
                            var ext = GetExt(log.Type);

                            if (string.IsNullOrEmpty(log.Path))
                                log.Path = m_LoggerDir;

                            var fileName = log.Type.ToString() + "-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ext;
                            var filePath = Path.Combine(log.Path, fileName);

                            if (!File.Exists(filePath))
                                using (File.Create(filePath)) { }

                            using (var stream = File.AppendText(filePath))
                            {
                                var json = JsonConvert.SerializeObject(log, m_Setting);
                                stream.WriteLine(json);
                            }

                            if(FileTypesEnum.EXCEPTION.Equals(log.Type))
                            {
                                var mainWindow = WindowsDictionary.GetMainWindow();
                                if (mainWindow != null)
                                {
                                    mainWindow.RefreshErrorInfo(++HandleErrorCounter);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        /* m_Logger.Add(new LogInfoModel
                        {
                            Type = FileTypesEnum.EXCEPTION,
                            Message = e.Message + Environment.NewLine + e.StackTrace
                        });*/

                        m_Logger.Add(newLog);
                    }
                }
            });
        }

        private string GetExt(FileTypesEnum title)
        {
            switch(title)
            {
                case FileTypesEnum.EXCEPTION:
                    return Resources.ERR_EXT;
                case FileTypesEnum.INFORMATION:
                    return Resources.LOG_EXT;
                case FileTypesEnum.NOTE:
                    return Resources.NOTE_EXT;
                case FileTypesEnum.TRACE:
                    return Resources.TRC_EXT;
                default:
                    return Resources.TXT_EXT;
            }
        }

        public void LogExceptionToFile(Exception e)
        {
            LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.EXCEPTION,
                Message = e.Message + Environment.NewLine + e.StackTrace
            });

            // TODO open error dialog window and deleted throw statment from this method

#if DEBUG
            throw e;
#endif
        }
    }
}
