using WPF.Enums;
using WPF.Exceptions;

namespace WPF.Validators
{
    public class BasePropertiesValidator
    {
        protected static readonly char idSeperator = '-';

        public static bool ValidateID(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new BaseExceptions.IDException("ID is required");
            }

            if (value.Length != 15)
            {
                throw new BaseExceptions.IDException("ID length must equals 15");
            }

            var idParts = value.Split(idSeperator);
            if (idParts.Length != 3)
            {
                throw new BaseExceptions.IDException("ID don't containt 2 separators");
            }

            if (!int.TryParse(idParts[2], out _))
            {
                throw new BaseExceptions.IDException("ID postfix is not number");
            }
            return true;
        }

        public static bool ValidatePrimaryKey(string value, IDInerfixEnum tablePrefix)
        {
            if(ValidateID(value) && !tablePrefix.ToString().Equals(value.Split(idSeperator)[1]))
            {
                throw new BaseExceptions.IDException("Invalid table prefix");
            }

            return true;
        }
    }
}
