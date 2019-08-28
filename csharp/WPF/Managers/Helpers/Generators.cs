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
        private static readonly char separator = '-';

        private static List<string> UsedByNotSavedIDs = new List<string>();

        public static string GenerateActivateCode(int? input = null, int length = 10)
        {
            var subput = GenerateSha256Hash(DateTime.Now.ToString());
            if (input != null)
                subput += input;

            return GenerateSha256Hash(subput).Substring(0, length).ToUpper();
        }

        public static string RecordIDGenerator(TableInerfixEnum tablePrefix, bool throwException = true)
        {
            return RecordIDGenerator(SystemPrefixEnum.AIT, tablePrefix, throwException);
        }

        public static string RecordIDGenerator(SystemPrefixEnum systemPrefix, TableInerfixEnum tablePrefix, bool throwException)
        {
            var id = "0000000";

            if(UsedByNotSavedIDs.Any())
            {
                var oldid = int.Parse(UsedByNotSavedIDs.Last().Split(separator)[2]);
                id = Converters.Digit2StringCreate(oldid + 1, 7);
            }
            else
            {
                using (var context = PDBContext.Instance.Context)
                {
                    var sysid = context.Stsgenids.OrderByDescending(q => q.ID.Split(separator)[2]).ToList().FirstOrDefault();
                    if (sysid != null)
                    { 
                        var oldid = int.Parse(sysid.ID.Split(separator)[2]);
                        id = Converters.Digit2StringCreate(oldid + 1, 7);
                    }
                    else if (throwException)
                    {
                        LogManager.Instance.LogExceptionToFileAndDB(new SqliteExceptions.EntityNotFound("Stsgenids table is empty"));
                    }
                }
            }

            var newid = systemPrefix.ToString() + separator + tablePrefix.ToString() + separator + id;
            if (BasePropertiesValidator.ValidateID(newid))
            {
                UsedByNotSavedIDs.Add(newid);
                return newid;
            }
            else
                throw new BaseExceptions.IDException("Incorect id generate");
        }

        public static void ClearLocalIDs()
        {
            UsedByNotSavedIDs.Clear();
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
