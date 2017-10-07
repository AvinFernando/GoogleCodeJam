using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GoogleCodeJam.Fourteen.Round1B
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
            var nCities = int.Parse(field[0]);
            var nFlights = int.Parse(field[1]);
            currentLine++;
            for (int i = 0; i < nCities; i++)
            {
                currentCase.Zips.Add(int.Parse(lines[currentLine].Split(_delimiterChars)[0]));
                currentLine++;
            }
            currentCase.Init();
            for (int i = 0; i < nFlights; i++)
            {
                field = lines[currentLine].Split(_delimiterChars);
                currentCase.Flights[int.Parse(field[0]) - 1, int.Parse(field[1]) - 1] = true;
                currentCase.Flights[int.Parse(field[1]) - 1, int.Parse(field[0]) - 1] = true;
                currentLine++;
            }

        }
        protected override StringBuilder OutputForEachCase(Round1BCTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.VisitedCities);
            return currentString;
        }
    }

    public class Round1BCTestCase : ITestCase
    {
        public string VisitedCities { get; set; }
        public bool[,] Flights { get; set; }
        public List<int> Zips { get; set; }
        public Dictionary<long, int> ZipsByMask { get; set; }
        public Dictionary<int, long> MasksByCity { get; set; }
        public SortedDictionary<int, long> MasksByZip { get; set; }
        public SortedDictionary<int, int> CitiesByZip { get; set; }
        public Dictionary<int,long> AvailableFlights { get; set; }
        public Dictionary<long, long> FlightsByMask { get; set; }
        public HashSet<long> PruneTree { get; set; }

        public bool CanPrune(long visited)
        {
            if (PruneTree.Contains(visited))
                return true;
            PruneTree.Add(visited);
            return false;
        }

        public Round1BCTestCase()
        {
            Zips = new List<int>();
            CitiesByZip = new SortedDictionary<int, int>();
            PruneTree = new HashSet<long>();
            MasksByCity = new Dictionary<int, long>();
            ZipsByMask = new Dictionary<long, int>();
            MasksByZip = new SortedDictionary<int, long>();
            FlightsByMask = new Dictionary<long, long>();
        }
        public void Init()
        {
            Flights = new bool[Zips.Count,Zips.Count];            
            for (int i = 0; i < Zips.Count; i++)
                CitiesByZip[Zips[i]] = i;
            int j = 0;
            long mask = 1L;
            foreach (var kvp in CitiesByZip)
            {
                MasksByCity[kvp.Value] = mask;
                MasksByZip[kvp.Key] = mask;
                ZipsByMask[mask] = kvp.Key;
                mask <<= 1;
                j++;
            }
        }

        static Round1BCTestCase()
        {
        }

        public List<long> SplitMasks(long mask)
        {
            List<long> masks = new List<long>();
            long next = 0;
            while (mask > 0)
            {
                next = ((mask - 1) ^ mask) & mask;
                mask ^= next;
                masks.Add(next);
            }
            return masks;
        }


        public string TryVisit(long mask, long toVisit, Stack<long> visitList, long availableFlights)
        {
            toVisit ^= mask;
            if (toVisit == 0)
                return ZipsByMask[mask].ToString();
            Stack<long> removed = new Stack<long>();
            while (visitList.Count > 0 && ((FlightsByMask[visitList.Peek()] & mask) == 0))
                removed.Push(visitList.Pop());
            visitList.Push(mask);
            if (removed.Count == 0)
                availableFlights |= FlightsByMask[mask];
            else
            {
                availableFlights = 0L;
                foreach (var formerCity in visitList)
                    availableFlights |= FlightsByMask[formerCity];
            }
            var possible = SplitMasks(toVisit & availableFlights);
            foreach (var next in possible)
            {
                var retString = TryVisit(next, toVisit, visitList, availableFlights);
                if (retString != null)
                    return string.Concat(ZipsByMask[mask], retString);
            }
            visitList.Pop();
            while (removed.Count > 0)
                visitList.Push(removed.Pop());
            return null;

        }

        public void Solve()
        {
            AvailableFlights = new Dictionary<int,long>();
            var mask = 1L;
            var allCities = 0L;
            for (int i = 0; i < Zips.Count; i++)
            {
                allCities |= mask;
                AvailableFlights[i] = 0;
                FlightsByMask[MasksByCity[i]] = 0;
                for (int j = 0; j < Zips.Count; j++)
                    if (Flights[i, j])
                    {
                        AvailableFlights[i] |= MasksByCity[j];
                        FlightsByMask[MasksByCity[i]] |= MasksByCity[j];
                    }
                mask <<= 1;
            }

            var ToVisit = allCities;
            var availableFlights = 0L;
            var visitList = new Stack<long>();
            foreach (var kvp in MasksByZip)
            {
                VisitedCities = TryVisit(kvp.Value, ToVisit, visitList, availableFlights);
                if (VisitedCities != null)
                    return;
            }
        }
    }
}
