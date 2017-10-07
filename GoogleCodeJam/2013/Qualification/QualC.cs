using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Thirteen.Qualification
{
    public class QualC: CodeJamProblem<QualCTestCase>
    {
        protected override void ParseCase(QualCTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.LowerBound = BigInteger.Parse(field[0]);
            currentCase.UpperBound = BigInteger.Parse(field[1]);
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(QualCTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.FoundPalindromes);            
            return currentString;
        }
        protected override void DoBeforeSolve()
        {
            var t = new QualCTestCase();
            t.LowerBound = TestCases.Min(x => x.LowerBound);
            t.UpperBound = TestCases.Max(x => x.UpperBound);
            t.Solve();
        }
    }

    public class QualCTestCase : ITestCase
    {
        public static BigInteger GlobalMax { get; set; }
        public static BigInteger GlobalMin { get; set; }
        public static HashSet<BigInteger> UnsortedSquares { get; set; }
        public static SortedSet<BigInteger> FairSquares { get; set; }
        static QualCTestCase()
        {
            FairSquares = new SortedSet<BigInteger>();
            UnsortedSquares = new HashSet<BigInteger>();
            UnsortedSquares.Add(1);
            UnsortedSquares.Add(4);
            UnsortedSquares.Add(9);
        }


        public BigInteger LowerBound { get; set; }
        public BigInteger UpperBound { get; set; }
        public int FoundPalindromes { get; set; }

        #region Utility
        public BigInteger SquareRoot(BigInteger n)
        {
            return (BigInteger)Complex.Sqrt((Complex)n).Real;
        }

        private static bool TrinaryIncrement(ref char old)
        {
            switch (old)
            {
                case '1':
                    old = '2';
                    return false;
                case '2':
                    old = '0';
                    return true;
                default:
                    old = '1';
                    return false;
            }
        }

        private static bool BinaryIncrement(ref char old)
        {
            switch (old)
            {
                case '1':
                    old = '0';
                    return true;
                default:
                    old = '1';
                    return false;
            }
        }

        private static void AddRoot(BigInteger root)
        {
            UnsortedSquares.Add(root * root);
        }

        private static void AddRoot(List<char> root)
        {
            AddRoot(BigInteger.Parse(string.Join(string.Empty, root)));
        }

        private static void AddIfFairStartingWith(char[] halfchr, bool calcsum)
        {
            AddIfFairStartingWith(halfchr, halfchr.Length, calcsum);
        }
        private static void AddIfFairStartingWith(char[] halfchr, int len, bool calcsum)
        {
            var sum = halfchr.Count(x => x=='1');
            if (sum <= 4)
            {
                var candidate = new List<char>(halfchr);
                candidate.AddRange(halfchr.Reverse());
                AddRoot(candidate);                    
                candidate.Insert(len, '0');
                AddRoot(candidate);  
                candidate[len] = '1';
                AddRoot(candidate);  
                candidate[len] = '2';
                if (calcsum && sum <= 2)
                    AddRoot(candidate);  
            }
        }        
        #endregion

        private static void AddPalindromesInRange(BigInteger upper)
        {
            int maxlen = (upper.ToString().Length + 1) / 2;
            for (int len = 1; len <= maxlen; len++)
            {
                var halfchr = string.Format("1{0}", new String('0', len - 1)).ToCharArray();
                while (halfchr.Any(x => x != '0'))
                {
                    AddIfFairStartingWith(halfchr, len, true);

                    for (int i = halfchr.Length - 1; i >= 0 && BinaryIncrement(ref halfchr[i]); i--) ;
                }
                AddIfFairStartingWith(string.Format("2{0}", new String('0', len - 1)).ToCharArray(), len, false);
            }
            FairSquares = new SortedSet<BigInteger>(UnsortedSquares);
        }

        public void Solve()
        {
             if (LowerBound >= GlobalMin && UpperBound <= GlobalMax)
            {
                FoundPalindromes = FairSquares.GetViewBetween(LowerBound, UpperBound).Count();
            }
            else
            {
                AddPalindromesInRange(SquareRoot(UpperBound));

                GlobalMin = 1;
                GlobalMax = UpperBound >= GlobalMax ? UpperBound : GlobalMax;
            }
        }
    }
}
