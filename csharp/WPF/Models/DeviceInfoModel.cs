using System;
using System.Runtime.Serialization;

namespace WPF.Models
{
    public class DeviceInfoModel : ISerializable, ICloneable
    {
        public HardwareModel Hardware { get; set; }
        public SoftwareModel Software { get; set; }

        public DeviceInfoModel() { }

        public DeviceInfoModel(SerializationInfo info, StreamingContext context)
        {
            Hardware = (HardwareModel)info.GetValue(nameof(Hardware), typeof(HardwareModel));
            Software = (SoftwareModel)info.GetValue(nameof(Software), typeof(SoftwareModel));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Hardware), Hardware, typeof(HardwareModel));
            info.AddValue(nameof(Software), Software, typeof(SoftwareModel));
        }

        public object Clone()
        {
            var clone = new DeviceInfoModel
            {
                Hardware = (HardwareModel)Hardware.Clone(),
                Software = (SoftwareModel)Software.Clone()
            };
            return clone;
        }
    }
}
