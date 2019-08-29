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
using WPF.GUI.Windows.Properties;
using WPF.Models.Interfaces;
using System.Windows;
using WPF.Databases.Models;
using WPF.Managers.Helpers;
using System.Linq;
using System.Configuration;

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
                                list = CryptoJsonManager.Instance.Deserialize<List<LogInfoModel>>(content, false);
                            }

                            if(log.Type.Equals(FileTypesEnum.EXCEPTION))
                            {
                                //
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

                        //m_Logger.Add(newLog);
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

        public void LogExceptionToFileAndDB(Exception e, string message = "")
        {
            var log = new LogInfoModel
            {
                Type = FileTypesEnum.EXCEPTION,
                Message = new MessageInfoModel(message, e)
            };
            LogToFile(log);
            LogToDB(log);

            try
            {
                if(e is ICustomException)
                {
                    MainContext.Instance.Windows.Open(new PopupProperties(Resources.INFORMATION, e.Message, 5), false);
                }
                else
                {
                    // FIX ME
                    //MainContext.Instance.Windows.Open(new DialogProperties(log, DialogTypeEnum.EXCEPTION_HANDLER), false);
                    MessageBox.Show(log.ToString(), Resources.WARNING, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch(Exception ex)
            {
                log = new LogInfoModel
                {
                    Type = FileTypesEnum.EXCEPTION,
                    Message = new MessageInfoModel(message, ex)
                };
                LogToFile(log);

                MainContext.Instance.Windows.Open(new PopupProperties(Resources.INFORMATION, Resources.ERROR_NOHANDLE, 10), false);
                //MessageBox.Show(log.ToString(), Resources.EXCEPTION, MessageBoxButton.OK, MessageBoxImage.Error);
#if DEBUG
                throw ex;
#endif
            }
        }

        private void LogToDB(LogInfoModel log)
        {
            var msg = CryptoJsonManager.Instance.Serialize(log);
            try
            {
                var list = new List<LogInfoModel>();
                using (var context = PDBContext.Instance.Context)
                {
                    var fileInDB = context.Files.Where(q => (q.Create.Day > DateTime.Now.AddDays(-1).Day || !q.Create.Month.Equals(DateTime.Now.AddDays(-1).Month))
                                                    && q.Type.Equals(log.Type)).FirstOrDefault();
                    if (fileInDB != null)
                    {
                        var content = fileInDB.Content;
                        list = CryptoJsonManager.Instance.Deserialize<List<LogInfoModel>>(content, false);

                        if (list == null)
                            list = new List<LogInfoModel>();

                        list.Add(log);
                        fileInDB.FileOwner = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                        fileInDB.Content = CryptoJsonManager.Instance.Serialize(list);
                        fileInDB.Update();
                    }
                    else
                    {
                        list.Add(log);
                        var logToSave = new AitFileModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                            FileCreator = context.Accounts.Where(q => q.ID.Equals(ConfigurationManager.AppSettings["TasksManager"].ToString())).FirstOrDefault(),
                            FileOwner = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault(),
                            Name = nameof(LogManager) + "-" + log.Type.ToString(),
                            Type = log.Type,
                            Content = CryptoJsonManager.Instance.Serialize(list)
                        };
                        logToSave.Insert();
                    }
                }
            }
            catch(Exception ex)
            {
                log = new LogInfoModel
                {
                    Type = FileTypesEnum.EXCEPTION,
                    Message = new MessageInfoModel(msg, ex)
                };
                LogToFile(log);
                MessageBox.Show(log.ToString(), Resources.EXCEPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
