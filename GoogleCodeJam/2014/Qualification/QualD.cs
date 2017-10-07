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
    public class QualD : CodeJamProblem<QualDTestCase>
    {
        protected override void ParseCase(QualDTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            currentLine++; // ignore number of weights
            
            var field = lines[currentLine].Split(_delimiterCharsWithoutDecimal);
            currentLine++;
            foreach (var val in field)
                currentCase.NaomiWeight.Add(float.Parse(val));

            field = lines[currentLine].Split(_delimiterCharsWithoutDecimal);
            currentLine++;
            foreach (var val in field)
                currentCase.KenWeight.Add(float.Parse(val));
        }
        protected override StringBuilder OutputForEachCase(QualDTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.DWarWins).Append(" ").Append(testCase.WarWins);
            return currentString;
        }
    }

    public class QualDTestCase : ITestCase
    {
        public SortedSet<float> NaomiWeight { get; set; }
        public SortedSet<float> KenWeight { get; set; }
        public int WarWins { get; set; }
        public int DWarWins { get; set; }

        public QualDTestCase()
        {
            NaomiWeight = new SortedSet<float>();
            KenWeight = new SortedSet<float>();
        }

        static QualDTestCase()
        {
        }

        public void Solve()
        {
            var nww = NaomiWeight.ToList();
            var kww = KenWeight.ToList();
            float nw;
            bool kenPoint;
            while (nww.Count > 0)
            {
                nw = nww[nww.Count-1];
                nww.RemoveAt(nww.Count - 1);
                kenPoint = kww[kww.Count-1] > nw;
                if (kenPoint)
                    kww.RemoveAt(kww.Count - 1);
                else
                {
                    kww.RemoveAt(0);
                    WarWins++;
                }
            }
            var ndw = NaomiWeight.ToList();
            var kdw = KenWeight.ToList();
            while (ndw.Count > 0)
            {
                if (ndw[0] < kdw[0])
                {
                    ndw.RemoveAt(0);
                    kdw.RemoveAt(kdw.Count() - 1);
                }
                else
                {
                    DWarWins++;
                    ndw.RemoveAt(0);
                    kdw.RemoveAt(0);
                }
            }
        }
    }
}
