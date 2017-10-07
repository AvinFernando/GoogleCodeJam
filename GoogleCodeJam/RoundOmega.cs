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
    public class RoundOmega: CodeJamProblem<RoundOmegaTestCase>
    {
        protected override void ParseCase(RoundOmegaTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);

            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(RoundOmegaTestCase testCase, StringBuilder currentString)
        {
            
            return currentString;
        }
    }

    public class Pattern
    {
        public int AFactor { get; set; }
        public int BFactor { get; set; }
        public int CFactor { get; set; }

        public Pattern(int a, int b, int c)
        {
            AFactor = a;
            BFactor = b;
            CFactor = c;
        }

        public Pattern() { }

        public Pattern(Pattern old)
        {
            AFactor = old.AFactor;
            BFactor = old.BFactor;
            CFactor = old.CFactor;
        }
    }

    public class RoundOmegaTestCase : ITestCase
    {
        int NumRounds { get; set; }

        static RoundOmegaTestCase()
        {
        }

        public void Solve()
        {
            
        }
    }
}
