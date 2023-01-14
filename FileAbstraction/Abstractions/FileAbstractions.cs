using FileAbstraction.Data;
using FileAbstraction.Data.DataTypes;
using static FileAbstraction.Search;

namespace FileAbstraction
{
    public static class FileAbstract
    {
        public static T ReadBinFile<T>() where T : new()
        {
            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            var latestFile = allFiles.Max(File.GetLastWriteTime);
            var path = new FilePath(allFiles.Single(x => File.GetLastWriteTime(x) == latestFile));

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
            var fileName = allFiles.Single(x => File.GetLastWriteTime(x) == latestFile);

            var file = new FileObject(fileName);
            return File.ReadAllText(file.Text);
        }
        public static string ReadFile(string input, SearchDepth searchDepth = SearchDepth.Shallow)
        {
            //Either name or path are acceptable inputs.
            if (Validation.IsDirectory(input))
            {
                var path = new FilePath(input);
                try
                {
                    return File.ReadAllText(path.Text);
                }
                catch
                {
                    return path.SearchRead(searchDepth).Data;

                }
            }
            else
            {
                var name = new FileName(input);
                var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
                var match = allFiles.FirstOrDefault(x => x.Contains(name.Text));

                return match is not null ? File.ReadAllText(match) : name.SearchRead(searchDepth).Data;
            }
        }
        public static void DisplayFile() => Console.WriteLine(ReadFile());
        public static void DisplayFile(string input) => Console.WriteLine(ReadFile(input));
    }    
}