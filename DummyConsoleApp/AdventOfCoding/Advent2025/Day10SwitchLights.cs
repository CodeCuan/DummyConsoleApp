using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using Google.OrTools.LinearSolver;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day10SwitchLights
{
    public void Main()
    {
        var stoppy = Stopwatch.StartNew();
        var totalSum = GetTotalSwitchCountForLines(AdventData2025.Day10SwitchDiagrams);
        stoppy.Stop();
        Console.WriteLine($"Total count is {totalSum}, took {stoppy.ElapsedMilliseconds} ms to calculate");
        stoppy = Stopwatch.StartNew();
        totalSum = GetTotalSwitchCountForLines(AdventData2025.Day10SwitchDiagrams, true);
        stoppy.Stop();
        Console.WriteLine($"Total voltage count is {totalSum}, took {stoppy.ElapsedMilliseconds} ms to calculate");
    }

    public int GetTotalSwitchCountForLines(string line, bool checkVoltage = false)
    {
        var dataLines = DataParser.SplitLines(line);
        var totalSum = 0;
        foreach (var dataLine in dataLines)
        {
            totalSum += GetSwitchCountForLine(dataLine, checkVoltage);
        }
        return totalSum;
    }

    public int GetSwitchCountForLine(string line, bool checkVoltage)
    {
        var lightConfig = new LightConfig(line);
        if (checkVoltage)
            return lightConfig.GetValidVoltageCombination();
        else
            return lightConfig.GetValidCombination();
    }

    private class LightConfig
    {
        public int GetValidCombination(bool checkVoltage = false)
        {
            int buttonCount = 1;
            while (true)
            {
                foreach (var buttonCombination in ButtonSets.GetCombinations(buttonCount))
                {
                    if (PowersLights(buttonCombination))
                        return buttonCount;
                }
                buttonCount++;
            }
        }

        private bool PowersLights(List<List<int>> buttons)
        {
            var lightToggles = buttons
                .SelectMany(x => x)
                .GroupBy(y => y)
                .ToDictionary(g => g.Key, g => g.Count());
            for (int i = 0; i < ExpectedStates.Count; i++)
            {
                var expected = ExpectedStates[i];
                if (lightToggles.TryGetValue(i, out int toggleCount))
                {
                    if (expected == (toggleCount % 2 == 0))
                        return false;
                }
                else
                {
                    if (expected)
                        return false;
                }
            }
            return true;
        }

        public int GetValidVoltageCombination()
        {
            Solver solver = Solver.CreateSolver("SCIP");
            Objective objective = solver.Objective();
            objective.SetMinimization();
            List<Variable> buttonVariables = [];
            for (int bVar = 0; bVar < ButtonSets.Count; bVar++)
            {
                var bVariable = solver.MakeIntVar(0, 1000, $"b{bVar}");
                buttonVariables.Add(bVariable);
                objective.SetCoefficient(bVariable, 1);
            }
            for (int joltageIndex = 0; joltageIndex < JoltageRequirements.Count; joltageIndex++)
            {
                var expectedJoltage = JoltageRequirements[joltageIndex];
                List<Variable> releventButtonVariables = [];
                for (int buttonIndex = 0; buttonIndex < ButtonSets.Count; buttonIndex++)
                {
                    if (ButtonSets[buttonIndex].Contains(joltageIndex))
                    {
                        releventButtonVariables.Add(buttonVariables[buttonIndex]);
                    }
                }
                LinearExpr expr = new LinearExpr();
                foreach (var bVar in releventButtonVariables)
                    expr += bVar;
                var constraint = expr == expectedJoltage;
                solver.Add(constraint);

            }
            var resultStatus = solver.Solve();
            var totalSum = (int)buttonVariables.Sum(bVar => bVar.SolutionValue());
            Console.WriteLine($"solution = {totalSum}: {string.Join(", ", buttonVariables.Select(bv =>
                $"{bv.Name()}={bv.SolutionValue()}"
            ))}");
            return totalSum;

        }

        public LightConfig(string input)
        {
            var sections = input.Split([']', '[', '{', '}'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var light in sections.First())
            {
                ExpectedStates.Add(light == '#');
            }
            foreach (var buttonSet in sections[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                var buttonNumbers = buttonSet.Trim('(').Trim(')');
                ButtonSets.Add(
                    buttonNumbers.Split(',').Select(int.Parse).ToList()
                    );
            }
            JoltageRequirements = sections[2].Split(',').Select(int.Parse).ToList();
        }
        public List<bool> ExpectedStates = [];
        public List<List<int>> ButtonSets = [];
        public List<int> JoltageRequirements = [];
    }
}
