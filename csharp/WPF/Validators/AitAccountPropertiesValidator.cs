using WPF.Exceptions;

namespace WPF.Validators
{
    public class AitAccountPropertiesValidator
    {
        private static readonly char idSeperator = '-';

        public static bool ValidateID(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.IDException("ID is required");
            }

            if (value.Length != 15)
            {
                throw new AitAccountExceptions.IDException("ID length must equals 15");
            }

            var idParts = value.Split(idSeperator);
            if (idParts.Length != 3)
            {
                throw new AitAccountExceptions.IDException("ID don't containt 2 separators");
            }

            if (!int.TryParse(idParts[2], out _))
            {
                throw new AitAccountExceptions.IDException("ID postfix is not number");
            }
            return true;
        }

        public static bool ValidateLogin(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.LoginException("Login is required");
            }

            if (value.Length < 4)
            {
                throw new AitAccountExceptions.LoginException("Password is too short");
            }

            return true;
        }

        public static bool ValidatePassword(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.PasswordException("Password is required");
            }

            if (value.Length < 8)
            {
                throw new AitAccountExceptions.PasswordException("Password is too weak");
            }

            return true;
        }

        public static bool ValidateEmail(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.EmailException("Email is required");
            }

            var addr = new System.Net.Mail.MailAddress(value);
            if (addr.Address != value)
            {
                throw new AitAccountExceptions.EmailException("Incorect Email");
            }

            return true;
        }
    }
}
