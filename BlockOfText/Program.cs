
using System;
using System.IO;
using System.Collections.Generic;

namespace BlockOfText
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            

            bool check = InitialCheck(args, out bool HiglightSpace, out StreamWriter Output, out int WidhtOfLine);
            if (check == false) //problem with arguments or output file
            {
                return;
            }
            
            LineMaker line = new LineMaker(WidhtOfLine, Output, HiglightSpace);
            WordMaker word = new WordMaker();

            int i = 0;
            int flawedFiles = 0; //counter of input files that cannot be opened
            if (HiglightSpace) //is arg0 file name or --HighlightSpaces?
            {
                i= 1;
                flawedFiles = 1;
            }


            for (; i < args.Length-2; i++)
            {
                if (i == (args.Length - 3))
                {
                    word.FinalFile = true;  
                }
                if(!ReadFile(args[i], word, line))
                {
                    flawedFiles++;
                }
            }
            if (flawedFiles == (args.Length - 2)) // all input files are flawed
            {
                line.LineInEmptyFile();
                line.FlushToFile();
            }
            line.PushToWriter();
            line.FlushToFile();



            line.CloseWriter();

        }

        static bool ReadFile(string FileName, WordMaker word, LineMaker line)
        {
            bool fileExists = OpenFile(FileName, out StreamReader Reader);
            if (!fileExists)
            {
                return false;
            }
            word.FileToRead = Reader;

            int input = word.FileToRead.Read(); //initialize input variable, if read char is non-white, is passed from EatSpaces fun. to ReadWord fun 
            word.EatSpaces(ref input); //eat eventual whitespaces at the begining of file, not taking into account columns at the beggining


            while (input != -1)
            {
                word.Clear();
                word.ReadWord(ref input);
                word.EatSpaces(ref input);
                line.BuildLine(word);

            }
            line.FlushToFile();
            word.FileToRead.Close();
            return true;


        }
        static bool InitialCheck(string[] args, out bool HiglightSpaces, out StreamWriter Output, out int WidhtOfLine)
        {
            HiglightSpaces = false;
            WidhtOfLine = 0;
            Output = null;
            //incorect number of args
            if (args.Length < 3)
            {
                Console.WriteLine("Argument Error");               
                return false;
            }
            if(args[0]== "--highlight-spaces")
            {
                if (args.Length < 4)
                {
                    Console.WriteLine("Argument Error");
                    return false;
                }
                HiglightSpaces = true;                
            }
            try
            {
                WidhtOfLine = Convert.ToInt32(args[args.Length-1]); //last arg
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("Argument Error");
                return false;
            }
            if (WidhtOfLine <= 0)
            {
                Console.WriteLine("Argument Error");
                return false;
            }
            try
            {  //TODO: Append mode?
                Output = new StreamWriter(args[args.Length-2], true);  //try to opne output file 
            } 
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is UnauthorizedAccessException || e is DirectoryNotFoundException || e is IOException || e is PathTooLongException)
            {
                Console.WriteLine("File Error");
                return false;
            }

            return true;

        }

        static bool OpenFile(string FileName, out StreamReader Reader)
        {
            try
            {
                Reader = new StreamReader(FileName);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is FileNotFoundException || e is DirectoryNotFoundException || e is IOException)
            {
                Reader = null;
                return false;
            }
            return true;
        }
        /// <summary>
        /// Controls wheather arguments  and files are valid
        /// </summary>
        /// <param name="args">arguments of program</param>
        /// <param name="Reader"></param>
        /// <param name="Writer"></param>
        /// <returns></returns>
        private static int OpenFile(string[] args, out StreamReader Reader, out StreamWriter Writer)
        {
            if (args.Length != 3)
            {
                Reader = null;
                Writer = null;
                Console.WriteLine("Argument Error");
                return 0;
            }
            int thirdArg;
            try
            {
                thirdArg = Convert.ToInt32(args[2]);
            }
            catch(Exception e) when(e is FormatException || e is OverflowException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("Argument Error");
                return 0;
            }
            if (thirdArg <= 0)
            {
                Reader = null;
                Writer = null;
                Console.WriteLine("Argument Error");
                return 0;
            }
            try
            {
                Reader = new StreamReader(args[0]);
            }
            catch (Exception e ) when(e is ArgumentException || e is ArgumentNullException || e is FileNotFoundException || e is DirectoryNotFoundException || e is IOException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("File Error");
                return 0;
            }
            try
            {
                Writer = new StreamWriter(args[1], false);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is UnauthorizedAccessException || e is DirectoryNotFoundException || e is IOException ||e is PathTooLongException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("File Error");
                return 0;
            }
            return 1;

        }
    }
}

