using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfuserCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>();
            var clist = new List<string>();
            Console.WriteLine("Hello World!");
            list = FileOperater.GetClassFileNames("../../../");
            foreach (var item in list)
            {
                var program = File.ReadAllLines("../../../"+item);
                clist = ObfusecateSupporter.GetClassName(program);
                //Console.WriteLine(ObfusecateSupporter.GetNamespace(program));
            }
            //a = 12354
            //i = 12356
            //23621
            //char ind = (char)23621;
            //char[] c = new char[] { (char)12354, (char)12356, (char)23621 };
            //string v = new string(c);
            //Console.WriteLine(v);
            //foreach (var item in list)
            {
               // var l = new List<string>();
                //var d = new Dictionary<string, string>();
                //var program = File.ReadAllLines("../../../" + item);
                //l = ObfusecateSupporter.GetIdentifer(program, clist);
                //d = ObfusecateSupporter.GetObfuseCatedIdentifierDictionary(l);
                //FileOperater.WriteObfusecateCodes("FileOperater.cs", d);
            }
            var l = new List<string>();
            var d = new Dictionary<string, string>();
            var pro = File.ReadAllLines("../../../" + "FileOperater.cs");
            l = ObfusecateSupporter.GetIdentifer(pro, clist);
            d = ObfusecateSupporter.GetObfuseCatedIdentifierDictionary(l);
            foreach (var item in d)
            {
                Console.WriteLine(item.Key + "         " + item.Value);
            }
            FileOperater.WriteObfusecateCodes("FileOperater.cs", d);

        }
    }
}
