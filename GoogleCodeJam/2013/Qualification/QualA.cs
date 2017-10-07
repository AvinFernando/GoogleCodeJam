using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleCodeJam.Thirteen.Qualification
{
    // CodeJamProblem is an abstract class that just implements 
    public class QualA : CodeJamProblem<QualATestCase>
    {
        protected override void ParseCase(QualATestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            for (int j = 0; j < 4; j++)
            {
                var line = lines[currentCaseNum * 5 + j + 1].ToCharArray();

                for (int k = 0; k < 4; k++)
                {
                    switch (line[k])
                    {
                        case 'X':
                            currentCase.Board[j][k] = 1;
                            break;
                        case 'O':
                            currentCase.Board[j][k] = 2;
                            break;
                        case 'T':
                            currentCase.Board[j][k] = 3;
                            break;
                    }
                }
            }
        }
        protected override StringBuilder OutputForEachCase(QualATestCase testCase, StringBuilder currentString)
        {
            switch (testCase.State)
            {
                case 0:
                    currentString.Append("Game has not completed");
                    break;
                case 1:
                    currentString.Append("X won");
                    break;
                case 2:
                    currentString.Append("O won");
                    break;
                case 3:
                    currentString.Append("Draw");
                    break;
            }
            return currentString;
        }

    }

    public class QualATestCase : ITestCase
    {
        public QualATestCase()
        {
            Board = new List<List<int>>();
            for (int i = 0; i < 4; i++)
            {
                Board.Add(new List<int>());
                for (int j = 0; j < 4; j++)
                {
                    Board[i].Add(0);
                }
            }
        }

        public List<List<int>> Board { get; set; }
        public int State { get; set; }
        public bool IsSolved { get; set; }

        private bool CheckFor(int p)
        {
            if (Board.Any(x => x.All(y => y == p || y == 3)))
                return true;
            List<bool> poss = new List<bool>();
            poss.Add(true);
            poss.Add(true);
            poss.Add(true);
            poss.Add(true);
            poss.Add(true);
            poss.Add(true);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    poss[j] &= Board[i][j] == p || Board[i][j] == 3;
                poss[4] &= Board[i][i] == p || Board[i][i] == 3;
                poss[5] &= Board[i][3 - i] == p || Board[i][i] == 3;
            }
            return poss.Any(x => x);
        }

        public void Solve()
        {
            if (CheckFor(1))
                State = 1;
            else if (CheckFor(2))
                State = 2;
            else if (Board.Any(x => x.Any(y => y == 0)))
                State = 0;
            else State = 3;
        }
    }
}
