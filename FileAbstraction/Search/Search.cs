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
    internal static class Search
    {
        internal static SearchResult<string> SearchRead(this DirectoryItem directoryItem, SearchDepth searchDepth)
        {
            var fileName = Validation.IsDirectory(directoryItem.Text)
                ? GetFileName(directoryItem)
                : directoryItem.Text;

            Hashtable hashtable = new Hashtable(1000000,0.7f);
            List<FileSearch> searches = new List<FileSearch>
            {
                new ForwardSearch(),
                new BackToRootSearch(),
                new AllDrivesForwardSearch()
            };

            foreach (var search in searches)
            {
                if (search.SearchDepth >= searchDepth)
                {
                    var result = search.Search(fileName, ref hashtable, Directory.GetCurrentDirectory());
                    if (result.IsSuccess)
                    {
                        return result;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(
                            result.Exception is not null ?
                            result.Exception.Message :
                            search.ToString() + " did not find the file: " + fileName);

                    }
                }
            }
            return new SearchResult<string>(
                new FileNotFoundException(
                    "File cannot be found with the " +
                    searchDepth.ToString() +
                    " search."));
        }

        private static string GetFileName(DirectoryItem fullPath)
        {
            return fullPath.Text.Substring(
                fullPath.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1,
                fullPath.Text.Length - 1 - (fullPath.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1
                ));
        }
    }
}
