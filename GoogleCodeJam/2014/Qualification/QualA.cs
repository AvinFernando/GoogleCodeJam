using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Fourteen.Qualification
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class QualA : CodeJamProblem<QualATestCase>
    {
        protected override void ParseCase(QualATestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.Pick1 = int.Parse(field[0]);
            currentLine++;
            for(int j=0;j<4;j++)
            {
                field = lines[currentLine].Split(_delimiterChars);
                foreach (var val in field)
                    currentCase.Board1[j].Add(int.Parse(val));
                currentLine++;
            }
            field = lines[currentLine].Split(_delimiterChars);
            currentCase.Pick2 = int.Parse(field[0]);
            currentLine++;
            for (int j = 0; j < 4; j++)
            {
                field = lines[currentLine].Split(_delimiterChars);
                foreach (var val in field)
                    currentCase.Board2[j].Add(int.Parse(val));
                currentLine++;
            }

        }
        protected override StringBuilder OutputForEachCase(QualATestCase testCase, StringBuilder currentString)
        {
            if (testCase.Answer == 0)
                currentString.Append("Volunteer cheated!");
            else if (testCase.Answer == 17)
                currentString.Append("Bad magician!");
            else currentString.Append(testCase.Answer);
            return currentString;
        }
    }

    public class QualATestCase : ITestCase
    {
        public List<HashSet<int>> Board1 { get; set; }
        public List<HashSet<int>> Board2 { get; set; }
        public int Pick1 { get; set; }
        public int Pick2 { get; set; }
        public int Answer { get; set; }

        public QualATestCase()
        {
            Board1 = new List<HashSet<int>>();
            Board1.Add(new HashSet<int>());
            Board1.Add(new HashSet<int>());
            Board1.Add(new HashSet<int>());
            Board1.Add(new HashSet<int>());
            Board2 = new List<HashSet<int>>();
            Board2.Add(new HashSet<int>());
            Board2.Add(new HashSet<int>());
            Board2.Add(new HashSet<int>());
            Board2.Add(new HashSet<int>());
        }

        static QualATestCase()
        {
        }

        public void Solve()
        {
            var result = Board1[Pick1-1].Intersect(Board2[Pick2-1]);
            if (result.Count() == 0)
                Answer = 0;
            else if (result.Count() > 1)
                Answer = 17;
            else Answer = result.Single();
        }
    }
}
