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
    public class Round1AB: CodeJamProblem<Round1ABTestCase>
    {
        protected override void ParseCase(Round1ABTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.MaxEnergy = int.Parse(field[0]);
            currentCase.Regenerate = int.Parse(field[1]);
            currentCase.NumActivities = int.Parse(field[2]);
            currentLine++;
            field = lines[currentLine].Split(_delimiterChars);
            foreach (var num in field)
                currentCase.Activities.Add(new Activity(int.Parse(num),0));
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1ABTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.MaxValue);
            return currentString;
        }
    }

    public class Activity
    {
        public Activity() { }
        public Activity(long val, long exp) { Val = val; Expense = exp; }
        public long Val { get; set; }
        public long Expense { get; set; }
    }

    public class Round1ABTestCase : ITestCase
    {
        public long MaxEnergy { get; set; }
        public long Regenerate { get; set; }
        public long NumActivities { get; set; }
        public List<Activity> Activities { get; set; }
        public long MaxValue { get; set; }
        public Round1ABTestCase()
        {
            Activities = new List<Activity>();
        }

        static Round1ABTestCase()
        {
        }

        private void CalculateValue()
        {
            long sum = 0;
            for (int i = 0; i < Activities.Count; i++)
                sum += Activities[i].Val * Activities[i].Expense;
            if (sum > MaxValue)
                MaxValue = sum;
        }

        private int GetMaxIndexIn(List<Activity> currentList)
        {
            long highest = 0;
            int hind = 0;
            for (int j = 0; j < currentList.Count; j++)
            {
                if (currentList[j].Val > highest)
                {
                    highest = currentList[j].Val;
                    hind = j;
                }
            }
            return hind;
        }


        public void Maximize(List<Activity> currentList, long EStart, long EEnd)
        {
            if (currentList.Count == 0)
                return;
            if (EStart > MaxEnergy)
                EStart = MaxEnergy;
            if (EEnd > MaxEnergy)
                EEnd = MaxEnergy;
            if (EStart < Regenerate)
                EStart = Regenerate;
            if (EEnd < Regenerate)
                EEnd = Regenerate;
            if (currentList.Count == 1)
                currentList[0].Expense = Math.Min(Math.Max(EStart - EEnd + Regenerate, 0), MaxEnergy);
            else
            {
                var slack = EStart - EEnd + currentList.Count * Regenerate;
                if (slack <= 0)
                    return;                
                var i = GetMaxIndexIn(currentList);
                var slackToi = EStart - EEnd + i * Regenerate;
                currentList[i].Expense = new List<long> { slack, EStart + i * Regenerate, slackToi }.Min();
                if (currentList[i].Expense < slack)
                {
                    Maximize(currentList.GetRange(0, i), EStart,
                        currentList[i].Expense);
                    if (i + 1 < currentList.Count)
                        Maximize(currentList.GetRange(i + 1, currentList.Count - i - 1),
                            Math.Min(EStart + i * Regenerate, MaxEnergy) - currentList[i].Expense + Regenerate
                            , EEnd);
                }
            }
        }

        public void Solve()
        {
            long availableEnergy = MaxEnergy;
            Dictionary<int, int> nextGreaterIndicies = new Dictionary<int, int>();
            if (Activities.Count == 1)
            {
                availableEnergy = MaxEnergy;
            }
            Stack<int> lastSeenMax = new Stack<int>();
            lastSeenMax.Push(Activities.Count - 1);
            nextGreaterIndicies.Add(Activities.Count -1, Activities.Count-1);
            for (int i = Activities.Count - 2; i >= 0; i--)
            {
                if (Activities[i].Val < Activities[i + 1].Val)
                    lastSeenMax.Push(i+1);

                while (lastSeenMax.Count > 0 && Activities[i].Val >= Activities[lastSeenMax.Peek()].Val)
                    lastSeenMax.Pop();
                
                nextGreaterIndicies.Add(i, lastSeenMax.Count > 0 ? lastSeenMax.Peek() : i);                
            }

            for (int i = 0; i < Activities.Count; i++)
            {
                int nextGreaterIndex = nextGreaterIndicies[i];
                if (nextGreaterIndicies[i] == i)
                    Activities[i].Expense = availableEnergy;
                else
                    Activities[i].Expense = Math.Min(Math.Max(availableEnergy + (nextGreaterIndicies[i] - i) * Regenerate - MaxEnergy, 0),availableEnergy);
                availableEnergy -= Activities[i].Expense;

                availableEnergy += Regenerate;
                if (availableEnergy >= MaxEnergy)
                    availableEnergy = MaxEnergy;
            }
            CalculateValue();
        }

        public void Solve2()
        {
            if (Regenerate > MaxEnergy)
                Regenerate = MaxEnergy;
            Maximize(Activities, MaxEnergy, Regenerate);
            CalculateValue();
        }
    }
}
