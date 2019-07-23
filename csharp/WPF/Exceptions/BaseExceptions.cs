using System;

namespace WPF.Exceptions
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
