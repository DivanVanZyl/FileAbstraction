using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data.Search
{
    internal class AllDrivesForwardSearch : ISearch
    {
        public SearchDepth SearchDepth => SearchDepth.Full;

        public SearchResult<string> Search(string fileName, string startDir = "")
        {
            var drives = Validation.IsWindows ? DriveInfo.GetDrives() : new DriveInfo[] { new DriveInfo("/") };
            try
            {
                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var resultOnDrive = WalkDirectoryTree(new DirectoryInfo(drive.Name), fileName);
                        if (resultOnDrive.Length > 0)
                        {
                            return new SearchResult<string>(resultOnDrive, true);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Full search did not find the file. Filename: " + fileName), "", false);
        }

        private static string WalkDirectoryTree(DirectoryInfo root, string fileName)
        {
            FileInfo[] files;
            DirectoryInfo[] subDirs;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
                foreach (FileInfo fi in files)
                {
                    if (fi.Name == fileName)
                    {
                        return File.ReadAllText(fi.FullName);
                    }
                }
                subDirs = root.GetDirectories();
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    var specialFolders = Validation.IsWindows ? Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>().Select(Environment.GetFolderPath).ToList() : Validation.LinuxSystemDrives;
                    // Resursive call for each subdirectory.
                    if (!specialFolders.Contains(dirInfo.FullName))    //Skip system folders, they are large and the user file is probably not here.
                    {
                        WalkDirectoryTree(dirInfo, fileName);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            catch (DirectoryNotFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to directory not found: {ex}");
            }
            return "";
        }
    }
}
