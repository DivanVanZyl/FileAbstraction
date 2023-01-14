using FileAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstractionTests
{
    public class BinaryFiles
    {
        [Fact]
        public void WriteObjectAsBinary()
        {
            try
            {
                256.ToFile();
            }
            catch
            {
                Assert.Fail("Writing object as binary to a file threw an exception.");
            }
        }
    }
}
