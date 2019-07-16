using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace AIT.Helpers
{
    public class aitEncryptionHelper
    {
        private const string m_Key = "NorAraxNeflinOwl";
        private const string m_EncryptionKey = "AIT_CryptiographyModules_v1.0";

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                var UTF8 = new UTF8Encoding();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(m_EncryptionKey, UTF8.GetBytes(m_Key));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                var UTF8 = new UTF8Encoding();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(m_EncryptionKey, UTF8.GetBytes(m_Key));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        /*
        private const string m_Key = "NorAraxNeflinOwl";

        public static string Encrypt(string plainText)
        {
            String encryptedText;
            var UTF8 = new UTF8Encoding();
            var tdes = new AesManaged();
            tdes.Key = UTF8.GetBytes(m_Key);
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            using (var crypt = tdes.CreateEncryptor())
            {
                byte[] plain = Encoding.UTF8.GetBytes(plainText);
                byte[] cipher = crypt.TransformFinalBlock(plain, 0, plain.Length);
                encryptedText = Convert.ToBase64String(cipher);
            }
            return encryptedText;
        }

         * // NOT WORKING
        public static string Decrypt(string cipherText)
        {
            String plainText;
            var UTF8 = new UTF8Encoding();
            var tdes = new AesManaged();
            tdes.Key = UTF8.GetBytes(m_Key);
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            byte[] keyBytes = new byte[32];
            byte[] ivBytes = new byte[32];
            using (var crypt = tdes.CreateDecryptor())
            {
                byte[] plain = Encoding.UTF8.GetBytes(cipherText);
                byte[] cipher = crypt.TransformFinalBlock(plain, 0, plain.Length);
                plainText = Convert.ToBase64String(cipher);
            }
            return plainText;
        } 
        */
    }
}
