namespace FileAbstraction
{
    public class Demo
    {
        public static void Main()
        {            
            4096.ToFile("I l0ve base 2.numbers");
            FileAbstraction.DisplayFile();

            8192.ToFile(@"..\Bas3 tw0 numb3r5.txt");
            FileAbstraction.DisplayFile("Bas3 tw0 numb3r5.txt");

            "This was found in my user folder".ToFile(@$"C: \Users\{Environment.UserName}\Downloads\\"+ "myFile.txt");
            FileAbstraction.DisplayFile("myFile.txt");

            "You found me!".ToFile("X:\\I hide here.txt");
            FileAbstraction.DisplayFile("I hide here.txt");

            "You found me too!".ToFile(@"C:\And I hide here.txt");
            FileAbstraction.DisplayFile("And I hide here.txt");

            var c = new Computer();
            c.ToFile();
            var savedC = FileAbstraction.ReadFile();
            Console.WriteLine(savedC.);
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
