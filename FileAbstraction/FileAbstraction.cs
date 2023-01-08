namespace FileAbstraction
{
    /// <summary>
    /// For File => Object
    /// </summary>
    public static class FileAbstract
    {
        public static T ReadBinFile<T>() where T : new()
        {
            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            var latestFile = allFiles.Max(File.GetLastWriteTime);
            var path = new FilePath(allFiles.Single(x => (File.GetLastWriteTime(x) == latestFile)));

            try
            {
                string fileContents;
                using (TextReader reader = new StreamReader(path.Text))
                {
                    fileContents = reader.ReadToEnd();
                }

                var payload = JsonConvert.DeserializeObject<T>(fileContents);
                return payload ?? new T();
            }
            catch
            {
                return new T();
            }
        }
        public static string ReadFile()
        {
            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            var latestFile = allFiles.Max(File.GetLastWriteTime);
            var fileName = allFiles.Single(x => (File.GetLastWriteTime(x) == latestFile));

            var file = new FileObject(fileName);
            return File.ReadAllText(file.Text);
        }
        public static string ReadFile(string input)
        {
            //name or path
            if (Validation.IsDirectory(input))
            {
                var path = new FilePath(input);
                try
                {
                    return File.ReadAllText(path.Text);
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
                var match = allFiles.FirstOrDefault(x => x.Contains(name.Text));

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
            if (o is string)
            {
                if (path.Text.Contains(".txt"))
                {
                    try
                    {
                        File.WriteAllText(path.Text, o.ToString());
                    }
                    catch (DirectoryNotFoundException)
                    {
                        File.WriteAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + Path.GetFileName(path.Text), o.ToString());
                    }
                }
                else
                {
                    File.WriteAllText(path.Text + ".txt", o.ToString());
                }
            }
            else
            {
                File.WriteAllBytes(path.Text, o.ObjectToByteArray());
            }
        }
        public static void ToFile<T>(this T o)
        {
            FilePath filePath = new FilePath(AppDomain.CurrentDomain.BaseDirectory + $"{Environment.UserName}");
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
                var path = new FilePath(AppDomain.CurrentDomain.BaseDirectory + $"{fileName.Text}");
                WriteToFile(o, path);
            }
        }
    }
}