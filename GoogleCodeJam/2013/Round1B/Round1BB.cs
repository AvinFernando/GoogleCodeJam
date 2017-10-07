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
    public class Round1BB : CodeJamProblem<Round1BBTestCase>
    {
        protected override void ParseCase(Round1BBTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.NumDiamonds = long.Parse(field[0]);
            currentCase.X = long.Parse(field[1]);
            currentCase.Y = long.Parse(field[2]);
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1BBTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.Odds);
            return currentString;
        }

        protected override void DoBeforeSolve()
        {
            Round1BBTestCase.Precalculate(1000000);
        }
    }

    public class Pos
    {
        public Pos() {}
        public Pos(long x, long y) { X = x; Y = y; }

        public long X { get; set;}
        public long Y { get; set;}

        public override bool  Equals(object obj)
        {
            if (!(obj is Pos))
                return false;
            var other = obj as Pos;
            return (other.X == X & other.Y == Y);
        }

        public override int  GetHashCode()
        {
 	        return X.GetHashCode() ^ Y.GetHashCode();
        }
    }

    public class BoardPos
    {
        public BoardPos() { Diamonds = new HashSet<Pos>(); }
        public BoardPos(BoardPos other) { Diamonds = new HashSet<Pos>(other.Diamonds); }
        public HashSet<Pos> Diamonds { get; set; }

        public void AddDiamond(long x, long y)
        {
            Diamonds.Add(new Pos(x, y));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoardPos))
                return false;
            var other = obj as BoardPos;
            return other.Diamonds.SetEquals(Diamonds);
        }

        public override int GetHashCode()
        {
            return Diamonds.GetHashCode() ^ Diamonds.Count.GetHashCode();
        }
    }

    public class Round1BBTestCase : ITestCase
    {
        public long NumDiamonds { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        public double Odds { get; set; }


        public static Dictionary<long, Dictionary<BoardPos, double>> AllPos { get; set; }
        public static Dictionary<long, long> NumPoss { get; set; }

        public static HashSet<BoardPos> GetNewPositions(Pos diamondPos, BoardPos oldPos)
        {
            HashSet<BoardPos> positions = new HashSet<BoardPos>();
            if (diamondPos.Y == 0)
            {
                var newPos = new BoardPos(oldPos);
                newPos.Diamonds.Add(diamondPos);
                positions.Add(newPos);
            }
            else
            {
                Pos possiblePos;
                bool blockedBoth = true;
                possiblePos = new Pos(diamondPos.X - 1, diamondPos.Y - 1);
                if (!oldPos.Diamonds.Contains(possiblePos)) {
                    blockedBoth = false;
                    positions.UnionWith(GetNewPositions(possiblePos, oldPos));
                }
                possiblePos = new Pos(diamondPos.X + 1, diamondPos.Y - 1);
                if (!oldPos.Diamonds.Contains(possiblePos))
                {
                    positions.UnionWith(GetNewPositions(possiblePos, oldPos));
                    blockedBoth = false;
                }
                if (blockedBoth)
                {
                    var newPos = new BoardPos(oldPos);
                    newPos.Diamonds.Add(diamondPos);
                    positions.Add(newPos);
                }
            }

            return positions;
        }

        public static HashSet<BoardPos> GetNewPositions(KeyValuePair<BoardPos,double> oldPos)
        {            
            BoardPos pos = new BoardPos(oldPos.Key);
            return GetNewPositions(new Pos(0,pos.Diamonds.Where(x=>x.X ==0).Max(x=>x.Y)+2) ,pos);
        }

        public static void Precalculate(long diamonds)
        {
            NumPoss[1]= 1;
            AllPos.Add(1, new Dictionary<BoardPos, double>());
            BoardPos pos = new BoardPos();
            pos.AddDiamond(0,0);
            AllPos[1].Add(pos,1);

            for (long diamond = 2; diamond <= diamonds; diamond++)
            {
                AllPos.Add(diamond, new Dictionary<BoardPos, double>());
                foreach (var oldPos in AllPos[diamond - 1])
                {
                    var newPositions = GetNewPositions(oldPos);
                    foreach (var newPos in newPositions)
                    {
                        if (!AllPos[diamond].ContainsKey(newPos))
                            AllPos[diamond][newPos] = 0;
                        AllPos[diamond][newPos] += oldPos.Value / (double)newPositions.Count;
                    }
                }

            }
        }

        public Round1BBTestCase()
        {
        }

        static Round1BBTestCase()
        {
            NumPoss = new Dictionary<long, long>();
            AllPos = new Dictionary<long, Dictionary<BoardPos, double>>();
        }

        public void Solve()
        {
            var pos = new Pos(X,Y);
            foreach (var kvp in AllPos[NumDiamonds])
                if (kvp.Key.Diamonds.Contains(pos))
                    Odds += kvp.Value;
        }
    }
}
