using FileAbstraction;

namespace FileAbstractionTests
{
    public class TextFileTests
    {
        [Fact]
        public void WriteObjectAsText()
        {
            try
            {
                "256".ToFile();
            }
            catch
            {
                Assert.Fail("Writing object as text to a file threw an exception.");
            }
        }
        [Fact]
        public void ReadFileAsText()
        {
            const string fileName = "myText.txt";
            "128".ToFile(fileName);
            var result = FileAbstract.ReadFile(fileName);
            Assert.NotNull(result);
            Assert.Equal("128", result);
        }

        [Fact]
        public void ReadFileAsTextNoExtension()
        {
            const string fileName = "myText";
            "128".ToFile(fileName);
            var result = FileAbstract.ReadFile(fileName);
            Assert.NotNull(result);
            Assert.Equal("128", result);
        }
    }    
}