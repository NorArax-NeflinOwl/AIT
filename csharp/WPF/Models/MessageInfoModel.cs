using System.Runtime.Serialization;

namespace WPF.Models
{
    public class MessageInfoModel : ISerializable
    {
        public string Message { get; set; }

        public MessageInfoModel() { }

        public MessageInfoModel(SerializationInfo info, StreamingContext context)
        {
            Message = (string)info.GetValue(nameof(Message), typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Message), Message, typeof(string));
        }

        public override string ToString()
        {
            return Message.ToString();
        }
    }
}
