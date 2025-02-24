using System;
using System.Text;

namespace AITConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var m_Key = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

            Console.WriteLine(Encoding.ASCII.GetString(m_Key));

            var my_Key = "NorAraxNeflin";
            var my_byte = Encoding.ASCII.GetBytes(my_Key);
            Console.WriteLine(my_Key);
            Console.WriteLine(my_byte);
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
