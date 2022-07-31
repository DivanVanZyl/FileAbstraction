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
            FileAbstraction.DisplayFile();


            8192.ToFile(@"..\Bas3 tw0 numb3r5.txt");
            FileAbstraction.DisplayFile("Bas3 tw0 numb3r5.txt");

            "You found me!".ToFile("X:\\I hide here.txt");
            FileAbstraction.DisplayFile("I hide here.txt");

            "You found me too!".ToFile(@"C:\AAAFolder1\And I hide here.txt");
            FileAbstraction.DisplayFile("And I hide here.txt");
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
