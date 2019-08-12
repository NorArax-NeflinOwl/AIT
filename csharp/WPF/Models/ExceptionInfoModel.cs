using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WPF.Models.Interfaces;

namespace WPF.Models
{
    public class ExceptionInfoModel : IMessageInfo, ISerializable
    {
        public string Message { get; set; }
        public string[] ExceptionInfo { get; set; }

        public ExceptionInfoModel(string message, Exception exception = null)
        { 
            if (string.IsNullOrWhiteSpace(message))
                Message = null;
            else
                Message = message;

            ExceptionInfo = GetException(exception, new List<string>()).ToArray();
        }

        public ExceptionInfoModel(SerializationInfo info, StreamingContext context)
        {
            Message = (string)info.GetValue(nameof(Message), typeof(string));
            ExceptionInfo = (string[])info.GetValue(nameof(ExceptionInfo), typeof(string[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Message), Message, typeof(int));
            info.AddValue(nameof(ExceptionInfo), ExceptionInfo, typeof(string[]));
        }

        private List<string> GetException(Exception exception, List<string> list)
        {
            if(exception != null)
            {
                list.Add(exception.Message);
                var stackTrace = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                foreach (var line in stackTrace)
                    list.Add(line);

                if (exception.InnerException != null)
                    list = GetException(exception.InnerException, list);
            }

            return list;
        }
    }
}
