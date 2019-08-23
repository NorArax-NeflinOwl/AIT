using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WPF.Models
{
    public class ArrayMessageInfoModel : MessageInfoModel
    {
        private List<string> array;

        public string[] Array
        {
            get { return array.ToArray(); }
            set { array = value.ToList(); }
        }

        public ArrayMessageInfoModel() : base() { }

        public ArrayMessageInfoModel(List<string> arr)
        {
            this.array = arr;
        }

        public ArrayMessageInfoModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Array = (string[])info.GetValue(nameof(Array), typeof(string[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Array), Array, typeof(string[]));
            base.GetObjectData(info, context);
        }

        public override string ToString()
        {
            var msg = string.Empty;
            foreach (var item in array)
                msg += item + Environment.NewLine;

            return msg;
        }
    }
}
