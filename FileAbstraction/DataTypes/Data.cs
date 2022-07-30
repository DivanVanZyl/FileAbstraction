using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    internal static class DataOperationExensions
    {
        public static byte[] ObjectToByteArray<T>(this T o)
        {
            if (o == null)
                return new byte[] { };

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(o, GetJsonSerializerOptions()));
        }
        public static T? ByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null || !byteArray.Any())
                return default;

            return JsonSerializer.Deserialize<T>(byteArray, GetJsonSerializerOptions());
        }
        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
    }
    abstract internal class DirectoryItem
    {
        protected string? _fileName;
        protected string CorrectedFileName(string fileName)
        {

            if (Validation.IsWindows() && (fileName.IndexOfAny(Validation.InvalidWindowsChars) != -1))
            {
                foreach (char c in fileName)
                {
                    if (Validation.InvalidWindowsChars.Contains(c)) { fileName.Replace(c.ToString(),""); }
                }
            }
            return fileName;
        }

        public string FileName => _fileName is null ? "" : _fileName;
    }
    internal class FileName : DirectoryItem
    {        
        public FileName(string name)
        {
            name = CorrectedFileName(name);
            if (name.Length > Validation.MaxFileNameLength) { name = name.Substring(Validation.MaxFileNameLength - 1, name.Length - 1); }  //Trim file if too long, avoiding file system error
            _fileName = name;
        }
    }
    internal class FilePath : DirectoryItem
    {
        public FilePath(string filePath)
        {
            filePath = CorrectedFileName(filePath);
            var slashIndexes = Enumerable.Range(0, filePath.Length)
                                            .Where(x => x == Validation.SlashChar)
                                            .ToList();
            while (filePath.Length > Validation.MaxDirectoryLength) //Go one dir shallower, untill size is valid
            {
                var lastSlashIndex = slashIndexes[slashIndexes.Count - 1];
                var secondLastSlashIndex = slashIndexes[slashIndexes.Count - 2];
                filePath = filePath.Substring(0, secondLastSlashIndex) + filePath.Substring(lastSlashIndex + 1, filePath.Length - 1);
            }
            _fileName = filePath;
        }
    }
}
