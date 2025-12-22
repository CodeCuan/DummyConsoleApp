using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day06MathHomework
{
    public void Main()
    {
        Console.WriteLine("Day 6 Math Homework");
        SolveProblems(AdventData2025.Day6MathHomework);

        SolveProblems(AdventData2025.Day6MathHomework, false);

    }

    List<MathProblem> mathProblems = [];

    public long SolveProblems(string input, bool simple = true)
    {
        if (simple)
            SetDataSimple(input);
        else
            SetData(input);
        long total = 0;
        foreach (var problem in mathProblems)
        {
            problem.Solve();
            total += problem.solution;
        }
        Console.WriteLine($"Total of all solutions: {total}");
        return total;

    }

    public void SetDataSimple(string input)
    {
        mathProblems = [];
        var lines = DataParser.SplitLines(input);
        var dataLines = lines
            .Take(lines.Count - 1)
            .Select(line => DataParser.ParseDataLine(line))
            .ToList();
        var operatorLine = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        for (int i = 0; i < operatorLine.Count; i++)
        {
            var op = operatorLine[i][0];
            var inputs = dataLines.Select(row => row[i]).ToList();
            mathProblems.Add(new MathProblem
            {
                operatorValue = op,
                inputValues = inputs
            });
        }
    }
    public void SetData(string input)
    {
        mathProblems = [];
        var lines = DataParser.SplitLines(input, trim: false);
        var mathInput = new MathProblem();
        var operatorLine = lines.Last();
        for (int i = 0; i < lines.First().Length; i++)
        {
            var dataInput = new String(
                lines
                    .Take(lines.Count - 1)
                    .Select(line => line[i])
                    .ToArray()
                );
            var op = operatorLine.Length > i
                ? operatorLine[i]
                : ' ';

            if (!string.IsNullOrWhiteSpace(dataInput))
            {
                mathInput.inputValues.Add(int.Parse(dataInput));
            }
            if (op != ' ')
            {
                mathInput.operatorValue = op;
            }
            if (string.IsNullOrWhiteSpace(dataInput))
            {
                mathProblems.Add(mathInput);
                mathInput = new MathProblem();
            }
        }
        if (mathInput.inputValues.Count > 0)
        {
            mathProblems.Add(mathInput);
        }
    }

    public class MathProblem
    {
        public char operatorValue;
        public List<int> inputValues = [];
        public long solution;
        public void Solve()
        {
            if (inputValues.Count == 0)
            {
                solution = 0;
                return;
            }
            switch (operatorValue)
            {
                case '+':
                    solution = inputValues.Sum();
                    break;
                case '*':
                    solution = 1;
                    foreach (var val in inputValues)
                    {
                        solution *= val;
                    }
                    break;
                default:
                    throw new NotImplementedException($"Operator {operatorValue} not implemented");
            }
        }
    }
}
