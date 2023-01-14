using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data
{
    internal class Validation
    {
        internal static bool IsDirectory(string s) => s.Contains(Path.DirectorySeparatorChar);
        internal static int MaxFileNameLength => IsWindows ? IsLongPathsEnabled() ? 32767 : 255 : 255;
        internal static int MaxDirectoryLength => IsLinux ? 4096 : 260;
        internal static char[] InvalidWindowsChars => new char[] { '<', '>', ':', '"', '/', '|', '?', '*' };
        internal static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        internal static bool IsLinux { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        internal static List<string> LinuxSystemDrives { get; } = new List<string> { "/sys", "/proc", "/lib" };
        internal static bool IsLongPathsEnabled()
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
        internal static bool IsTextType(object o)
        {
            return o is string
                || o is string[]
                || o is char
                || o is List<string>
                || o is List<char>;
        }
    }
}
