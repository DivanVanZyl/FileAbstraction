namespace FileAbstraction
{
    class Program
    {
        private static readonly string _helpText = @"This is a file helper command.
Usage:
f [r] [""file name""]
f [w] [""contents""]
f [w] [""contents""] [""file name""]

Operation Options:
-r      read text
-w      write text

Source Target Options:
You may either have one parameter, which will be data to write with a default name, or read from a default name.
Or, you may specify both, with the first parameter being the data, and the second the file name to write to or read from." + Environment.NewLine;
        static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                if (args[0].ToLower() == "h" || args[0].ToLower() == "help")
                {
                    Console.Write(_helpText);
                }
                else
                {
                    Console.WriteLine("'" + args[0] + "'" + " is not a valid command.");
                    Console.Write(_helpText);
                }
            }
            else if (args.Count() == 2)
            {
                if (args[0].ToLower() == "r")
                {
                    Console.Out.WriteLine(FileAbstract.ReadFile(args[1]));
                }
                else if (args[0].ToLower() == "w")
                {
                    args[1].ToFile();
                }
                else
                {
                    Console.Error.WriteLine("Only 'r/rb' for read and 'w/wb' for write, are valid operations.");
                    Console.Write(_helpText);
                }
            }
            else if (args.Count() == 3)
            {
                if (args[0].ToLower() == "r")
                {
                    Console.Out.WriteLine(FileAbstract.ReadFile(args[2]));
                }
                else if (args[0].ToLower() == "w")
                {
                    args[1].ToFile(args[2]);
                }
                else
                {
                    Console.Error.WriteLine("Only 'r' for read and 'w' for write, are valid operations.");
                    Console.Write(_helpText);
                }
            }
            else
            {
                Console.Write(_helpText);
            }
        }
    }
}