using System;
using System.Runtime.Serialization;

namespace WPF.Models
{
    public class SoftwareModel : ISerializable, ICloneable
    {
        public string ComputerName { get; set; }
        public string CurrentLanguage { get; set; }
        public string OSInfo { get; set; }

        public SoftwareModel() { }

        public SoftwareModel(SerializationInfo info, StreamingContext context)
        {
            ComputerName = (string)info.GetValue(nameof(ComputerName), typeof(string));
            CurrentLanguage = (string)info.GetValue(nameof(CurrentLanguage), typeof(string));
            OSInfo = (string)info.GetValue(nameof(OSInfo), typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ComputerName), ComputerName, typeof(string));
            info.AddValue(nameof(CurrentLanguage), CurrentLanguage, typeof(string));
            info.AddValue(nameof(OSInfo), OSInfo, typeof(string));
        }

        public object Clone()
        {
            var software = new SoftwareModel
            {
                ComputerName = ComputerName,
                CurrentLanguage =CurrentLanguage,
                OSInfo = OSInfo
            };
            return software;
        }
    }
}
