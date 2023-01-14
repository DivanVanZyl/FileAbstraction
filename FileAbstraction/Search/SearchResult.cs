using System;
using System.Collections;
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
            IsSuccess = true;
            Data = data;
        }
        public SearchResult(Exception e)
        {
            IsSuccess = false;
            Exception = e;
        }
        public Exception? Exception { get; }
        public T? Data { get; }
        public bool IsSuccess { get; }
    }
}
