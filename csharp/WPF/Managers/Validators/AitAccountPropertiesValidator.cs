using WPF.Models.Extensions.Exceptions;

namespace WPF.Managers.Validators
{
    public class AitAccountPropertiesValidator : BasePropertiesValidator
    {
        public static bool ValidateLogin(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.LoginException(Properties.Resources.LOGIN_REQUIRED);
            }

            if (value.Length < 4)
            {
                throw new AitAccountExceptions.LoginException(Properties.Resources.LOGIN_TOOSHORT);
            }

            return true;
        }

        public static bool ValidatePassword(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.PasswordException(Properties.Resources.PASS_REQUIRED); 
            }

            if (value.Length < 8)
            {
                throw new AitAccountExceptions.PasswordException(Properties.Resources.PASS_TOOWEAK); 
            }

            return true;
        }

        public static bool ValidateEmail(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AitAccountExceptions.EmailException(Properties.Resources.EMAIL_REQUIRED); 
            }

            var addr = new System.Net.Mail.MailAddress(value);
            if (addr.Address != value)
            {
                throw new AitAccountExceptions.EmailException(Properties.Resources.EMAIL_INCORECT); 
            }

            return true;
        }
    }
}
