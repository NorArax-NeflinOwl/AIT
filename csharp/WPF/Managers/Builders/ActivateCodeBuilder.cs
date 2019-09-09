using System;
using WPF.Managers.Helpers;

namespace WPF.Managers.Builders
{
    public class ActivateCodeBuilder
    {
        public static string GenerateActivateCode(int? input = null, int length = 10)
        {
            var subput = Generators.GenerateSha256Hash(DateTime.Now.ToString());
            if (input != null)
                subput += input;

            return Generators.GenerateSha256Hash(subput).Substring(0, length).ToUpper();
        }
    }
}
