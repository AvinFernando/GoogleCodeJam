using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Thirteen.Round1A
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class Round1AA: CodeJamProblem<Round1AATestCase>
    {
        protected override void ParseCase(Round1AATestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.CenterRadius = ulong.Parse(field[0]);
            currentCase.Paint = ulong.Parse(field[1]);
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1AATestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.Rings);
            return currentString;
        }
    }

    public class Round1AATestCase : ITestCase
    {
        public ulong Rings { get; set ;}
        public ulong CenterRadius { get; set; }
        public ulong Paint { get; set; }

        static Round1AATestCase()
        {
        }

        public static decimal Sqrt(decimal x, decimal epsilon = 0.0M)
        {
            if (x < 0) throw new OverflowException("Cannot calculate square root from a negative number");

            decimal current = (decimal)Math.Sqrt((double)x), previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + x / previous) / 2;
            }
            while (Math.Abs(previous - current) > epsilon);
            return current;
        }

        public void Solve()
        {
            ulong b = ((2 * CenterRadius) - 1);
            double c = Paint;
            double bigB = (double)(b) / 4.0;
            
            double sol1 = Math.Sqrt(bigB * bigB + c / 2.0) - bigB;
            Rings = (ulong)sol1;
            if (Rings < 1000)
                Rings = 1;
            else 
                Rings -= Math.Min(Rings/10,10000);

            ulong UsedPaint = (2*Rings + 2*CenterRadius - 1)*Rings;
            ulong CurrentRadius = CenterRadius + 2 * Rings;
            CurrentRadius *= 2;
            CurrentRadius += 1;
            var firstGuess = Rings;

            while (UsedPaint <= Paint)
            {
                Rings++;
                UsedPaint += CurrentRadius;
                CurrentRadius += 4;
            }
            Rings--;
            if (firstGuess == Rings && firstGuess > 10)
            {
                Console.Out.WriteLine(Rings);
            }
        }
    }
}
