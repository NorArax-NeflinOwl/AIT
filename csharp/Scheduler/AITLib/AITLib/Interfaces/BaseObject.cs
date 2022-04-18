using System.Runtime.Serialization;

namespace AITLib.Interfaces
{
    public interface BaseObject : ISerializable, ICloneable
    {
        public uint ID { get; }
        public uint ParentID { get; }
    }
}
