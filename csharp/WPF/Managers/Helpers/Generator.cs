using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Enums;
using WPF.Exceptions;
using WPF.Validators;

namespace WPF.Managers.Helpers
{
    public class Generator
    {
        private static readonly char separator = '-';

        public static string IDGenerator(IDInerfixEnum tablePrefix)
        {
            return IDGenerator(IDPrefixEnum.AIT, tablePrefix);
        }

        public static string IDGenerator(IDPrefixEnum prefix, IDInerfixEnum inerfix)
        {
            var id = "0000000";

            using(var context = PDBContext.Instance.Context)
            {
                SysStsgenids sysids = context.Stsgenids.OrderByDescending(q => q.Create).ToList().FirstOrDefault();
                if(sysids != null)
                {
                    var oldid = int.Parse(sysids.ID.Split(separator)[2]);
                    id = Digit2StringCreate(oldid + 1, 7);
                }
                else
                {
                    ExceptionManager.Instance.LogExceptionToFile(new SqliteExceptions.EntityNotFound("Stsgenids table is empty"));
                }
            }

            var newid = prefix.ToString() + separator + inerfix.ToString() + separator + id;
            if (BasePropertiesValidator.ValidateID(newid))
                return newid;
            else
                throw new BaseExceptions.IDException("Incorect id generate");
        }

        public static string Digit2StringCreate(int digit, int length)
        {
            var result = digit.ToString();
            var diffLength = length - result.Length;
            if (diffLength < 0)
                throw new GenerateExceptions.InvalidDigitLenght("Too big number");

            for(int i = 0; i < diffLength; i++)
            {
                result = "0" + result;
            }

            return result;
        }

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
