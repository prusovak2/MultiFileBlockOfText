using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockOfText;
using System.IO;

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
}
