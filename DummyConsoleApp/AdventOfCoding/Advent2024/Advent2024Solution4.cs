using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2024;

public class Advent2024Solution4
{
    public int Search(string input)
    {
        var sum = 0;
        foreach (var line in BreakDown(input))
        {
            sum += Regex.Count(line, "XMAS");
            sum += Regex.Count(line, "SAMX");
        }
        return sum;
    }

    public int SearchX(string input)
    {
        var data = DataParser.ParseDataIntoString(input);
        int matches = 0;
        foreach (var row in data.Skip(1).Take(data.Count - 2))
        {
            for (int col = 1; col < row.Count - 1; col++)
            {
                if (row[col] == 'A')
                {
                    if (IsValidCoordinate(data.IndexOf(row), col, data))
                        matches++;
                }
            }
        }
        return matches;
    }

    public bool IsValidCoordinate(int row, int col, List<List<char>> data)
    {
        var row1 = data[row - 1];
        var row2 = data[row + 1];
        string d1 = $"{row1[col - 1]}{row2[col + 1]}";
        string d2 = $"{row1[col + 1]}{row2[col - 1]}";

        return (d1 == "MS" || d1 == "SM") && (d2 == "MS" || d2 == "SM");
    }

    public IEnumerable<string> BreakDown(string input)
    {
        var data = DataParser.SplitDataLine(input);
        foreach (var line in data)
        {
            yield return line;
        }
        for (int i = 0; i < data[0].Length; i++)
        {
            yield return new string(data.Select(row => row[i]).ToArray());
        }

        foreach (var line in GetDiagonals(data, false))
        {
            yield return line;
        }

        foreach (var line in GetDiagonals(data, true))
        {
            yield return line;
        }
    }

    public IEnumerable<string> GetDiagonals(List<string> data, bool leftDiags)
    {
        List<List<char>> accumulators = new List<List<char>>();

        foreach (var line in data)
        {
            int index = 0;
            var lineCharacters = leftDiags ? line.Reverse().ToArray() : line.ToArray();

            foreach (var lineChar in lineCharacters)
            {
                if (accumulators.Count <= index)
                {
                    accumulators.Add(new List<char>());
                }
                accumulators[index].Add(lineChar);
                index++;
            }
            accumulators.Insert(0, new List<char>());
            yield return new string(accumulators.Last().ToArray());
            accumulators.Remove(accumulators.Last());
        }
        foreach (var accumulator in accumulators)
        {
            if (accumulator.Count > 0)
            {
                yield return new string(accumulator.ToArray());
            }
        }


    }

    public void LogBreakDown(string input)
    {
        foreach (var line in BreakDown(input))
        {
            Console.WriteLine(line);
        }
    }

    public void Main()
    {
        Console.WriteLine("Advent2024 Solution 4");
        Console.WriteLine(SearchX(AdventData2024.PuzzleFourWordSearch));
    }
}
