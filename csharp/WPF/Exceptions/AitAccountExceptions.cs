using System;

namespace WPF.Exceptions
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
    }
}
