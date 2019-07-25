using System;
using System.Runtime.Serialization;

namespace WPF.Models
{
    public class HardwareModel : ISerializable, ICloneable
    {
        public string ProcessorInfo { get; set; }
        public string PhysicalMemory { get; set; }

        public HardwareModel() { }

        public HardwareModel(SerializationInfo info, StreamingContext context)
        {
            ProcessorInfo = (string)info.GetValue(nameof(ProcessorInfo), typeof(string));
            PhysicalMemory = (string)info.GetValue(nameof(PhysicalMemory), typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ProcessorInfo), ProcessorInfo, typeof(string));
            info.AddValue(nameof(PhysicalMemory), PhysicalMemory, typeof(string));
        }

        public object Clone()
        {
            var hardware = new HardwareModel
            {
                ProcessorInfo = ProcessorInfo,
                PhysicalMemory = PhysicalMemory
            };
            return hardware;
        }
    }
}
