﻿using FileAbstraction.Data.DataTypes;
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
        public abstract SearchDepth SearchDepth { get; }
        public abstract SearchResult<string> Search(string fileName, ref Hashtable hashtable, string startDir = "");
        public override string ToString()
        {
            return this.GetType().Name;
        }
        protected SearchResult<string> SearchSubDirectoryForFile(string directory, string fileName, ref Hashtable hashtable)
        {
            if (!hashtable.ContainsKey(directory))
            {
                foreach (var filePath in Directory.GetFiles(directory))
                {
                    var name = new FileName(filePath);
                    if (name.Text == fileName)
                    {
                        return new SearchResult<string>(File.ReadAllText(filePath));
                    }
                }
            }
            hashtable.Add(directory, directory.GetHashCode());
            return new SearchResult<string>(new FileNotFoundException());
        }
    }

    public enum SearchDepth
    {
        None = 0,
        Shallow = 1,
        Deep = 2,
        Full = 3
    }
}
