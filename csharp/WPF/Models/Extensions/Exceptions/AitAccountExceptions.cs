using System;
using WPF.Models.Interfaces;

namespace WPF.Models.Extensions.Exceptions
{
    public class AitAccountExceptions : BaseExceptions
    {
        public class LoginException : Exception, ICustomException
        {
            public LoginException(string message) : base(message)
            {
            }
        }

        public class PasswordException : Exception, ICustomException
        {
            public PasswordException(string message) : base(message)
            {
            }
        }

        public class EmailException : Exception, ICustomException
        {
            public EmailException(string message) : base(message)
            {
            }
        }

        public class AccoutnNotActivatedException : Exception, ICustomException
        {
            public AccoutnNotActivatedException(string message) : base(message)
            {
            }
        }

        public class HostException : Exception, ICustomException
        {
            public HostException(string message) : base(message)
            {
            }
        }

        public class CodeException : Exception, ICustomException
        {
            public CodeException(string message) : base(message)
            {
            }
        }
    }
}
