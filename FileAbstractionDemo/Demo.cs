namespace FileAbstraction
{
    public class Demo
    {
        public static void Main()
        {
            //"This is some text that I wrote to a file!".ToFile();
            //1024.ToFile();
            //new Computer().ToFile();
            //new object().ToFile();

            //2048.ToFile("myNumber!");
            4096.ToFile("I l0ve base 2.numbers");
            8192.ToFile(@"..\Bas3 tw0 numb3r5.txt");

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
