using System.Runtime.Serialization;
using WPF.Models.Interfaces;

namespace WPF.Models
{
    public class SimpleMessageInfoModel : MessageInfoModel
    {

        public SimpleMessageInfoModel() : base()
        {
        }

        public SimpleMessageInfoModel(string message)
        {
            Message = message;
        }

        public SimpleMessageInfoModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Message = (string)info.GetValue(nameof(Message), typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Message), Message, typeof(string));
        }
    }
}
