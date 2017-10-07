using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class QualC : CodeJamProblem<QualCTestCase>
    {
        protected override void ParseCase(QualCTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {                       
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.Rows = int.Parse(field[0]);
            currentCase.Cols = int.Parse(field[1]);
            currentCase.Mines = int.Parse(field[2]);
            currentCase.InitBoard();
            currentLine++;    
        }
        protected override StringBuilder OutputForEachCase(QualCTestCase testCase, StringBuilder currentString)
        {
            if (testCase.Board == null)
                currentString.AppendLine().Append("Impossible");
            else
            {                
                foreach (var row in testCase.Board)
                {
                    currentString.AppendLine();
                    foreach (var val in row)
                        currentString.Append(val);
                }
            }
            return currentString;
        }
    }

    public class QualCTestCase : ITestCase
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int Mines { get; set; }
        public List<List<char>> Board { get; set; }

        public void InitBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                Board.Add(new List<char>());
                for (int j = 0; j < Cols; j++)
                    Board[i].Add('.');
            }
        }

        public QualCTestCase()
        {
            Board = new List<List<char>>();
        }

        static QualCTestCase()
        {
        }

        public void Solve()
        {
            int space = Rows * Cols - Mines;
            Board[0][0] = 'c';
            int curRow = Rows-1;
            #region One or Two Rows or Cols
            if (curRow == 0)
            {
                var curCol = Cols - 1;
                while (curCol > 0 && Mines > 0)
                {
                    Board[0][curCol] = '*';
                    curCol--;
                    Mines--;
                }
                if (Mines > 0)
                    Board = null;
            }
            if (Cols == 1)
            {
                while (curRow > 0 && Mines > 0)
                {
                    Board[curRow][0] = '*';
                    curRow--;
                    Mines--;
                }
                if (Mines > 0)
                    Board = null;
            }
            if (Cols == 2 && Rows == 2)
            {
                if (Mines == 3)
                {
                    Board[0][1] = '*';
                    Board[1][0] = '*';
                    Board[1][1] = '*';
                    Mines = 0;
                }
                else
                    Board = null;
            }
            if (Cols == 2 && Rows > 2)
            {
                while (curRow > 0 && Mines > 1 && Board != null)
                {
                    Board[curRow][0] = '*';
                    Board[curRow][1] = '*';
                    curRow--;
                    Mines -= 2;
                }
                if (Mines == 1 && curRow == 0)
                    Board[0][1] = '*';
                else if (Mines > 0)
                    Board = null;
            }
            if (Cols >= 2 && Rows == 2)
            {
                var curCol = Cols - 1;
                while (curCol > 0 && Mines > 1 && Board != null)
                {
                    Board[0][curCol] = '*';
                    Board[1][curCol] = '*';
                    curCol--;
                    Mines -= 2;
                }
                if (Mines == 1 && curCol == 0)
                    Board[1][0] = '*';
                else if (Mines > 0)
                    Board = null;
            } 
            #endregion
            while (Mines > 0 && Board != null && curRow >= 0)
            {
                if (curRow > 2)
                {
                    if (Mines >= Cols)
                    {
                        Mines -= Cols;
                        for (int i = 0; i < Cols; i++)
                            Board[curRow][i] = '*';
                    }
                    else if (Mines == Cols - 1)
                    {
                        Mines = 1;
                        for (int i = 2; i < Cols; i++)
                            Board[curRow][i] = '*';
                    }
                    else 
                    {
                        for (int i = Cols - Mines; i < Cols; i++)
                            Board[curRow][i] = '*';
                        Mines = 0;
                    }
                }
                else
                {
                    int curCol = Cols-1;
                    while (Mines > 0 && Board != null && curRow >= 0 && curCol >= 0)
                    {
                        if (curCol > 2)
                        {
                            if (Mines > curRow)
                            {
                                Mines -= curRow + 1;
                                for (int i = 0; i <= curRow; i++)
                                    Board[i][curCol] = '*';
                            }
                            else
                            {
                                Mines--;
                                Board[curRow][curCol] = '*';                                
                            }
                        }
                        else
                        {
                            if (Board[curRow][curCol] != '*')
                            {
                                Board[curRow][curCol] = '*';
                                Mines--;
                            }
                            if (Mines > 0)
                            {
                                if (Mines >= 2)
                                {
                                    Board[curRow][0] = '*';
                                    Board[curRow][1] = '*';
                                    Mines -= 2;
                                } 
                                if (Mines >= 2)
                                {
                                    Board[0][curCol] = '*';
                                    Board[1][curCol] = '*';
                                    Mines -= 2;
                                }
                                if (Mines == 3)
                                {
                                    Board[0][1] = '*';
                                    Board[1][0] = '*';
                                    Board[1][1] = '*';
                                    Mines = 0;
                                }
                                if (Mines > 0)
                                    Board = null;
                            }
                        }
                        curCol--;
                    }
                }
                curRow--;
            }
            if (Mines > 0)
                Board = null;
        }
    }
}
