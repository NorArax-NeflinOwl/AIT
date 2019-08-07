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
                throw new BaseExceptions.IDException(Properties.Resources.ID_REQUIRED);
            }

            if (value.Length != 15)
            {
                throw new BaseExceptions.IDException(Properties.Resources.ID_LENINCORECT);
            }

            var idParts = value.Split(idSeperator);
            if (idParts.Length != 3)
            {
                throw new BaseExceptions.IDException(Properties.Resources.ID_FORMATINCORECT);
            }

            if (!int.TryParse(idParts[2], out _))
            {
                throw new BaseExceptions.IDException(Properties.Resources.ID_NUMBERMISING);
            }
            return true;
        }

        public static bool ValidatePrimaryKey(string value, TableInerfixEnum tablePrefix)
        {
            if(ValidateID(value) && !tablePrefix.ToString().Equals(value.Split(idSeperator)[1]))
            {
                throw new BaseExceptions.IDException(Properties.Resources.PREFIX_INVALID);
            }

            return true;
        }
    }
}
