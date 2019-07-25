using System;

namespace WPF.Models.Extensions.Exceptions
{
    public class BaseExceptions
    {
        public class IDException : Exception
        {
            public IDException(string message) : base(message)
            {
            }
        }
    }
}
