using FileAbstraction.Data;
using FileAbstraction.Data.DataTypes;
using System.IO;
using System.Text;

namespace FileAbstraction
{
    public static class FileAbstract
    {
        public static T? ReadBinFile<T>(string path = "")
        {
            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            var latestFile = allFiles.Max(File.GetLastWriteTime);
            if(path == "")
            {
                path = new FilePath(allFiles.First(x => File.GetLastWriteTime(x) == latestFile)).Text;
            }

            string fileContents;
            using (TextReader reader = new StreamReader(path))
            {
                fileContents = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(fileContents);
        }
        public static byte[] ReadBinFile(string path = "")
        {
            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            var latestFile = allFiles.Max(File.GetLastWriteTime);
            if (path == "")
            {
                path = new FilePath(allFiles.First(x => File.GetLastWriteTime(x) == latestFile)).Text;
            }

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    using (var ms = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch
            {
                return new byte[0];
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
        public static string ReadFile(string input)
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
                    var data = path.SearchRead().Data;
                    return data is null ? "" : data;

                }
            }
            else
            {
                var name = new FileName(input);
                var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
                var match = allFiles.FirstOrDefault(x => x.Contains(name.Text));

                var data = match is not null ? File.ReadAllText(match) : name.SearchRead().Data;
                return data is null ? "" : data;
            }
        }
        public static void DisplayFile() => Console.WriteLine(ReadFile());
        public static void DisplayFile(string input) => Console.WriteLine(ReadFile(input));
    }
}