using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    abstract internal class DirectoryItem
    {
        public string FileName => _fileName is null ? "" : _fileName;
        protected string? _fileName;
        protected string CorrectedFileName(string fileName)
        {
            if (Validation.IsWindows() && (fileName.IndexOfAny(Validation.InvalidWindowsChars) != -1))
            {
                foreach (char c in fileName)
                {
                    if (Validation.InvalidWindowsChars.Contains(c)) { fileName.Replace(c.ToString(), ""); }
                }
            }
            return fileName;
        }
        protected string TrimmedFileName(string fileName)
        {
            return fileName.Length > Validation.MaxFileNameLength ? fileName.Substring((fileName.Length - 1) - Validation.MaxFileNameLength, Validation.MaxFileNameLength) : fileName;  //Trim file if too long, avoiding file system error
        }
    }
    internal class FileName : DirectoryItem
    {
        public FileName(string name)
        {
            name = Validation.IsDirectory(name) ? Path.GetFileName(name) : name;
            name = CorrectedFileName(name);
            name = TrimmedFileName(name);
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
                filePath = filePath.Substring(0, secondLastSlashIndex) + filePath.Substring(lastSlashIndex + 1, (filePath.Length - 1) - (lastSlashIndex + 1));
            }
            _fileName = filePath;
        }
    }
    internal class FileObject : DirectoryItem
    {
        private string _fileExtensionText;
        private FileType _type;
        public FileObject(string fileName)
        {
            _fileName = fileName;
            if (fileName.Length > 3)
            {
                _type = fileName.Substring(fileName.Length - 4, 4) == ".txt" ? FileType.text : FileType.binary;
            }
            else
            {
                _type = FileType.binary;
            }
            _fileExtensionText = fileName.Contains('.') ? fileName.Substring(fileName.LastIndexOf('.')) : "";
        }
        public FileType Type => _type;
    }
    enum FileType
    {
        text,
        binary
    }
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
        internal static string SearchRead(this DirectoryItem directoryItem)
        {
            var fileName = Validation.IsDirectory(directoryItem.FileName)
                ? directoryItem.FileName.Substring(directoryItem.FileName.LastIndexOf(Validation.SlashChar) + 1, (directoryItem.FileName.Length - 1) - (directoryItem.FileName.LastIndexOf(Validation.SlashChar) + 1))
                : directoryItem.FileName;

            var startDir = Directory.GetCurrentDirectory();

            //Search deeper
            var subDirectories = Directory.GetDirectories(startDir, "*", SearchOption.AllDirectories);

            foreach (var subDirectory in subDirectories)
            {
                foreach (var filePath in Directory.GetFiles(subDirectory))
                {
                    var name = new FileName(filePath);
                    if (name.FileName == fileName) return File.ReadAllText(filePath);
                }
            }

            var drives = DriveInfo.GetDrives();
            var thisDrive = drives.Where(x => x.Name == startDir.Substring(0, 3)).Single();

            //Search back
            var currentDir = startDir;
            do
            {
                currentDir = Path.GetFullPath(Path.Combine(currentDir, ".."));
                foreach (var filePath in Directory.GetFiles(currentDir))
                {
                    var name = new FileName(filePath);
                    if (name.FileName == fileName) return File.ReadAllText(filePath);
                }

            } while (currentDir != thisDrive.Name);


            //Search all drives
            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    var resultOnDrive = WalkDirectoryTree(new DirectoryInfo(drive.Name), fileName);
                    if (resultOnDrive.Length > 0)
                        return resultOnDrive;
                }                
            }
            return "";  //File not found
        }
        private static string WalkDirectoryTree(System.IO.DirectoryInfo root, string fileName)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException)
            {
            }

            catch (System.IO.DirectoryNotFoundException)
            {
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    if (fi.Name == fileName) return File.ReadAllText(fi.FullName);
                }

                subDirs = root.GetDirectories();
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    if(! (dirInfo.Name == "Windows" || dirInfo.Name == "Program Files(x86)" || dirInfo.Name == "Program Files"))    //Skip system folders, they are large and the user file is probably not here.
                    {
                        WalkDirectoryTree(dirInfo, fileName);                        
                    }
                }
            }
            return "";
        }
    }

}
