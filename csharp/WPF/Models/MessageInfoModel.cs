using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WPF.Models
{
    public class MessageInfoModel : ISerializable
    {
        public string Message { get; set; }

        private List<string> array;
        public string[] Array
        {
            get { return array?.ToArray(); }
            set { array = value?.ToList(); }
        }

        public string[] ExceptionInfo { get; set; }

        public MessageInfoModel() { }
        
        public MessageInfoModel(List<string> arr)
        {
            array = arr;
        }

        public MessageInfoModel(string message, Exception exception = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                Message = null;
            else
                Message = message;

            ExceptionInfo = GetException(exception, new List<string>()).ToArray();
        }

        public MessageInfoModel(SerializationInfo info, StreamingContext context)
        {
            Message = (string)info.GetValue(nameof(Message), typeof(string));
            Array = (string[])info.GetValue(nameof(Array), typeof(string[]));
            ExceptionInfo = (string[])info.GetValue(nameof(ExceptionInfo), typeof(string[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Message), Message, typeof(string));
            info.AddValue(nameof(Array), Array, typeof(string[]));
            info.AddValue(nameof(ExceptionInfo), ExceptionInfo, typeof(string[]));
        }

        public override string ToString()
        {
            var msg = string.Empty;

            if (!string.IsNullOrEmpty(Message))
                msg += Message.ToString() + Environment.NewLine;

            if(array?.Any() == true)
            {
                foreach (var item in array)
                    msg += item + Environment.NewLine;
            }

            if(ExceptionInfo?.Any() == true)
            {
                foreach (var exception in ExceptionInfo.ToList())
                    msg += exception;
            }

            return msg;
        }

        private List<string> GetException(Exception exception, List<string> list)
        {
            if (exception != null)
            {
                list.Add(exception.Message);
                var stackTrace = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                foreach (var line in stackTrace)
                    list.Add(line + Environment.NewLine);

                if (exception.InnerException != null)
                    list = GetException(exception.InnerException, list);
            }

            return list;
        }
    }
}
