using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    abstract internal class DirectoryItem
    {
        public string Text =>  _text ?? "";
        protected string? _text;
        protected string CorrectedFileName(string fileName)
        {
            if (Validation.IsWindows && (fileName.IndexOfAny(Validation.InvalidWindowsChars) != -1))
            {
                foreach (var c in Validation.InvalidWindowsChars.Where(c => fileName.Contains(c)))
                {
                    fileName = fileName.Replace(c.ToString(), string.Empty);
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
            _text = name;
        }
    }
    internal class FilePath : DirectoryItem
    {
        public FilePath(string filePath)
        {
            if(filePath.Length > Validation.MaxDirectoryLength)
            {
                FileName name = new FileName(Path.GetFileName(filePath).Substring(0,Validation.MaxDirectoryLength));
                var dir = Path.GetDirectoryName(filePath);
                _text = dir + Path.DirectorySeparatorChar + name.Text;
            }
            else
            {
                _text = filePath;
            }            
        }
    }
    internal class FileObject : DirectoryItem
    {
        private string _fileExtensionText;
        public FileObject(string fileName)
        {
            _text = fileName;
            if (fileName.Length > 3)
            {
                Type = fileName.Substring(fileName.Length - 4, 4) == ".txt" ? FileType.Text : FileType.Binary;
            }
            else
            {
                Type = FileType.Binary;
            }
            _fileExtensionText = fileName.Contains('.') ? fileName.Substring(fileName.LastIndexOf('.')) : "";
        }
        public FileType Type { get; }
    }
    enum FileType
    {
        Text,
        Binary,
        Unknown
    }
    internal static class DataOperationExensions
    {
        public static byte[] ObjectToByteArray<T>(this T o)
        {
            if (o is null)
            {
                return Array.Empty<byte>();
            }

            return Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(o, GetJsonSerializerOptions()));
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
            var fileName = Validation.IsDirectory(directoryItem.Text)
                ? directoryItem.Text.Substring(directoryItem.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1, (directoryItem.Text.Length - 1) - (directoryItem.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1))
                : directoryItem.Text;

            var startDir = Directory.GetCurrentDirectory();

            //Search deeper
            var subDirectories = Directory.GetDirectories(startDir, "*", SearchOption.AllDirectories);

            foreach (var subDirectory in subDirectories)
            {
                foreach (var filePath in Directory.GetFiles(subDirectory))
                {
                    var name = new FileName(filePath);
                    if (name.Text == fileName)
                    {
                        return File.ReadAllText(filePath);
                    }
                }
            }

            var drives = DriveInfo.GetDrives();
            var thisDrive = drives.Single(x => x.Name == startDir.Substring(0, 3));

            //Search back
            var currentDir = startDir;
            do
            {
                currentDir = Path.GetFullPath(Path.Combine(currentDir, ".."));
                foreach (var filePath in Directory.GetFiles(currentDir))
                {
                    var name = new FileName(filePath);
                    if (name.Text == fileName)
                    {
                        return File.ReadAllText(filePath);
                    }
                }

            } while (currentDir != thisDrive.Name);


            //Search all drives
            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    var resultOnDrive = WalkDirectoryTree(new DirectoryInfo(drive.Name), fileName);
                    if (resultOnDrive.Length > 0)
                    {
                        return resultOnDrive;
                    }
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
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            catch (DirectoryNotFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to directory not found: {ex}");
            }
            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    if (fi.Name == fileName)
                    {
                        return File.ReadAllText(fi.FullName);
                    }
                }
                subDirs = root.GetDirectories();
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    var specialFolders = Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>().Select(Environment.GetFolderPath).ToList();
                    // Resursive call for each subdirectory.
                    if (!specialFolders.Contains(dirInfo.FullName))    //Skip system folders, they are large and the user file is probably not here.
                    {
                        WalkDirectoryTree(dirInfo, fileName);
                    }
                }
            }
            return "";
        }
    }

}
