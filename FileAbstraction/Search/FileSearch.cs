using FileAbstraction.Data.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction
{
    internal abstract class FileSearch
    {
        public abstract SearchResult<string> Search(string fileName, ref Hashtable hashtable, string startDir = "");
        protected SearchResult<string> SearchSubDirectoryForFile(string directory, string fileName, ref Hashtable hashtable)
        {
            try
            {
                if (hashtable.ContainsKey(directory))
                {
                    return new SearchResult<string>(new Exception("Already searched this directory: " + directory));                
                }
                else
                {
                    foreach (var filePath in Directory.GetFiles(directory))
                    {
                        var name = new FileName(filePath);
                        if (name.Text == fileName)
                        {
                            return new SearchResult<string>(File.ReadAllText(filePath));
                        }
                    }
                    hashtable.Add(directory, directory.GetHashCode());
                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                new SearchResult<string>(e);
            }
            return new SearchResult<string>(new FileNotFoundException());
        }
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
