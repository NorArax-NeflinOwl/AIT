using WPF.Models.Extensions.Exceptions;

namespace WPF.Managers.Validators
{
    public class AitAccountPropertiesValidator : BasePropertiesValidator
    {
        public static bool ValidateLogin(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.LoginException("Login is required");
            }

            if (value.Length < 4)
            {
                throw new AitAccountExceptions.LoginException("Login is too short");
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
