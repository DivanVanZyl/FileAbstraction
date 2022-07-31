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

            var file = new FileObject(fileName);
            return File.ReadAllText(file.FileName);
        }
        public static string ReadFile(string input)
        {
            //name or path
            if (Validation.IsDirectory(input))
            {
                var path = new FilePath(input);
                try
                {
                    return File.ReadAllText(path.FileName);
                }
                catch
                {
                    return path.SearchRead();
                }
            }
            else
            {
                var name = new FileName(input);
                var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
                var match = allFiles.FirstOrDefault(x => x.Contains(name.FileName));

                return match is null ? name.SearchRead() : File.ReadAllText(match);
            }
        }
        public static void DisplayFile() => Console.WriteLine(ReadFile());
        public static void DisplayFile(string input) => Console.WriteLine(ReadFile(input));
    }
    /// <summary>
    /// For Object => File
    /// </summary>
    public static class FileExtensions
    {
        private static void WriteToFile<T>(T o, FilePath path)
        {
            if (o is not null)
            {
                if (o.GetType() == typeof(string))
                {
                    if (path.FileName.Contains(".txt"))
                    {
                        File.WriteAllText(path.FileName, o.ToString());
                    }
                    else
                    {
                        File.WriteAllText(path.FileName + ".txt", o.ToString());
                    }
                }
                else
                {
                    File.WriteAllBytes(path.FileName, o.ObjectToByteArray());
                }
            }
        }
        public static void ToFile<T>(this T o)
        {
            FilePath filePath = new FilePath(AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}{Environment.UserName}");
            WriteToFile(o, filePath);

        }
        public static void ToFile<T>(this T o, string input)
        {
            if (Validation.IsDirectory(input))
            {
                var path = new FilePath(input);
                WriteToFile(o, path);
            }
            else
            {
                var fileName = new FileName(input);
                var path = new FilePath(AppDomain.CurrentDomain.BaseDirectory + @$"{fileName.FileName}");
                WriteToFile(o, path);
            }
        }
    }
}