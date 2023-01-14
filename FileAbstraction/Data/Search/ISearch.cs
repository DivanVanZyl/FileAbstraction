using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data
{
    internal interface ISearch
    {
        SearchResult<string> Search(string fileName, string startDir = "");
        SearchDepth SearchDepth { get; }
    }

    public enum SearchDepth
    {
        None = 0,
        Shallow = 1,
        Deep = 2,
        Full = 3
    }
}
