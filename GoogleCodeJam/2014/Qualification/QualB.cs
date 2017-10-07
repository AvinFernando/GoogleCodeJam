using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleCodeJam.Fourteen.Qualification
{
    // CodeJamProblem is an abstract class that just implements 
    public class QualB : CodeJamProblem<QualBTestCase>
    {
        protected override void ParseCase(QualBTestCase currentCase, int currentCaseNum, ref int currentLine, string[] lines)
        {
            var field = lines[currentLine].Split(_delimiterCharsWithoutDecimal);
            currentCase.FarmCost = double.Parse(field[0]);
            currentCase.FarmProduction = double.Parse(field[1]);
            currentCase.Goal = double.Parse(field[2]);
            currentLine++;
        }
        protected override StringBuilder OutputForEachCase(QualBTestCase testCase, StringBuilder currentString)
        {
            currentString.Append(testCase.Seconds);
            return currentString;
        }

    }

    public class QualBTestCase : ITestCase
    {
        public QualBTestCase()
        {
            Seconds = double.MaxValue;
        }

        public double Goal { get; set; }
        public double FarmCost { get; set; }
        public double FarmProduction { get; set; }
        public double Seconds { get; set; }


        public void Solve()
        {
            double currentProduction = 2.0;
            double currentTime = Goal / currentProduction;
            double lastPurchaseTime = 0.0;
            while (currentTime < Seconds) // try buying a farm
            {
                Seconds = currentTime;
                lastPurchaseTime += FarmCost / currentProduction;
                currentProduction += FarmProduction;
                currentTime = lastPurchaseTime + Goal / currentProduction;
            }
        }
    }
}
