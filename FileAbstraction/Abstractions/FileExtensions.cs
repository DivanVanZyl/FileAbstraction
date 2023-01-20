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
        public static void ToFile<T>(this T o, string inputPath = "")
        {
            FilePath path = new FilePath(inputPath);
            if (path.Text == "")
            {
                path = new FilePath(AppDomain.CurrentDomain.BaseDirectory + $"{Environment.UserName}");
            }
            else
            {
                if (!Validation.IsDirectory(path.Text))
                {
                    path = new FilePath(AppDomain.CurrentDomain.BaseDirectory + $"{new FileName(path.Text).Text}");
                }
            }

            if (o is null)
            {
                File.WriteAllBytes(path.Text, new byte[0]);
            }
            else if (Validation.IsTextType(o))
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