//UNIT TESTS
//Unit test class and all used test files are to be find in my public github repository
//https://github.com/prusovak2/MultiFileBlockOfText
/*
 namespace MultyFileTests
{
    [TestClass]
    public class UnitTest1
    {
        //test 1-3 stand for recodex examples 
        [TestMethod]
        public void TestMethod1()
        {
            Clear(@"Tmps\ex01.out");
            Run(@"--highlight-spaces Ins\01.in Tmps\ex01.out 17");
            Assert.IsTrue(Utils.FileDiff(@"Tmps\ex01.out", @"Outs\ex01.out"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Clear(@"Tmps\ex02.out");
            Run(@"--highlight-spaces Ins\01.in Ins\01.in Ins\01.in Tmps\ex02.out 17");
            Assert.IsTrue(Utils.FileDiff(@"Tmps\ex02.out", @"Outs\ex02.out"));
        }
        [TestMethod]
        public void TestMethod3()
        {
            Clear(@"Tmps\ex08.out");
            Run(@"--highlight-spaces xx.in xx.in xx.in Ins\01.in xx.in xx.in Tmps\ex08.out 80");
            Assert.IsTrue(Utils.FileDiff(@"Tmps\ex08.out", @"Outs\ex08.out"));
        }
        //Lorem Ipsum Test
        [TestMethod]
        public void TestMetod4()
        {
            Clear(@"Tmps\LoremIpsum.out");
            Run(@" Ins\LoremIpsum.in Tmps\LoremIpsum.out 40");
            bool a = Utils.FileDiff(@"Tmps\LoremIpsum.out", @"Outs\LoremIpsum_Aligned.out");
            Assert.IsTrue(a);
        }
        //only nonexisting files
        [TestMethod]
        public void TestMetod5()
        {
            Clear(@"Tmps\empty.out");
            Run(@" xx.in xx.in xx.in Tmps\empty.out 4");
            bool a = Utils.FileDiff(@"Tmps\empty.out", @"Outs\empty.out");
            Assert.IsTrue(a);
        }
        //file with randomly distibuted whitespaces
        [TestMethod]
        public void TestMetod6()
        {
            Clear(@"Tmps\Test3x.out");
            Run(@" Ins\testFile.in Ins\testFile.in Ins\testFile.in Tmps\Test3x.out 40");
            bool a = Utils.FileDiff(@"Tmps\Test3x.out", @"Outs\Test3x.out");
            Assert.IsTrue(a);
        }
        //only one char long line
        [TestMethod]
        public void TestMetod7()
        {
            Clear(@"Tmps\short3.out");
            Run(@" Ins\short3.in Ins\short3.in Ins\short3.in Tmps\short3.out 1");
            bool a = Utils.FileDiff(@"Tmps\short3.out", @"Outs\short3.out");
            Assert.IsTrue(a);
        }


        public void Run(string args)
        {
            Program.Main(args.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));

        }

        public void Clear(params string[] Path)
        {
            for (int i = 0; i < Path.Length; i++)
            {
                try
                {
                    File.Delete(Path[i]);
                }
                catch { }
            }
        }
    }

    public static class Utils
    {
        public static bool Diff(string A, string B)
        {
            if (A.Length != B.Length)
                return false;

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] != B[i])
                {
                    Console.WriteLine($"Utils.Diff: Difference at index {i}");
                    return false;
                }
            }
            return true;
        }

        public static bool FileDiff(string A, string B)
        {
            return Diff(File.ReadAllText(A), File.ReadAllText(B));
        }
    }
}
*/
