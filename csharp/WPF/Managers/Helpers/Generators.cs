using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WPF.Databases.Contexts;
using WPF.Models.Enums;
using WPF.Models.Extensions.Exceptions;
using WPF.Managers.Validators;
using System.Collections.Generic;

namespace WPF.Managers.Helpers
{
    public class Generators
    {
        public static string GenerateSha256Hash(string obj)
        {
            var hash = string.Empty;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hash = GetSha256Hash(sha256Hash, obj);
            }
            return hash;
        }

        public static bool VerifySha256Hash(string plaintText, string hash)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return VerifySha256(sha256Hash, plaintText, hash);
            }
        }

        private static string GetSha256Hash(SHA256 sha256Hash, string input)
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static bool VerifySha256(SHA256 sha256Hash, string input, string hash)
        {
            string hashOfInput = GetSha256Hash(sha256Hash, input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
