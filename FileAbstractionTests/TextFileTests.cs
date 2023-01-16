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

        [Fact]
        public static void ReadFileDirectlyUserDirectory()
        {
            var content = "This was found in my user folder";
            var dir = Path.Combine(@"C:", "Users", $"{Environment.UserName}", "Downloads", "myFile.txt");

            var result = FileAbstract.ReadFile(dir, SearchDepth.Deep);
            Assert.NotNull(result);
            Assert.Equal(content.ToString(), result);
        }
    }    
}