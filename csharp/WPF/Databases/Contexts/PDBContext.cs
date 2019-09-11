using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Models;

namespace WPF.Databases.Contexts
{
    public class PDBContext
    {
        private DeviceInfoModel deviceInfo;
        private IDictionary<string, string> sessionDictionary;
        private static readonly object locker = new object();
        private static readonly PDBContext instance = new PDBContext();

        private PDBContext()
        {
            sessionDictionary = new Dictionary<string, string>();
        }

        public static PDBContext Instance
        {
            get
            {
                lock(locker)
                {
                    return instance;
                }
            }
        }

        public DBContext Context
        {
            get
            {
                return new DBContext();
            }
        }

        public DeviceInfoModel DeviceInfo
        {
            get
            {
                if (deviceInfo == null)
                {
                    deviceInfo = new DeviceInfoModel
                    {
                        Hardware = new HardwareModel
                        {
                            PhysicalMemory = HardwareManager.GetPhysicalMemory(),
                            ProcessorInfo = HardwareManager.GetProcessorInformation()
                        },
                        Software = new SoftwareModel
                        {
                            ComputerName = HardwareManager.GetComputerName(),
                            CurrentLanguage = HardwareManager.GetCurrentLanguage(),
                            OSInfo = HardwareManager.GetOSInformation()
                        }
                    };
                }
                return deviceInfo;
            }
        }

        /// <summary>
        /// Account ID from session
        /// </summary>
        public string AccountID { get; set; }

        public IDictionary<string, string> SessionDictionary
        {
            get => sessionDictionary;
            set
            {
                sessionDictionary = value;
                if(value != null)
                {
                    MainContext.Instance.Windows.DeserializeSession();
                }
            }
        }

        public void SaveSession()
        {
            try
            {
                var json = CryptoJsonManager.Instance.Serialize(SessionDictionary);

                using (var context = Context)
                {
                    AitAccountModel fileCreator = null, assignedTo = null;
                    var creator = ConfigurationManager.AppSettings["TasksManager"].ToString();
                    if (!string.IsNullOrEmpty(creator))
                    {
                        fileCreator = context.Accounts.Where(q => q.ID.Equals(creator)).FirstOrDefault();
                    }

                    var sessionFile = context.Files.Where(q => !string.IsNullOrEmpty(q.Creator) && q.Creator.Equals(creator)
                                                            && !string.IsNullOrEmpty(q.AssignedTo) && q.AssignedTo.Equals(AccountID)
                                                            && q.Name.Equals(nameof(SessionDictionary))).FirstOrDefault();
                    if (sessionFile == null)
                    {
                        assignedTo = context.Accounts.Where(q => q.ID.Equals(AccountID)).FirstOrDefault();

                        sessionFile = new AitFileModel(context)
                        {
                            FileCreator = fileCreator,
                            FileOwner = assignedTo,
                            Name = nameof(SessionDictionary),
                            Content = json
                        };
                        sessionFile.Insert();
                    }
                    else
                    {
                        sessionFile.Content = json;
                        sessionFile.Update();
                    }
                }
            }
            catch(Exception e)
            {
                LogManager.Instance.LogExceptionToFileAndDB(e);
            }
        }
    }
}
