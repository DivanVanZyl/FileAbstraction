namespace FileAbstraction
{
    /// <summary>
    /// For File => Object
    /// </summary>
    public static class FileAbstraction
    {
        public static string ReadFile()
        {            
            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            var latestFile = allFiles.Max(x => File.GetLastWriteTime(x));
            var fileName = allFiles.Where(x => (File.GetLastWriteTime(x) == latestFile)).Single();
            return File.ReadAllText(fileName);
        }
        public static void DisplayFile() => Console.WriteLine(ReadFile());
    }
    /// <summary>
    /// For Object => File
    /// </summary>
    public static class FileExtensions
    {
        public static bool ToFile<T>(this T o)
        {
            var fileName = AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}{Environment.UserName}";

            if (o is not null)
            {
                if (o.GetType() == typeof(string))
                {
                    File.WriteAllText(fileName + ".txt", o.ToString());
                }
                else
                {
                    File.WriteAllBytes(fileName, o.ObjectToByteArray());
                }
            }
            return true;
        }
        public static bool ToFile<T>(this T genericData, string input)
        {            

            var fileContents = genericData is null ? "" : genericData.ToString();


            if (Validation.IsDirectory(input))
            {
                var path = new FilePath(input);
                File.WriteAllText(@$"{path.FileName}.txt", fileContents);
            }
            else
            {
                var name = new FileName(input);                
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}{name.FileName}.txt", fileContents);
            }
            return true;
        }
    }   
}