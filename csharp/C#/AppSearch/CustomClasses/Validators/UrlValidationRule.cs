using System.Globalization;
using System.Windows.Controls;

namespace AppSearch.CustomClasses.Validators
{
    public class UrlValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? url = value as string;
            if (string.IsNullOrEmpty(url))
            {
                return ValidationResult.ValidResult;
            }

            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isValidUrl)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Please enter a valid URL.");
            }
        }
    }
}
