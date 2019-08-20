using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using WPF.Models.Enums;
using WPF.Models;
using WPF.Properties;
using WPF.Models.Extensions;
using System.Collections.Generic;
using WPF.Databases.Contexts;
using WPF.UI.Windows.Properties;
using WPF.Models.Interfaces;

namespace WPF.Managers
{
    public partial class LogManager
    {
        private static readonly string m_LoggerDir = FileManager.CombinePath(new string[] { Environment.CurrentDirectory, Resources.LOGDIR_SUBPATH });
        private static BlockingCollection<LogInfoModel> m_Logger;

        private LogManager()
        {
            m_Logger = new BlockingCollection<LogInfoModel>(100);
            FileManager.CreateDirectory(m_LoggerDir);
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

                            List<LogInfoModel> list = null;
                            var content = File.ReadAllText(filePath);
                            if(!string.IsNullOrEmpty(content))
                            {
                                list = CryptoJsonManager.Instance.Deserialize<List<LogInfoModel>>(content);
                            }

                            if(list == null)
                            {
                                list = new List<LogInfoModel>();
                            }

                            list.Add(log);
                            var json = CryptoJsonManager.Instance.Serialize(list);
                            File.WriteAllText(filePath, json);
                        }
                    }
                    catch (Exception)
                    {
                        // TODO This exception should be handled by another process (and file if this is nessesery)
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
                case FileTypesEnum.KEYLOGGER:
                    return Resources.KEYLOGGER_EXT;
                default:
                    return Resources.TXT_EXT;
            }
        }

        public void LogExceptionToFile(Exception e, string message = "")
        {
            var log = new LogInfoModel
            {
                Type = FileTypesEnum.EXCEPTION,
                Message = new ExceptionInfoModel(message, e)
            };
            LogToFile(log);

            try
            {
                if(e is ICustomException)
                {
                    MainContext.Instance.Windows.Open(new PopupProperties(Resources.INFORMATION, e.Message, 5), false);
                }
                else
                {
                    MainContext.Instance.Windows.Open(new DialogProperties(log, DialogTypeEnum.EXCEPTION_HANDLER), false);
                }
            }
            catch(Exception ex)
            {
                log = new LogInfoModel
                {
                    Type = FileTypesEnum.EXCEPTION,
                    Message = new ExceptionInfoModel(message, ex)
                };
                LogToFile(log);

                MainContext.Instance.Windows.Open(new PopupProperties(Resources.INFORMATION, Resources.ERROR_NOHANDLE, 10), false);
#if DEBUG
                throw ex;
#endif
            }
        }
    }
}
