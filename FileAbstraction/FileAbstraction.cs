namespace FileAbstraction
{
    public static class FileAbstraction
    {
        public static string ReadFile()
        {
            //Get all files in dir
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            //Read latest one
            var latest = files.Max(x => File.GetLastWriteTime(x));
            var fileName = files.Where(x => (File.GetLastWriteTime(x) == latest)).Single();
            return File.ReadAllText(fileName);
        }
        public static void DisplayFile() => Console.WriteLine(ReadFile());
    }
    public static class ExtensionMethods
    {
        public static bool ToTextFile<T>(this T o)
        {
            var dt = new DateTime();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}Default {dt.Ymdt()}.txt", o is null ? "" : o.ToString());
            return true;
        }
        public static bool ToTextFile<T>(this T o, string fileName)
        {
            if (Validation.IsWindows() && (fileName.IndexOfAny(Validation.InvalidWindowsChars) != -1))
            {
                foreach (char c in fileName)
                {
                    if (Validation.InvalidWindowsChars.Contains(c)) { fileName.Remove(c); }
                }
            }
            var fileContents = o is null ? "" : o.ToString();
            if (Validation.IsDirectory(fileName))
            {
                var slashIndexes = Enumerable.Range(0, fileName.Length)
                                            .Where(x => x == Validation.SlashChar)
                                            .ToList();
                while (fileName.Length > Validation.MaxDirectoryLength) //Go one dir shallower, untill size is valid
                {
                    var lastSlashIndex = slashIndexes[slashIndexes.Count - 1];
                    var secondLastSlashIndex = slashIndexes[slashIndexes.Count - 2];
                    fileName = fileName.Substring(0, secondLastSlashIndex) + fileName.Substring(lastSlashIndex + 1, fileName.Length - 1);
                }
                File.WriteAllText(@$"{fileName}.txt", fileContents);
            }
            else
            {
                if (fileName.Length > Validation.MaxFileNameLength) { fileName = fileName.Substring(Validation.MaxFileNameLength - 1, fileName.Length - 1); }  //Trim file if too long, avoiding file system error
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}{fileName}.txt", fileContents);
            }
            return true;
        }        
        // Year, Month, Day, Time
        private static string Ymdt(this DateTime dateTime)
        {
            return DateTime.Now.ToString("yyyyMMdd HH-mm-ss-fff");
        }
    }
    internal class Validation
    {
        public static bool IsDirectory(string s) => s.Contains(SlashChar);
        public static int MaxFileNameLength => IsWindows() ? ((IsLongPathsEnabled()) ? 32767 : 255) : 255;
        public static int MaxDirectoryLength => IsLinux() ? 4096 : 260;
        public static char SlashChar => IsWindows() ? '\\' : '/';
        public static char[] InvalidWindowsChars => new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private static bool IsLongPathsEnabled()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\FileSystem");
            if (key is null)
            {
                return false;
            }

            var regVal = key.GetValue("LongPathsEnabled");
            if (regVal is null)
            {
                return false;
            }

            return (int)regVal > 0;
        }
    }
}