using FileAbstraction;

namespace Demo
{
    public class Program
    {
        public static void Main()
        {
            TextDemos.Simple();
            TextDemos.SearchIncorrectParam();
            TextDemos.SearchShallow();
            TextDemos.SearchDeep();
            TextDemos.SearchFull();

            BinaryDemos.Simple();
        }
    }
    
}
