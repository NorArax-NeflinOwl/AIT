using AITLib.Constants;
using System.Globalization;
using System.Windows.Controls;

namespace AIT.UIValidators
{
    public class AitRequiredFieldValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, AitStrings.REQUIRED_FIELD)
                : ValidationResult.ValidResult;
        }
    }
}
