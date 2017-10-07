using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Round1C
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class Round1CC : CodeJamProblem<Round1CCTestCase>
    {
        protected override void ParseCase(Round1CCTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.NumTribes = int.Parse(field[0]);
            while (currentCase.Tribes.Count < currentCase.NumTribes)
            {
                currentLine++;
                field = lines[currentLine].Split(_delimiterChars);
                currentCase.Tribes.Add(new Tribe(
                    long.Parse(field[0]), long.Parse(field[1]), long.Parse(field[2]), long.Parse(field[3]), 
                    long.Parse(field[4]), long.Parse(field[5]), long.Parse(field[6]), long.Parse(field[7])));
            }
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1CCTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.NumBreaches);
            return currentString;
        }
    }

    public class Tribe
    {
        public long AttackDay { get; set; }
        public long TotalNumAttacks { get; set; }
        public Interval AttackInterval { get; set; }
        public long Strength { get; set; }
        public long AttackPeriod { get; set; }
        public long DeltaDistance { get; set; }
        public long DeltaStrength { get; set; }
        public bool Expired { get; set; }


        public long AttacksSoFar { get; set; }

        public Tribe() { }
        public Tribe(long d, long n, long w, long e, long s, long dd, long dp, long ds)
        {
            AttackDay = d;
            TotalNumAttacks = n;
            AttackInterval = new Interval(w * 2, e * 2);
            Strength = s;
            AttackPeriod = dd;
            DeltaDistance = dp * 2;
            DeltaStrength = ds;
        }

        private bool DoesAttackDay(long day)
        {
            return day >= AttackDay;
        }

        private IEnumerable<Interval> GetOverlaps(Dictionary<Interval, long> Wall, Interval attack)
        {
            return Wall.Where(x => x.Key.DoesOverlap(attack)).Select(x=>x.Key);
        }


        public int AttackWall(long day, Dictionary<Interval, long> wall, Dictionary<Interval, long> improvements, HashSet<Interval> obsolete)
        {
            if (Expired || !DoesAttackDay(day))
                return 0;
            AttacksSoFar++;
            int breach = 0;

            var overlaps = GetOverlaps(wall, AttackInterval);
            var segmentedAttack = new HashSet<Interval>();
            segmentedAttack.Add(AttackInterval);
            foreach (var wallSegment in overlaps)
                if (wall[wallSegment] >= Strength)
                    wallSegment.SubtractFrom(segmentedAttack);
                else
                    obsolete.Add(wallSegment);
            if (segmentedAttack.Count > 0)
                breach = 1;
            foreach (var attackBreach in segmentedAttack)
                if (improvements.ContainsKey(attackBreach))
                    improvements[attackBreach] = Math.Max(Strength, improvements[attackBreach]);
                else
                    improvements[attackBreach] = Strength;

            if (AttacksSoFar == TotalNumAttacks)
                Expired = true;

            AttackInterval = AttackInterval.AddDelta(DeltaDistance);
            Strength += DeltaStrength;
            AttackDay += AttackPeriod;
            return breach;
        }

        public int AttackWall(long day, Dictionary<long, long> wall, Dictionary<long, long> improvements)
        {
            if (Expired || !DoesAttackDay(day))
                return 0;
            AttacksSoFar++;
            int breach = 0;
            for (long i = AttackInterval.Min; i <= AttackInterval.Max; i++)
            {
                if (!wall.ContainsKey(i))
                    wall[i] = 0;
                if (wall[i] < Strength)
                {
                    breach = 1;
                    improvements[i] = Math.Max(improvements.ContainsKey(i) ? improvements[i] : 0, Strength);
                }
            }
            if (AttacksSoFar == TotalNumAttacks)
                Expired = true;

            AttackInterval = AttackInterval.AddDelta(DeltaDistance);
            Strength += DeltaStrength;
            AttackDay += AttackPeriod;
            return breach;
        }
    }

    public struct Interval : IComparable<Interval>
    {
        public long Min { get; set; }
        public long Max { get; set; }

        public Interval(long min, long max) : this() { Min = min; Max = max; }
        public Interval(Interval other) : this() { Min = other.Min; Max = other.Max; }

        public override bool Equals(object obj)
        {
            if (obj is Interval)
            {
                var other = (Interval)obj;
                return other.Min == Min && other.Max == Max;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        public int CompareTo(Interval other)
        {
            return Min.CompareTo(other.Min);
        }

        public bool DoesOverlap(Interval other)
        {
            return (Min <= other.Min && Max >= other.Min)
                || (Max >= other.Max && Min <= other.Max);
        }

        public Interval AddDelta(long delta)
        {
            Min += delta;
            Max += delta;
            return this;
        }

        public void SubtractFrom(HashSet<Interval> intervals)
        {
            HashSet<Interval> toRemove = new HashSet<Interval>();
            HashSet<Interval> toAdd = new HashSet<Interval>();
            foreach (var interval in intervals)
                if (DoesOverlap(interval)) {
                    toRemove.Add(interval);
                    if (interval.Min < Min)
                        toAdd.Add(new Interval(interval.Min, Min));
                    if (interval.Max > Max)
                        toAdd.Add(new Interval(Max, interval.Max));
                }
            intervals.ExceptWith(toRemove);
            intervals.UnionWith(toAdd);
        }
    }


    public class Round1CCTestCase : ITestCase
    {
        public List<Tribe> Tribes { get; set; }
        public int NumTribes { get; set; }
        public int NumBreaches { get; set; }
        public Dictionary<long, long> Wall { get; set; }

        public Round1CCTestCase()
        {
            Tribes = new List<Tribe>();
            Wall = new Dictionary<long, long>();
        }

        static Round1CCTestCase()
        {
        }

        public void Solve()
        {
            Dictionary<Interval, long> wall = new Dictionary<Interval, long>();
            Dictionary<Interval, long> improvements = new Dictionary<Interval, long>();
            HashSet<Interval> obsolete = new HashSet<Interval>();
            long day = Tribes.Min(x => x.AttackDay);
            while (Tribes.Any(x => !x.Expired))
            {
                foreach (var tribe in Tribes)
                    NumBreaches += tribe.AttackWall(day, wall, improvements, obsolete);
                foreach (var obsoleteSegement in obsolete)
                    wall.Remove(obsoleteSegement);
                obsolete.Clear();
                foreach (var improvement in improvements)
                    wall[improvement.Key] = improvement.Value;
                improvements.Clear();
                day = Tribes.Min(x => x.Expired ? long.MaxValue : x.AttackDay);
            }
        }

        public void Solve2()
        {
            Dictionary<long, long> improvements = new Dictionary<long, long>();
            long day = Tribes.Min(x => x.AttackDay);
            while (Tribes.Any(x => !x.Expired))
            {
                foreach (var tribe in Tribes)
                    NumBreaches += tribe.AttackWall(day, Wall, improvements);
                foreach (var improvement in improvements)
                    Wall[improvement.Key] = improvement.Value;
                improvements.Clear();
                day = Tribes.Min(x => x.Expired ? long.MaxValue : x.AttackDay);
            }
        }
    }
}

