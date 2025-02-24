using System.Globalization;
using System.Windows.Controls;

namespace AppSearch.CustomClasses.Validators
{
    public class RangeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public RangeValidationRule()
        {

        }

        public RangeValidationRule(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string input && int.TryParse(input, out int result))
            {
                if (result >= Min && result <= Max)
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, $"Please enter a value between {Min} and {Max}.");
                }
            }
            return new ValidationResult(false, "Invalid input.");
        }
    }
}
