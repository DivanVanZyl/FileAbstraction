using FileAbstraction;

namespace Demo
{
    public class Demo
    {
        public static void Main()
        {
            //Simple operation with easy syntax. This can be done with any object.
            4096.ToFile();
            FileAbstract.DisplayFile();

            //Library will search for this file, as the location is not given.
            8192.ToFile(@$"..{Path.DirectorySeparatorChar}Bas3 tw0 numb3r5.txt");
            FileAbstract.DisplayFile("Bas3 tw0 numb3r5.txt");

            //A more complex search
            "This was found in my user folder".ToFile( Path.Combine(@"C:","Users",$"{ Environment.UserName}", "Downloads", "myFile.txt"));
            FileAbstract.DisplayFile("myFile.txt");

            //An even more complex search, here the caller supplied an incorrect path. (Drive does not exist)
            "You found me, event though an incorrect path was given!"
                .ToFile(Path.Combine("X:","I hide here.txt"));
            FileAbstract.DisplayFile("I hide here.txt");

            //Also a search here. In this case, the file the caller wants, is on another drive (than what the app is running on).
            //NOTE: This will only work, if you computer has another drive.
            "You found me on another drive!".ToFile(Path.Combine("E:","And I hide here.txt"));
            FileAbstract.DisplayFile("And I hide here.txt");

            //Serialization/De-Serialization of objects.
            var c = new Computer();
            c.ToFile();
            var savedC = FileAbstract.ReadBinFile<Computer>();
            Console.WriteLine(savedC.Name);            
        }
    }
    internal class Computer
    {
        public Computer()
        {
            Name = Environment.MachineName;
            _description = RuntimeInformation.ProcessArchitecture;
        }
        public string Name { get; }
        private Architecture _description;

    }
}
