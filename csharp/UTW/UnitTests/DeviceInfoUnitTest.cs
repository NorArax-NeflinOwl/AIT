using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using WPF.Managers;

namespace UTW.UnitTests
{
    [TestClass]
    public class DeviceInfoUnitTest
    {
        [TestMethod]
        public void DeviceMethodTest()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetAccountName()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetBIOScaption()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetBIOSmaker()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetBIOSserNo()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetBoardMaker()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetBoardProductId()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetCdRomDrive()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetComputerName()));
            Assert.IsTrue(HardwareManager.GetCPUCurrentClockSpeed() != 0);
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetCPUManufacturer()));
            Assert.IsTrue(HardwareManager.GetCpuSpeedInGHz() != 0);
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetCurrentLanguage()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetDefaultIPGateway()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetHDDSerialNo()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetMACAddress()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetNoRamSlots()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetOSInformation()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetPhysicalMemory()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetProcessorId()));
            Assert.IsTrue(!string.IsNullOrEmpty(HardwareManager.GetProcessorInformation()));
        }
#if DEBUG
        [TestMethod]
        public void DebugWrite()
        {
            Debug.WriteLine(HardwareManager.GetAccountName());
            Debug.WriteLine(HardwareManager.GetBIOScaption());
            Debug.WriteLine(HardwareManager.GetBIOSmaker());
            Debug.WriteLine(HardwareManager.GetBIOSserNo());
            Debug.WriteLine(HardwareManager.GetBoardMaker());
            Debug.WriteLine(HardwareManager.GetBoardProductId());
            Debug.WriteLine(HardwareManager.GetCdRomDrive());
            Debug.WriteLine(HardwareManager.GetComputerName()); //
            Debug.WriteLine(HardwareManager.GetCPUCurrentClockSpeed());
            Debug.WriteLine(HardwareManager.GetCPUManufacturer());
            Debug.WriteLine(HardwareManager.GetCpuSpeedInGHz());
            Debug.WriteLine(HardwareManager.GetCurrentLanguage()); //
            Debug.WriteLine(HardwareManager.GetDefaultIPGateway());
            Debug.WriteLine(HardwareManager.GetHDDSerialNo());
            Debug.WriteLine(HardwareManager.GetMACAddress());
            Debug.WriteLine(HardwareManager.GetNoRamSlots());
            Debug.WriteLine(HardwareManager.GetOSInformation()); //
            Debug.WriteLine(HardwareManager.GetPhysicalMemory()); //
            Debug.WriteLine(HardwareManager.GetProcessorId());
            Debug.WriteLine(HardwareManager.GetProcessorInformation()); //
        }
#endif
    }
}
