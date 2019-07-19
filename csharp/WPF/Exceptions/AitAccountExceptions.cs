using System;

namespace WPF.Exceptions
{
    public class AitAccountExceptions
    {
        public class IDException : Exception
        {
            public IDException(string message) : base(message)
            {
            }
        }

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
    }
}
