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
    public class Round1AB : CodeJamProblem<Round1ABTestCase>
    {
        protected override void ParseCase(Round1ABTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.TestCaseNumber = currentCaseNum;
            currentCase.InitGraph(int.Parse(field[0]));
            currentLine++;
            for (int i = 0; i < currentCase.Nodes-1; i++)
            {
                field = lines[currentLine].Split(_delimiterChars);
                currentCase.Graph[int.Parse(field[0])].Add(int.Parse(field[1]));
                currentCase.Graph[int.Parse(field[1])].Add(int.Parse(field[0]));
                currentLine++;
            }
        }
        protected override StringBuilder OutputForEachCase(Round1ABTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.NumToRemove);
            return currentString;
        }
    }

    public class Round1ABTestCase : ITestCase
    {
        public Dictionary<int,HashSet<int>> Graph { get; set; }
        public int Nodes { get; set; }
        public int NumToRemove { get; set; }
        public int TestCaseNumber { get; set; }

        public void InitGraph(int nodes)
        {
            Nodes = nodes;
            for (int i = 1; i <= nodes; i++)
                Graph.Add(i,new HashSet<int>());
        }
        public Round1ABTestCase()
        {
            Graph = new Dictionary<int, HashSet<int>>();
        }

        static Round1ABTestCase()
        {
        }

        public void TryRemoving(int node, IEnumerable<int> remainingIndexes)
        {
            remainingIndexes = remainingIndexes.Where(x=>x != node);
            if (remainingIndexes.Count() <= Nodes - NumToRemove)
                return;
            if (IsFullB(remainingIndexes))
                NumToRemove = Nodes - remainingIndexes.Count();
            else
                foreach (var i in remainingIndexes)
                    TryRemoving(i, remainingIndexes);
        }

        public void Solve()
        {
            HashSet<int> indexes = new HashSet<int>(Graph.Keys);
            NumToRemove = IsFullB(indexes) ? 0 : Nodes - 1;
            foreach (var kvp in Graph)
                TryRemoving(kvp.Key, indexes);

        }

        private bool IsFullB(IEnumerable<int> remainingIndexes)
        {
            var modGraph = new Dictionary<int, HashSet<int>>();
            foreach (var kvp in Graph)
                if (remainingIndexes.Contains(kvp.Key))
                {
                    modGraph[kvp.Key] = new HashSet<int>(kvp.Value);
                    modGraph[kvp.Key].IntersectWith(remainingIndexes);
                }

            int rootCandidate = 0;
            foreach (var kvp in modGraph)
            {
                if (kvp.Value.Contains(kvp.Key))
                    return false;
                if (kvp.Value.Count == 0 || kvp.Value.Count > 3)
                    return false;
                if (kvp.Value.Count == 2)
                {
                    if (rootCandidate != 0)
                        return false;
                    rootCandidate = kvp.Key;
                }
            }
            if (rootCandidate == 0)
                return false;
            HashSet<int> visited = new HashSet<int>();
            visited.Add(rootCandidate);
            var toVisit = new Queue<int>();
            foreach (var link in modGraph[rootCandidate])
                toVisit.Enqueue(link);
            int numVisited = 1;
            while (toVisit.Count > 0)
            {
                numVisited++;
                int v = toVisit.Dequeue();
                visited.Add(v);
                foreach (var link in modGraph[v])
                    if (!visited.Contains(link))
                        toVisit.Enqueue(link);
            }
            return numVisited == modGraph.Count;
        }
    }
}
