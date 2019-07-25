using WPF.Models.Extensions.Exceptions;

namespace WPF.Managers.Helpers
{
    public class Converters
    {
        public static string Digit2StringCreate(int digit, int length)
        {
            var result = digit.ToString();
            var diffLength = length - result.Length;
            if (diffLength < 0)
                throw new GenerateExceptions.InvalidDigitLenght("Too big number");

            for (int i = 0; i < diffLength; i++)
            {
                result = "0" + result;
            }

            return result;
        }
    }
}
