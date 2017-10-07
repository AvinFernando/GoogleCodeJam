using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleCodeJam.Thirteen.Qualification
{
    // CodeJamProblem is an abstract class that just implements 
    public class QualB: CodeJamProblem<QualBTestCase>
    {
        protected override void ParseCase(QualBTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.InitializeBoard(int.Parse(field[0]), int.Parse(field[1]));
            currentLine++;
            for (int j = 0; j < currentCase.Height; j++)
            {
                var line = lines[currentLine].Split(_delimiterChars);
                currentLine++;

                for (int k = 0; k < currentCase.Width; k++)
                {
                    currentCase.Board[j][k] = int.Parse(line[k]);
                }
            }
        }
        protected override StringBuilder OutputForEachCase(QualBTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.IsPossible ? "YES" : "NO");            
            return currentString;
        }

    }

    public class QualBTestCase : ITestCase
    {
        public QualBTestCase()
        {
        }

        public void InitializeBoard(int h, int w)
        {
            Height = h;
            Width = w;
            Board = new List<List<int>>();
            for (int i = 0; i < h; i++)
            {
                Board.Add(new List<int>());
                for (int j = 0; j < w; j++)
                {
                    Board[i].Add(0);
                }
            }  
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public List<List<int>> Board { get; set; }


        public int State { get; set; }
        public bool IsPossible { get; set; }

        private void RemoveRow(int row)
        {
            Board.RemoveAt(row);
        }
        private void RemoveCol(int col)
        {
            foreach (var row in Board)
                row.RemoveAt(col);
        }

        public void Solve()
        {
            while (Board.Any() && Board[0].Any())
            {
                int lowestHeight = Board.Min(x => x.Min(y => y));

                bool hasRemoved = false;
                // Look for uniform Rows
                int i = 0;
                while (i < Board.Count)
                {
                    if (Board[i].All(x => lowestHeight == x))
                    {
                        RemoveRow(i);
                        hasRemoved = true;
                    }
                    else i++;
                }

                if (!Board.Any()) break;
                // Look for uniform Columns
                i = 0;
                while (i < Board[0].Count)
                {
                    if (Board.All(x => lowestHeight == x[i]))
                    {
                        RemoveCol(i);
                        hasRemoved = true;
                    }
                    else i++;
                }

                if (!hasRemoved) return;
            }
            IsPossible = true;
        }
    }
}
