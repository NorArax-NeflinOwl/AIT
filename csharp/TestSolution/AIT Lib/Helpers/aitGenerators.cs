using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace AIT_Lib.Helpers
{
    public static class aitGenerators
    {
        public static string GenerateHashID()
        {
            return DateTime.Now.ToUniversalTime().GetHashCode().ToString();
        }

        public static string GenerateMD5Hash(string obj)
        {
            var hash = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, obj);
            }
            return hash;
        }

        public static bool VerifyMD5Hash(string plaintText, string hash)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return VerifyMd5Hash(md5Hash, plaintText, hash);
            }
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);

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
