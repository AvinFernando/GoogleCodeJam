using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Drawing;

namespace GoogleCodeJam.Round1C
{
    /// <summary>
    /// CodeJamProblem contains a collection of TestCases which are created by parsing the input file.
    /// The Solve method is called for each Test Case, then The output file is generated.
    /// </summary>
    public class Round1CB : CodeJamProblem<Round1CBTestCase>
    {
        protected override void ParseCase(Round1CBTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterChars);
            currentCase.Destination = new Point(int.Parse(field[0]), int.Parse(field[1]));
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(Round1CBTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.Path);
            return currentString;
        }

        protected override void  DoBeforeSolve()
        {
            //Round1CBTestCase.GenerateGrid(500, TestCases);
        }
    }



    public class Round1CBTestCase : ITestCase
    {

        public Point Destination { get; set; }
        public string Path { get; set; }

        public static Dictionary<Point, string> Grid { get; set; }

        public Round1CBTestCase()
        {
        }

        static Round1CBTestCase()
        {
            Grid = new Dictionary<Point, string>();
        }

        private static void AttemptAddGrid(Point oldPoint, Queue<Point> queue, string previous, string newDirection)
        {
            Point point = new Point(oldPoint.X, oldPoint.Y);
            switch (newDirection)
            {
                case "E":
                    point.X += previous.Length + 1;
                    break;
                case "W":
                    point.X -= previous.Length + 1;
                    break;
                case "N":
                    point.Y += previous.Length + 1;
                    break;
                case "S":
                    point.Y -= previous.Length + 1;
                    break;
            }
            if (!Grid.ContainsKey(point))
            {
                string newString = string.Concat(previous, newDirection);
                Grid[point] = newString;
                queue.Enqueue(point);
            }
        }

        public static void GenerateGrid(int moves, List<Round1CBTestCase> testCasess)
        {
            Point current = new Point(0,0);
            Grid.Add(current,"");
            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(current);
            while (Grid[current].Length < moves && testCasess.Any(x=>!Grid.ContainsKey(x.Destination)))
            {
                current = pointQueue.Dequeue();
                AttemptAddGrid(current, pointQueue, Grid[current], "E");
                AttemptAddGrid(current, pointQueue, Grid[current], "W");
                AttemptAddGrid(current, pointQueue, Grid[current], "N");
                AttemptAddGrid(current, pointQueue, Grid[current], "S");
            }
        }

        public void Solve()
        {
            bool unsolved = true;
            StringBuilder currentPath = new StringBuilder();
            Point current = new Point(0, 0);
            //Path = Grid[Destination];
            for (int i = 1; unsolved && i <= 250; i++)
            {
                if (current.X != Destination.X)
                {
                    if (current.X < Destination.X)
                    {
                        current.X += 1;
                        currentPath.Append("WE");
                    }
                    else
                    {
                        current.X -= 1;
                        currentPath.Append("EW");
                    }
                }
                else if (current.Y != Destination.Y)
                {
                    if (current.Y < Destination.Y)
                    {
                        current.Y += 1;
                        currentPath.Append("SN");
                    }
                    else
                    {
                        current.Y -= 1;
                        currentPath.Append("NS");
                    }
                }
                else unsolved = false;                
            }
            Path = currentPath.ToString();
        }
    }
}
