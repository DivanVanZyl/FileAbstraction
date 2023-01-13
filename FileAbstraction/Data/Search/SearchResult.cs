using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    internal class SearchResult<T>
    {
        public SearchResult(T data)
        {
            Data = data;
        }
        public SearchResult(Exception e, T data)
        {
            Exception = e;
            Data = data;
        }
        public Exception? Exception { get; }
        public T Data { get; }
    }
}
