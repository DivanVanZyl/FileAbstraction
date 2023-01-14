using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data.DataTypes
{
    abstract internal class DirectoryItem
    {
        public string Text => _text ?? "";
        protected string? _text;
        protected string CorrectedFileName(string fileName)
        {
            if (Validation.IsWindows && fileName.IndexOfAny(Validation.InvalidWindowsChars) != -1)
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
            return fileName.Length > Validation.MaxFileNameLength ? fileName.Substring(fileName.Length - 1 - Validation.MaxFileNameLength, Validation.MaxFileNameLength) : fileName;  //Trim file if too long, avoiding file system error
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
            if (filePath.Length > Validation.MaxDirectoryLength)
            {
                FileName name = new FileName(Path.GetFileName(filePath).Substring(0, Validation.MaxDirectoryLength));
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
}