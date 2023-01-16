using FileAbstraction.Data;
using FileAbstraction.Data.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    internal class BackToRootSearch : FileSearch
    {
        public override SearchResult<string> Search(string fileName, ref Hashtable hashtable, string startDir = "")
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
                    var result = SearchSubDirectoryForFile(currentDir, fileName, ref hashtable);
                    if (result.IsSuccess)
                    {
                        return result;
                    }

                } while (currentDir != thisDrive.Name);
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Did not find the file: " + fileName));
        }
    }
}
