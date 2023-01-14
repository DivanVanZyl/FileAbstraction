using FileAbstraction.Data.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data.Search
{
    /// <summary>
    /// Search Forward from current directory
    /// </summary>
    internal class ForwardSearch : ISearch
    {
        public SearchDepth SearchDepth => SearchDepth.Shallow;

        public SearchResult<string> Search(string fileName, string startDir = "")
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
                            return new SearchResult<string>(File.ReadAllText(filePath), true);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skipping folder due to access exception: {ex}");
            }
            return new SearchResult<string>(new FileNotFoundException("Shallow search did not find the file. Filename: " + fileName), "", false);
        }
    }
}
