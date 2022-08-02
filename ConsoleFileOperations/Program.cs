namespace FileAbstraction
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Count() == 2)
            {
                if (args[0] == "r")
                {
                     Console.Out.WriteLine(FileAbstraction.ReadFile(args[1]));
                }
                else if (args[0] == "d")
                {
                    Console.WriteLine(FileAbstraction.ReadFile(args[1]));
                }
                else if(args[0] == "w")
                {
                    args[1].ToFile();
                }
                else
                {
                    Console.Error.WriteLine("Only 'r' for read and 'w' for write, are valid operations.");
                }
            }
            else
            {
                Console.Error.WriteLine(@"Syntax: f [r/w] [""file name""]");
            }
        }
    }
}