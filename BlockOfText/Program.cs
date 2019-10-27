using System;
using System.IO;
using System.Collections.Generic;

namespace BlockOfText
{
    class Program
    {
       
        static void Main(string[] args)
        {
            int check = OpenFile(args, out StreamReader Reader, out StreamWriter Output);
            if (check == 0) //problem with arguments or files
            {
                return;
            }
            

            WordMaker word = new WordMaker(Reader);
            int input = word.FileToRead.Read(); //initialize input variable, if read char is non-white, is passed from EatSpaces fun. to ReadWord fun 
            word.EatSpaces(ref input); //eat eventual whitespaces at the begining of file, not taking into account columns at the beggining
            LineMaker line = new LineMaker(Convert.ToInt32(args[2]), Output);

            while(input != -1)
            {               
                    word.Clear();   
                    word.ReadWord(ref input);                    
                    word.EatSpaces(ref input);
                    line.BuildLine(word);
                
            }
            line.FlushToFile();
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
