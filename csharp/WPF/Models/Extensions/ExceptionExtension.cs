using System;
using WPF.Managers;

namespace WPF.Models.Extensions
{
    public class ExceptionExtension
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public ExceptionExtension(string message, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(message))
                Message = null;
            else
                Message = message;

            Exception = exception;
        }

        public override string ToString()
        {
            var result = Exception.Message + Environment.NewLine + Exception.StackTrace;

            if (Message != null)
                result = CryptoJsonManager.Instance.Serialize(this);

            return result;
        }
    }
}
