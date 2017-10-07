using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Fourteen.Round1A
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class Round1AC : CodeJamProblem<Round1ACTestCase>
    {
        protected override void ParseCase(Round1ACTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);

            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1ACTestCase testCase, StringBuilder currentString)
        {
            for (int i = 0; i < Round1ACTestCase.BadFreq.Length; i++)
                currentString.Append(string.Format("{0} {1}", i, Round1ACTestCase.BadFreq[i])).AppendLine();
            return currentString;
        }
    }

    public class Round1ACTestCase : ITestCase
    {
        public static double[] BadFreq = new double[1000];
        public static int[] Bad1 = new int[1000];
        public static int[] Bad2 = new int[1000];
        public static int[] Bad3 = new int[1000];
        public static int[] Good1 = new int[1000];
        public Round1ACTestCase()
        {
        }

        private static void GenerateBadArray(int[] arr)
        {
            int r;
            int p;
            var rng = new Random();
            for (int j = 0; j < 1000; j++)
                arr[j] = j;
            for (int j = 0; j < 1000; j++)
            {
                r = rng.Next(1000);
                p = arr[j];
                arr[j] = arr[r];
                arr[r] = p;
            }
        }

        static Round1ACTestCase()
        {
            for (int i = 0; i < 10000; i++)
            {
                int[] arr = new int[1000];
                GenerateBadArray(arr);
                for (int j = 0; j < 1000; j++)
                    BadFreq[j] += arr[j];
            }
            for (int j = 0; j < 1000; j++)
                BadFreq[j] /= 2000.0;
        }

        public void Solve()
        {

        }
    }
}
