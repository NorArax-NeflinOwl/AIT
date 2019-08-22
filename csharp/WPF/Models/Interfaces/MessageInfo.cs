using System.Runtime.Serialization;
using WPF.Models.Enums;

namespace WPF.Models.Interfaces
{
    public abstract class MessageInfo : ISerializable
    {
        public NoteTypesEnum Type { get; set; }
        public string Message { get; set; }

        public MessageInfo() { }

        public MessageInfo(SerializationInfo info, StreamingContext context)
        {
            Type = (NoteTypesEnum)info.GetValue(nameof(Type), typeof(NoteTypesEnum));
            Message = (string)info.GetValue(nameof(Message), typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Type), Type, typeof(NoteTypesEnum));
            info.AddValue(nameof(Message), Message, typeof(string));
        }
    }
}
