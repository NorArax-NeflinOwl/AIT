using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.Models;
using System.Reflection;
using System.Threading.Tasks;
using AIT_Lib.FilesManagers;
using AIT_Lib.Interfaces;
using AIT_Lib.Helpers;

namespace Console_Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //SerializeTest();

            //A.Execute();

            testContains();
            Console.ReadKey();
        }

        /// <summary>
        /// Example show you that you can'nt use protected method even by reflection
        /// </summary>
        private static void ErrorPermitionTestReflection()
        {
            var node = new aitNodeNoteModel();
            Console.WriteLine(node.ID);

            var type = node.GetType();
            //Application show error because method SetID is protected
            MethodInfo method = type.GetMethod("SetID");
            method.Invoke(node, new object[] { "1000" });
            Console.WriteLine(node.ID);
        }

        private static void SerializeTest()
        {
            var note = new aitSimpleNoteModel();
            aitCryptJsonManager.Instance.SerializeObjectToFile(note);

        }

        private static List<Object> testList;

        private static void testContains()
        {
            testList = new List<object>();
            testList.Add(new Object());
            if (testList.Contains(null))
                Console.WriteLine("YES");
        }
    }

    public class A
    {
        private int id;
        public A(int id)
        {
            this.id = id;
        }

        public static void Execute()
        {
            foreach(A a in B.Alist)
            {
                if (a.id == 3)
                    Console.WriteLine("IT'S FUCKING WORKING!!!");
            }
        }
    }

    public class B
    {
        public static List<A> Alist = new List<A>
            {
                new A(10),
                new A(3),
                new A(-21)
            };
    }
}
