using System;

namespace WPF.Models.Extensions.Exceptions
{
    public class GenerateExceptions
    {
        public class InvalidDigitLenght : Exception
        {
            public InvalidDigitLenght(string message) : base(message)
            {
            }
        }
    }
}
