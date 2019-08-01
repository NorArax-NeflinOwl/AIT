using System;

namespace WPF.Models.Extensions.Exceptions
{
    public class LoginPageExceptions
    {
        public class EmptyLoginException : Exception
        {
            public EmptyLoginException(string message) : base(message)
            { }
        }
    }
}
