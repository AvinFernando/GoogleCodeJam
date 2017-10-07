using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Round1B
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class Round1BC : CodeJamProblem<Round1BCTestCase>
    {
        protected override void ParseCase(Round1BCTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);

            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1BCTestCase testCase, StringBuilder currentString)
        {
            
            return currentString;
        }
    }

    public class Round1BCTestCase : ITestCase
    {
        public Round1BCTestCase()
        {
        }

        static Round1BCTestCase()
        {
        }

        public void Solve()
        {
        }
    }
}
