using Demo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal static class TextDemos
    {
        public static void Simple()
        {
            //Simple operation with easy syntax. This can be done with any object.
            Console.WriteLine("Object to text and reading that file from current path:");
            4096.ToFile();
            FileAbstract.DisplayFile();
            Text.PrintLineSeperator();
        }

        public static void SearchShallow()
        {
            //Library will search for this file, as the location is not given.
            Console.WriteLine("Search for a file, one directory back:");
            8192.ToFile(@$"..{Path.DirectorySeparatorChar}Bas3 tw0 numb3r5.txt");
            FileAbstract.DisplayFile("Bas3 tw0 numb3r5.txt",SearchDepth.Deep);
            Text.PrintLineSeperator();
        }

        public static void SearchDeep()
        {
            //A more complex search
            var dir = Path.Combine(@"C:", "Users", $"{Environment.UserName}", "Downloads", "myFile.txt");
            Console.WriteLine($"Search for a file in the user directory ({dir}):");
            "This was found in my user folder".ToFile();
            FileAbstract.DisplayFile("myFile.txt");
            Text.PrintLineSeperator();
        }

        public static void SearchIncorrectParam()
        {
            //An even more complex search, here the caller supplied an incorrect path (Drive does not exist). The library will try to find the file, even if the param is incorrect.
            Console.WriteLine("Search for a file, where a path param was supplied, but is incorrent:");
            "You found me, event though an incorrect path was given!"
                .ToFile(Path.Combine("X:", "I hide here.txt"));
            FileAbstract.DisplayFile("I hide here.txt");
            Text.PrintLineSeperator();
        }

        public static void SearchFull()
        {
            //Also a search here. In this case, the file the caller wants, is on another drive (than what the app is running on).
            //NOTE: This will only work, if you computer has another drive.
            Console.WriteLine("Search for a file, who's name is supplied, but not the path, and the file is on another drive:");
            "You found me on another drive!".ToFile(Path.Combine("E:", "And I hide here.txt"));
            FileAbstract.DisplayFile("And I hide here.txt",SearchDepth.Full);
            Text.PrintLineSeperator();
        }
    }
    internal static class BinaryDemos
    {
        public static void Simple()
        {
            //Serialization/De-Serialization of objects.
            Console.WriteLine("Serialize object to bin file, then deserialize back to another object of same type:");
            var c = new Computer();
            c.ToFile();
            var savedC = FileAbstract.ReadBinFile<Computer>();
            Console.WriteLine(savedC.Name);
            Text.PrintLineSeperator();
        }
    }
}
