using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticsLab
{
    class PairWiseAlign
    {
        
        /// <summary>
        /// Align only 5000 characters in each sequence.
        /// </summary>
        private int MaxCharactersToAlign = 5000;

        private int INDEL = 5;
        private int MATCH = -3;
        private int SUB = 1;

        private Char[] charA, charB;
        private int[] resultRow;

        //Dictionary<string, int> previouslyCalculatedValues = new Dictionary<string, int>();
        /// <summary>
        /// this is the function you implement.
        /// </summary>
        /// <param name="sequenceA">the first sequence</param>
        /// <param name="sequenceB">the second sequence, may have length not equal to the length of the first seq.</param>
        /// <param name="resultTableSoFar">the table of alignment results that has been generated so far using pair-wise alignment</param>
        /// <param name="rowInTable">this particular alignment problem will occupy a cell in this row the result table.</param>
        /// <param name="columnInTable">this particular alignment will occupy a cell in this column of the result table.</param>
        /// <returns>the alignment score for sequenceA and sequenceB.  The calling function places the result in entry rowInTable,columnInTable
        /// of the ResultTable</returns>
        public int Align(GeneSequence sequenceA, GeneSequence sequenceB, ResultTable resultTableSoFar, int rowInTable, int columnInTable)
        {
            // a place holder computation.  You'll want to implement your code here.
            //string key = sequenceA.Sequence.ToString() + sequenceB.Sequence.ToString();
            //if (previouslyCalculatedValues.ContainsKey(key))
            //    return previouslyCalculatedValues[key];

            // set up algorithm
            initializeSequencing(sequenceA, sequenceB);

            // calculate each additional row
            //Overall time complexity of this part is O(n^2) and space complexity is O(n)
            for (int i = 0; i < charA.Length; i++) // will go through the length of our frist sequence array size 0-5000 O(n)
                resultRow = createNextRow(resultRow, charA[i], charB); //O(n)

            // return score
            //previouslyCalculatedValues.Add(key,resultRow[resultRow.Length - 1]);
            return resultRow[resultRow.Length - 1];       
        }

        //Function is of time complexity O(n)
        // uses previous row to compute next cell -> keeps memory in n space, and keeps the space complexity to O(2n) which is O(n)
        public int[] createNextRow(int[] bottomRow, Char currentCharA, Char[] charB)
        {
            int[] topScores = new int[bottomRow.Length];
            topScores[0] = bottomRow[0] + INDEL; // This sets up the base row which increases by a value of 5 each column
            for (int j = 1; j < topScores.Length; j++) //This will run though O(n) times
            {
                // new score is calculated by grabbing min of the three edits (indel, subsitution, or match)
                topScores[j] = Math.Min(Math.Min(
                    topScores[j - 1] + INDEL, bottomRow[j] + INDEL),
                    currentCharA == charB[j - 1] ? bottomRow[j - 1] + MATCH : bottomRow[j - 1] + SUB);
            }
            return topScores;
        }

        //Sets up the array with the initial values for sequencing
        private void initializeSequencing(GeneSequence sequenceA, GeneSequence sequenceB)
        {
            // grabs first 5000 characters of the sequences to evaluate
            charA = stringLimit(sequenceA.Sequence, MaxCharactersToAlign).ToCharArray();
            charB = stringLimit(sequenceB.Sequence, MaxCharactersToAlign).ToCharArray();
            resultRow = new int[charB.Length + 1];

            // initialize result row with costs for INDEL
            for (int i = 0; i < resultRow.Length; i++)
                resultRow[i] = i * INDEL;
        }

        //Above are functions dealing with the alignment, below are functions dealing with the extraction

        public String[] extractSequences(GeneSequence sequenceA, GeneSequence sequenceB)
        {
            // set up backtrace
            initializeSequencing(sequenceA, sequenceB);

            // initialize table to store each row
            List<int[]> resultTable = new List<int[]>(charA.Length + 1);
            resultTable.Add(resultRow);

            // calculate individual table
            for (int i = 0; i < charA.Length; i++) //recalculates the table in O(n^2) time as before in scoring algorithm
                resultTable.Add(createNextRow(resultTable[i], charA[i], charB));

            // initialize stringholders
            StringBuilder one = new StringBuilder();
            StringBuilder two = new StringBuilder();
            int row = charA.Length;
            int col = charB.Length;

            // backtrace strings
            // creates the string in reverse order as it traverses from the end to the beginning only going through those on the final path
            while (row != 0 || col != 0)
            {
                if (resultTable[row][col] == resultTable[row][col - 1] + INDEL)
                {
                    one.Append('-');
                    two.Append(charB[--col]);
                }
                else if (resultTable[row][col] == resultTable[row - 1][col] + INDEL)
                {
                    one.Append(charA[--row]);
                    two.Append('-');
                }
                else if (resultTable[row][col] == resultTable[row - 1][col - 1] + MATCH ||
                    resultTable[row][col] == resultTable[row - 1][col - 1] + SUB)
                {
                    one.Append(charA[--row]);
                    two.Append(charB[--col]);

                }
                else
                    throw new ArgumentException();
            }

            String[] results = new String[2];
            results[0] = reverseString(one.ToString());
            results[1] = reverseString(two.ToString());
            return results;
        }

        //reverses string
        private String reverseString(String s)
        {
            Char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new String(charArray);
        }

        // returns a substring of the limit length
        public String stringLimit(String seq, int limit)
        {
            if (seq.Length > limit)
                return seq.Substring(0, limit);
            return seq;

        }
    }
}
