using System;
using System.Runtime.Serialization;

namespace WPF.Models
{
    public class HardwareModel : ISerializable, ICloneable
    {
        public HardwareModel() { }

        public HardwareModel(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
