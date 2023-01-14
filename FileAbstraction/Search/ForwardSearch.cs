using FileAbstraction.Data.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    /// <summary>
    /// Search Forward from current directory
    /// </summary>
    internal class ForwardSearch : FileSearch
    {
        public static new SearchDepth SearchDepth => SearchDepth.Shallow;

        public override SearchResult<string> Search(string fileName, ref Hashtable hashtable, string startDir = "")
        {
            try
            {
                var subDirectories = Directory.GetDirectories(startDir, "*", SearchOption.AllDirectories);

                foreach (var subDirectory in subDirectories)
                {
                    var result = SearchSubDirectoryForFile(subDirectory, fileName, ref hashtable);
                    if(result.IsSuccess)
                    {
                        return result;
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Shallow search did not find the file. Filename: " + fileName));
        }
    }
}
