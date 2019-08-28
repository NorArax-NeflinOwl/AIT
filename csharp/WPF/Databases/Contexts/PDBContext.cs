using WPF.Managers;
using WPF.Models;

namespace WPF.Databases.Contexts
{
    public class PDBContext
    {
        private DeviceInfoModel deviceInfo;

        private static readonly object locker = new object();
        private static readonly PDBContext instance = new PDBContext();

        private PDBContext()
        {
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
    }
}
