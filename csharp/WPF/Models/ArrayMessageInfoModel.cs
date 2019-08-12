using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WPF.Models.Interfaces;

namespace WPF.Models
{

    public class ArrayMessageInfoModel : IMessageInfo, ISerializable
    {
        private List<string> array;

        public string Message { get; set; }

        public string[] Array
        {
            get { return array.ToArray(); }
            set { array = value.ToList(); }
        }

        public ArrayMessageInfoModel(List<string> arr)
        {
            this.array = arr;
        }

        public ArrayMessageInfoModel(SerializationInfo info, StreamingContext context)
        {
            Array = (string[])info.GetValue(nameof(Array), typeof(string[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Array), Array, typeof(string[]));
        }
    }
}
