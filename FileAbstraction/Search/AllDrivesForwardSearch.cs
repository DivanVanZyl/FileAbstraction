using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileAbstraction.Data;

namespace FileAbstraction
{
    internal class AllDrivesForwardSearch : FileSearch
    {
        public override SearchDepth SearchDepth => SearchDepth.Deep;

        public override SearchResult<string> Search(string fileName, ref Hashtable hashtable, string startDir = "")
        {
            var drives = Validation.IsWindows ? DriveInfo.GetDrives() : new DriveInfo[] { new DriveInfo("/") };
            try
            {
                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var result = WalkDirectoryTree(new DirectoryInfo(drive.Name), fileName, hashtable);
                        if (result.IsSuccess)
                        {
                            return result;
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Full search did not find the file. Filename: " + fileName));
        }

        private SearchResult<string> WalkDirectoryTree(DirectoryInfo root, string fileName, Hashtable hashtable)
        {
            DirectoryInfo[] subDirs;

            // First, process all the files directly under this folder
            try
            {
                var result = SearchSubDirectoryForFile(root.FullName, fileName, ref hashtable);
                if (result.IsSuccess)
                {
                    return result;
                }

                subDirs = root.GetDirectories();
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    var specialFolders = Validation.IsWindows ? Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>().Select(Environment.GetFolderPath).ToList() : Validation.LinuxSystemDrives;
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, fileName, hashtable);
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
            return new SearchResult<string>(new FileNotFoundException());
        }
    }
}
