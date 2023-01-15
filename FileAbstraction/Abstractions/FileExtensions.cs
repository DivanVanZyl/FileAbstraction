using FileAbstraction.Data.DataTypes;
using FileAbstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileAbstraction
{
    public static class FileExtensions
    {
        public static void ToBinFile(this byte[] bytes, string path = "")
        {
            if (path == "")
            {
                File.WriteAllBytes(
                    new FilePath(AppDomain.CurrentDomain.BaseDirectory + $"{Environment.UserName}").Text,
                    bytes);
            }
            else
            {
                File.WriteAllBytes(
                    new FilePath(AppDomain.CurrentDomain.BaseDirectory + $"{new FileName(path).Text}").Text,
                    bytes);
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
        private static void WriteToFile<T>(T o, FilePath path)
        {
            if (Validation.IsTextType(o))
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
                File.WriteAllBytes(path.Text, o.ObjectToByteArray());
            }
        }
    }
}
