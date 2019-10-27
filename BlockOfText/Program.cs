using System;
using System.IO;
using System.Collections.Generic;

namespace BlockOfText
{
    class Program
    {
       
        static void Main(string[] args)
        {
            

            bool check = InitialCheck(args, out bool HiglightSpace, out StreamWriter Output, out int WidhtOfLine);
            if (check == false) //problem with arguments or output file
            {
                return;
            }
            
            LineMaker line = new LineMaker(WidhtOfLine, Output,HiglightSpace);
            WordMaker word = new WordMaker();

            int i = 0;
            if (HiglightSpace) //is arg0 file name or --HighlightSpaces?
            {
                i= 1;
            }

            for ( i = 0; i < args.Length-2; i++)
            {
                if (i == (args.Length - 3))
                {
                    word.FinalFile = true;  //TODO: this is totally wrong
                }
                ReadFile(args[i], word, line);
            }

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
