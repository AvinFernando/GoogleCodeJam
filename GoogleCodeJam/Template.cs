using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Fourteen
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class Round1: CodeJamProblem<Round1TestCase>
    {
        protected override void ParseCase(Round1TestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);

            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1TestCase testCase, StringBuilder currentString)
        {
            
            return currentString;
        }
    }

    public class Round1TestCase : ITestCase
    {
        static Round1TestCase()
        {
        }

        public void Solve()
        {
        }
    }
}
