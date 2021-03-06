using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Cecil2
{
    class Program
    {
        static void Main(string[] args)
        {
            TestType tt = new TestType();
            tt.SayHello();
            tt.AboutMe();

            string path= Directory.GetCurrentDirectory();
            DFLog.Log(path);

            Console.ReadKey();
        }
    }

    public class TestType
    {
        [Obsolete]
        public void SayHello()
        {
            Console.WriteLine("Hello Cecil");
        }

        public static void StaticSayHello()
        {

        }

        public void AboutMe()
        {
            Type type = typeof(TestType);
            MethodInfo method = type.GetMethod("SayHello");
            if(method.IsVirtual)
            {
                Console.WriteLine("Virtual");
            }
            else
            {
                Console.WriteLine("Non Virtual");
            }

            object[] attrs= method.GetCustomAttributes(false);
            if(attrs!=null && attrs.Length>0)
            {
                Console.WriteLine("attrs : ");
                foreach(var v in attrs)
                {
                    Console.WriteLine(v.GetType().Name);
                }
            }
        }
    }
}
