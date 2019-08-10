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
            var result = GetInnersExceptions(Exception, string.Empty);

            if (Message != null)
                result = CryptoJsonManager.Instance.Serialize(this);

            return result;
        }

        private string GetInnersExceptions(Exception exception, string output)
        {
            if(exception != null)
            {
                output += exception.Message + Environment.NewLine + exception.StackTrace;
                return GetInnersExceptions(exception.InnerException, output);
            }
            return output;
        }
    }
}
