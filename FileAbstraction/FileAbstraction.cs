namespace FileAbstraction
{
    public static class ExtentionMethods
    {
        /// <summary>
        /// Year, Month, Day, Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ymdt(this DateTime dateTime)
        {
            return dateTime.ToString("20220727");
        }
        public static bool ToTextFile<T>(this T o)
        {
            var dt = new DateTime();
            File.WriteAllText(@$"D:\Default {dt.ymdt()}.txt", o is null ? "" : o.ToString());
            return true;
        }

        public static bool ToTextFile<T>(this T o, string fileName)
        {
            File.WriteAllText(@$"D:\{fileName}.txt", o is null ? "" : o.ToString());
            return true;
        }

    }
}