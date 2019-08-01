using WPF.Models.Enums;
using WPF.Models.Extensions.Exceptions;

namespace WPF.Managers.Validators
{
    public class BasePropertiesValidator
    {
        protected static readonly char idSeperator = '-';

        public static bool ValidateID(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new BaseExceptions.IDException("ID is required"); // TODO Resources
            }

            if (value.Length != 15)
            {
                throw new BaseExceptions.IDException("ID length must equals 15"); // TODO Resources
            }

            var idParts = value.Split(idSeperator);
            if (idParts.Length != 3)
            {
                throw new BaseExceptions.IDException("ID don't containt 2 separators"); // TODO Resources
            }

            if (!int.TryParse(idParts[2], out _))
            {
                throw new BaseExceptions.IDException("ID postfix is not number"); // TODO Resources
            }
            return true;
        }

        public static bool ValidatePrimaryKey(string value, TableInerfixEnum tablePrefix)
        {
            if(ValidateID(value) && !tablePrefix.ToString().Equals(value.Split(idSeperator)[1]))
            {
                throw new BaseExceptions.IDException("Invalid table prefix"); // TODO Resources
            }

            return true;
        }
    }
}
