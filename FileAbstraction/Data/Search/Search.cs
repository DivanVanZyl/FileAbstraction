using FileAbstraction.Data;
using FileAbstraction.Data.DataTypes;
using FileAbstraction.Data.Search;
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

            List<ISearch> searches = new List<ISearch>
            {
                new ForwardSearch(),
                new BackToRootSearch(),
                new AllDrivesForwardSearch()
            };

            foreach(var search in searches)
            {
                if(search.SearchDepth >= searchDepth)
                {
                    var result = search.Search(fileName, Directory.GetCurrentDirectory());
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
                    " search."), "", false);
        }
    }
}
