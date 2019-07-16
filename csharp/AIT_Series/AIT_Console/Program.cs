using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT.Helpers;

namespace AIT_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var helloWorld = "Hello world!";
            var encryptText = aitEncryptionHelper.Encrypt(helloWorld);
            Console.WriteLine(encryptText);
            var decryptText = aitEncryptionHelper.Decrypt(encryptText);
            Console.WriteLine(decryptText);

            Console.ReadKey();
        }
    }
}
