using FileAbstraction.Data.DataTypes;
using FileAbstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    public static class FileExtensions
    {
        private static void WriteToFile<T>(T o, FilePath path)
        {
            if (Validation.IsTextType(o))
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
