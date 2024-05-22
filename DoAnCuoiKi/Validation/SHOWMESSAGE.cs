using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKi.Validation
{
    internal static class SHOWMESSAGE
    {
        public static void SUCCESS(string data)
        {
            Console.WriteLine(data);
        }
        public static void WARNING(string data)
        {
            Console.WriteLine(data);
        }
        public static void ERROR(string data)
        {
            Console.WriteLine(data);
        }
    }
}
