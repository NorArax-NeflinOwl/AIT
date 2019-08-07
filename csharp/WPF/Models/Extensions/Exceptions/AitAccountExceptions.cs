using System;

namespace WPF.Models.Extensions.Exceptions
{
    public class AitAccountExceptions : BaseExceptions
    {
        public class LoginException : Exception
        {
            public LoginException(string message) : base(message)
            {
            }
        }

        public class PasswordException : Exception
        {
            public PasswordException(string message) : base(message)
            {
            }
        }

        public class EmailException : Exception
        {
            public EmailException(string message) : base(message)
            {
            }
        }

        public class AccoutnNotActivatedException : Exception
        {
            public AccoutnNotActivatedException(string message) : base(message)
            {
            }
        }

        public class HostException : Exception
        {
            public HostException(string message) : base(message)
            {
            }
        }

        public class CodeException : Exception
        {
            public CodeException(string message) : base(message)
            {
            }
        }
    }
}
