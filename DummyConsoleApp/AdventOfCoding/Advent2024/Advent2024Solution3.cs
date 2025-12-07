using DummyConsoleApp.AdventOfCoding.Data;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2024;

public class Advent2024Solution3
{
    public int Solve(string input)
    {
        return Regex.Matches(input, @"mul\((\-?\d+),(\-?\d+)\)")
             .Select(m => (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
             .Select(t => t.Item1 * t.Item2)
             .Sum();
    }

    public int Solve2(string input)
    {

        var filtered = Regex.Replace(input, @"don't\(\).*?do\(\)", "");
        filtered = Regex.Replace(filtered, @"don't\(\).*?$", "");
        return Regex.Matches(filtered, @"mul\((\-?\d+),(\-?\d+)\)")
             .Select(m => (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
             .Select(t => t.Item1 * t.Item2)
             .Sum();
    }

    internal void Main()
    {
        // Console.WriteLine(Solve2("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))don't()hello"));
        Console.WriteLine(Solve2(AdventData2024.PuzzleThreeCorruptData));
    }
}
