using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Cecil2
{
    class Program
    {
        static void Main(string[] args)
        {
            TestType tt = new TestType();
            tt.SayHello();
            tt.AboutMe();
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
