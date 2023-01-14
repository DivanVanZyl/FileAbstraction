using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal static class Text
    {
        public static void PrintLineSeperator(char seperator = '*')
        {
            foreach(var i in Enumerable.Range(0, Console.WindowWidth))
                Console.Write(seperator);
            Console.Write(Environment.NewLine);
        }
    }
}
