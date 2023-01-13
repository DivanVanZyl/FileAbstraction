using FileAbstraction.Data;
using FileAbstraction.Data.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    internal static class Search
    {
        internal static SearchResult<string> SearchRead(this DirectoryItem directoryItem, SearchDepth searchDepth)
        {
            var fileName = Validation.IsDirectory(directoryItem.Text)
                ? directoryItem.Text.Substring(directoryItem.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1, directoryItem.Text.Length - 1 - (directoryItem.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1))
                : directoryItem.Text;


            //Search deeper
            var startDir = Directory.GetCurrentDirectory();
            if (searchDepth >= SearchDepth.Shallow)
            {
                var result = ShallowSearch(fileName, startDir);
                if (result.Exception is null)
                {
                    return result;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(result.Exception.Message);
                }
            }

            if (searchDepth >= SearchDepth.Deep)
            {
                var result = DeepSearch(fileName, startDir);
                if (result.Exception is null)
                {
                    return result;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(result.Exception.Message);
                }
            }

            //Search all drives
            if (searchDepth == SearchDepth.Full)
            {
                var result = FullSearch(fileName);
                if (result.Exception is null)
                {
                    return result;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(result.Exception.Message);
                }
            }
            return new SearchResult<string>(
                new FileNotFoundException("File cannot be found with the " + searchDepth.ToString() + " search."), "");
        }

        private static SearchResult<string> ShallowSearch(string fileName, string startDir)
        {
            try
            {
                var subDirectories = Directory.GetDirectories(startDir, "*", SearchOption.AllDirectories);

                foreach (var subDirectory in subDirectories)
                {
                    foreach (var filePath in Directory.GetFiles(subDirectory))
                    {
                        var name = new FileName(filePath);
                        if (name.Text == fileName)
                        {
                            return new SearchResult<string>(File.ReadAllText(filePath));
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Shallow search did not find the file. Filename: " + fileName), "");
        }

        private static SearchResult<string> DeepSearch(string fileName, string startDir)
        {
            var rootPath = Path.GetPathRoot(startDir);
            if (rootPath is null)
            {
                if (Validation.IsWindows)
                {
                    rootPath = @"C:\";
                }
                else
                {
                    rootPath = @"/";
                }
            }
            var thisDrive = new DriveInfo(rootPath);
            //Search back
            try
            {
                var currentDir = startDir;
                do
                {
                    currentDir = Path.GetFullPath(Path.Combine(currentDir, ".."));
                    foreach (var filePath in Directory.GetFiles(currentDir))
                    {
                        var name = new FileName(filePath);
                        if (name.Text == fileName)
                        {
                            return new SearchResult<string>(File.ReadAllText(filePath));
                        }
                    }

                } while (currentDir != thisDrive.Name);
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Deep search did not find the file. Filename: " + fileName), "");
        }

        private static SearchResult<string> FullSearch(string fileName)
        {
            var drives = Validation.IsWindows ? DriveInfo.GetDrives() : new DriveInfo[] { new DriveInfo("/") };
            try
            {
                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var resultOnDrive = Search.WalkDirectoryTree(new DirectoryInfo(drive.Name), fileName);
                        if (resultOnDrive.Length > 0)
                        {
                            return new SearchResult<string>(resultOnDrive);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Full search did not find the file. Filename: " + fileName), "");
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
    public enum SearchDepth
    {
        None = 0,
        Shallow = 1,
        Deep = 2,
        Full = 3
    }
}
