using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BlockOfText  
{
    public class LineMaker
    {
        /// <summary>
        /// number of chars that forms one line (ARG[2])
        /// </summary>
        private readonly int WidthOfLine;
        /// <summary>
        /// number of chars that can be added to current line before its capacity is reached
        /// </summary>
        private int RemainingCapacity;
        /// <summary>
        /// number of non-white chars on current line
        /// needed for counting spaces to be added to line
        /// </summary>
        private int CharsOnLine;
        /// <summary>
        /// streamwriter writing to output file
        /// </summary>
        private readonly StreamWriter Output;
      
       /// <summary>
       /// words on current line
       /// </summary>
        private readonly List<string> WordsOnline;
        /// <summary>
        /// word that did not fit the last pritnted line and is to be added to next one
        /// </summary>
        private string WordWaiting="";

        public LineMaker(int width, StreamWriter output)
        {
            this.WidthOfLine = width;
            this.WordsOnline = new List<string>();
            this.Output = output;
        }

        /// <summary>
        /// assigns actual values to this.RemainigCapacity and this.CharsOnLine, based on contend of this.WordsOnLine
        /// </summary>
        private void RecountCapacity()
        {
            int charCounter = 0;
            foreach (var item in this.WordsOnline)
            {
                charCounter += item.Length; //count chars in words
            }
            this.CharsOnLine = charCounter;
            if (this.WordsOnline.Count > 0)
            {
                charCounter += (WordsOnline.Count - 1);//count spaces between words
            }
            this.RemainingCapacity = WidthOfLine - charCounter;
        }

        /// <summary>
        /// trys to add word on line, if it fits, returns true, if not, adds line to output, clears it and adds word to next line
        /// </summary>
        /// <param name="Word">Word to be added</param>
        /// <returns></returns>
        public bool BuildLine(WordMaker Word)
        {
            bool added = TryAddWord(Word.Word);
             if (added && !Word.EndOfColumn)
            {
                return false; //not finished line
            }
            else if(!added && Word.EndOfColumn) //special case, word doesnt fit to previous line and it ends column at the same time
            {
                string ToPrint = PrepareLine(false); //column ends after not fitting word just read and stored in Word variable
                //I need to print what is stored in line first
                this.Output.WriteLine(ToPrint);
                //Console.WriteLine(ToPrint);  //TODO: remove

                this.WordsOnline.Clear();
                this.TryAddWord(Word.Word); //add non fitting on new line
                ToPrint = PrepareLine(Word.EndOfColumn);  //printing not fitting word and ending column
                this.Output.WriteLine(ToPrint);
                //Console.WriteLine(ToPrint);  //TODO: remove
                if (!Word.EndOfFile)  //last line of file
                {
                    Output.WriteLine();
                 
                    //Console.WriteLine("\\n"); //TODO: remove
                }
                this.WordWaiting = ""; //not fitting word was stored in WordWaiting even though it has been just printed
            }
            else if (Word.EndOfColumn) //last line of Column
            {
                string ToPrint = PrepareLine(Word.EndOfColumn);  //add spaces between words
                this.Output.WriteLine(ToPrint); //add to output file
                //Console.WriteLine(ToPrint); //TODO: remove
              
                if (!Word.EndOfFile)  //last line of file
                {
                    Output.WriteLine();
                  
                    //Console.WriteLine("\\n"); //TODO: remove
                }

            }

            else //finished line
            {           
                string ToPrint = PrepareLine(Word.EndOfColumn);  //add spaces between words
                this.Output.WriteLine(ToPrint); //add to output file

                //Console.WriteLine(ToPrint); //TODO: remove
            }

            this.WordsOnline.Clear();
            if (this.WordWaiting != "")
            {
                this.WordsOnline.Add(WordWaiting);  //add word that did not fit previous line to next line
            }
            this.WordWaiting = "";
            return true; //line finished

        }

        /// <summary>
        /// converts this.WordsOnLine into string representation of line containing additional spaces
        /// </summary>
        /// <returns>line containg correct number of spaces</returns>
        private string PrepareLine(bool EndOfColumn)
        {
            if(this.WordsOnline.Count == 1) //only one word on line
            {
                return this.WordsOnline[0];  //to be alligned to left
            }
            else if (this.WordsOnline.Count < 1)  //this should never happen
            {
               Console.WriteLine("prepareline doesnt work properly");
                return null;
            }
            else if (EndOfColumn)  //last line of the column, allinged to left
            {
                StringBuilder line = new StringBuilder();
                for (int i = 0; i < WordsOnline.Count-1; i++)
                {
                    line.Append(WordsOnline[i]);
                    line.Append(" ");  //one space between words
                }
                line.Append(WordsOnline[WordsOnline.Count - 1]); //last word of line, no space behind it
                return line.ToString();
            }
            else
            {
                int SpacesToAdd = this.WidthOfLine - CharsOnLine;  //amount of chars (space) to be added on line
                int SpacesToFill = this.WordsOnline.Count - 1; //amount spaces between words
                int BaseSpaces = SpacesToAdd / SpacesToFill; //amount of spaces (chars)to be put into every space between wods 
                int RemaingSpaces = SpacesToAdd % SpacesToFill; //additional spaces (chars) to be added from left

                StringBuilder line = new StringBuilder();
                //add bigger spaces from left
                for (int i = 0; i <RemaingSpaces; i++)
                {
                    line.Append(WordsOnline[i]);
                    for (int j = 0; j < BaseSpaces+1; j++)
                    {
                        line.Append(" ");
                    }
                }
                //add base spaces to remaining slots
                for (int i = RemaingSpaces; i <SpacesToFill; i++)
                {
                    line.Append(WordsOnline[i]);
                    for (int j = 0; j < BaseSpaces; j++)
                    {
                        line.Append(" ");
                    }
                }
                //add last word
                line.Append(WordsOnline[SpacesToFill]);
                return line.ToString();
            }
        }

        /// <summary>
        /// determines whether word does fit the line and if so, adds it to line
        /// </summary>
        /// <param name="word">word to be added</param>
        /// <returns>true if word was added to line, false if word does mot fit the line</returns>
        private bool TryAddWord(string word)
        {
            RecountCapacity();
            if (this.WordsOnline.Count == 0) //adding first word on line
            {
                this.WordsOnline.Add(word); //it doesnt matter how long the first word is, it always fits the line
                return true;
            }
            else if (this.RemainingCapacity >= (word.Length + 1)) //word does fit the line with space before it
            {
                this.WordsOnline.Add(word);
                RecountCapacity();
                return true;
            }
            else //word does not fit the line
            {
                this.WordWaiting = word; //store word
                return false; //inform that line is ready to be printed
            }
            
        }
        /// <summary>
        /// prints content of this.Output to output file
        /// </summary>
        public void FlushToFile()
        {
            this.Output.Flush();
        }

    }
}
