using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Search
{
    internal class SearchResult<T>
    {
        public SearchResult(T data, bool isSuccess)
        {
            Data = data;
            IsSuccess = isSuccess;
        }
        public SearchResult(Exception e, T data, bool isSuccess)
        {
            Exception = e;
            Data = data;
            IsSuccess = isSuccess;
        }
        public Exception? Exception { get; }
        public T Data { get; }
        public bool IsSuccess { get; }
    }
}
