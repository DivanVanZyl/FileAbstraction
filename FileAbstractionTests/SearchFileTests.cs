using FileAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstractionTests
{
    public class SearchFileTests
    {        
        [Fact]
        public static void SearchIncorrectParam()
        {
            var content = "You found me, event though an incorrect path was given!";
            content.ToFile(Path.Combine("X:", "I hide here.txt"));

            var result = FileAbstract.ReadFile("I hide here.txt");
            Assert.NotNull(result);
            Assert.Equal(content, result);
        }

        [Fact]
        public static void SearchShallowBackTextFile()
        {
            var content = 8192;
            content.ToFile(@$"..{Path.DirectorySeparatorChar}Bas3 tw0 numb3r5.txt");

            var result = FileAbstract.ReadFile("Bas3 tw0 numb3r5.txt");
            Assert.NotNull(result);
            Assert.Equal(content.ToString(), result);
        }

        [Fact]
        public static void SearchDeepBackUserDirectoryTextFile()
        {
            var content = "This was found in my user folder";
            var fileName = "myUserFile.txt";
            content.ToFile(Path.Combine(@"C:", "Users", $"{Environment.UserName}", "Downloads", fileName));


            var result = FileAbstract.ReadFile(fileName);
            Assert.NotNull(result);
            Assert.Equal(content.ToString(), result);
        }

        [Fact]
        public static void SearchForwardOnAnotherDrive()
        {
            var content = "You found me on another drive!";
            content.ToFile("E:" + Path.DirectorySeparatorChar + "And I hide on another drive.txt");

            var result = FileAbstract.ReadFile("And I hide on another drive.txt");
            Assert.NotNull(result);
            Assert.Equal(content.ToString(), result);
        }
    }
}
