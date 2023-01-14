using FileAbstraction.Data.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data.Search
{
    internal class BackToRootSearch : ISearch
    {
        public SearchDepth SearchDepth => SearchDepth.Deep;

        public SearchResult<string> Search(string fileName, string startDir = "")
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
            //Search back to the root of the starting directory.
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
                            return new SearchResult<string>(File.ReadAllText(filePath), true);
                        }
                    }

                } while (currentDir != thisDrive.Name);
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Did not find the file: " + fileName), "", false);
        }
    }
}
