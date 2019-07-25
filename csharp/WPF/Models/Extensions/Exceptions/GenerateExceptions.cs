using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
