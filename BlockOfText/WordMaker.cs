using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BlockOfText
{
    /// <summary>
    /// Reads words from input file and informs about ends of columns and end of file
    /// </summary>
    public class WordMaker 
    {
        /// <summary>
        /// word just read
        /// </summary>
        public string Word ="";
        /// <summary>
        /// Streamreader conected to input file
        /// </summary>
        public StreamReader FileToRead;
        /// <summary>
        /// informs whether word ends comlumn
        /// </summary>
        public bool EndOfColumn = false;
        /// <summary>
        /// informs whether word ends line
        /// </summary>
        public bool EndOfFinalFile = false;
        public bool FinalFile = false;

        public bool EndOfFile;

        const int space = 32;
        const int newline = 10;
        const int tab = 9;
        const int cr = 13;

        private int NewlineCounter = 0;

       /* public WordMaker(StreamReader fileToRead)
        {
            this.FileToRead = fileToRead;
        }
        */

        /// <summary>
        /// skips whitespaces between words, sets EndOfColumn/EndOfLine flag to true if column/line ends
        /// </summary>
        /// <param name="LastRead">last char read, should be passed white, return non-white</param>
        public void EatSpaces(ref int LastRead) 
        {
            int input = LastRead;
            while (input == space || input == tab || input == newline || input == cr) //skip white chars
            {
                if (input == newline)
                {
                    NewlineCounter++;
                }
                input = FileToRead.Read();
            }

            if (NewlineCounter >= 2  && input != -1 ) //check whether column ends 
            {
                EndOfColumn = true;
            }
            if(input == -1)  //check wether file ends
            {
                EndOfFile = true;
                if (FinalFile)
                {
                    EndOfFinalFile = true;
                }
            }
            LastRead = input; 
        }

        /// <summary>
        /// reads a word from input file
        /// </summary>
        /// <param name="LastRead">last char read, shoud be passed non-white, return white</param>
        public void ReadWord(ref int LastRead)  //takes non-white char read by eatspaces function
        {
            int input = LastRead;  // non white char acidently eaten by eatspaces function
            StringBuilder builder = new StringBuilder();
            char InputChar;

            while(input != newline && input != tab && input != space && input != -1 && input != cr ) //reads word
            {
                InputChar = (char)input;
                builder.Append(InputChar);
                input = FileToRead.Read();
            }

            this.Word = builder.ToString();
            LastRead = input;
        }
        
        public  override string ToString()
        {
            return this.Word;
        }
        /// <summary>
        /// sets default values to WordMaker's flags 
        /// </summary>
        public void Clear()
        {
            this.Word = "";
            this.NewlineCounter = 0;
            this.EndOfColumn = false;
        }
    }
}
