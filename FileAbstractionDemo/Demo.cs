namespace FileAbstraction
{
    public class Demo
    {
        public static void Main()
        {
            //"This is some text that I wrote to a file!".ToFile();
            new Computer().ToFile();
            FileAbstraction.DisplayFile();
        }
    }
    internal class Computer
    {
        public Computer()
        {
            Name = Environment.MachineName;
            _description = RuntimeInformation.ProcessArchitecture;
        }
        public string Name { get; set; }
        private Architecture _description;

    }
}
