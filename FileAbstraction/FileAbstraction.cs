namespace FileAbstraction
{
    public static class ExtentionMethods
    {
        /// <summary>
        /// Year, Month, Day, Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string Ymdt(this DateTime dateTime)
        {
            return DateTime.Now.ToString("yyyyMMdd HH-mm-ss-fff");
        }
        public static bool ToTextFile<T>(this T o)
        {
            var dt = new DateTime();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}Default {dt.Ymdt()}.txt", o is null ? "" : o.ToString());
            return true;
        }
        public static bool ToTextFile<T>(this T o, string fileName)
        {
            if (fileName.Length > Validation.MaxFileNameLength) { fileName = fileName.Substring(Validation.MaxFileNameLength - 1, fileName.Length - 1); }  //Trim file if too long, avoiding file system error

            var fileContents = o is null ? "" : o.ToString();
            if (fileName.Contains(Validation.SlashChar))
            {
                File.WriteAllText(@$"{fileName}.txt", fileContents);
            }
            else
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @$"{Validation.SlashChar}{fileName}.txt", fileContents);
            }
            return true;
        }
    }
    internal class Validation
    {
        public static int MaxFileNameLength => IsWindows() ? ((IsLongPathsEnabled()) ? 32767 : 255) : 255;
        public static char SlashChar => IsWindows() ? '\\' : '/';
        private static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? true : false;
        }
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