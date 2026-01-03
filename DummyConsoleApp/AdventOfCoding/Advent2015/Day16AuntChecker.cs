using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day16AuntChecker
{
    public Day16AuntChecker()
    {
        var reqLines = DataParser.SplitLines(requirementsInput);
        Requirements = [];
        foreach (var reqLine in reqLines)
        {
            var parts = reqLine.Split(": ");
            Requirements[parts[0]] = int.Parse(parts[1]);
        }
    }
    public const string requirementsInput = @"children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1";
    public static HashSet<string> GreaterThan = ["cats", "trees"];
    public static HashSet<string> LessThan = ["pomeranians", "goldfish"];


    public Dictionary<string, int> Requirements = [];
    public void Main()
    {
        Console.WriteLine("Day 16 Aunt Checker");
        var aunt = GetAuntNumber(AdventData2015.Day16AuntChecker);
        Console.WriteLine($"The correct aunt number is {aunt}");
    }

    public int GetAuntNumber(string input)
    {
        foreach (var aunt in DataParser.SplitLines(input))
        {
            var auntSections = aunt.Split([":", " ", ","], StringSplitOptions.RemoveEmptyEntries);
            var auntNumber = int.Parse(auntSections[1]);
            var auntData = new DefaultDictionary<string, int>();
            for (int i = 2; i < auntSections.Length; i += 2)
            {
                var key = auntSections[i];
                var value = int.Parse(auntSections[i + 1]);
                auntData[key] = value;
            }

            if (AuntMatchesReqs(auntData))
            {
                Console.WriteLine($"Aunt {auntNumber} matches requirements ({aunt})");
                return auntNumber;
            }
        }
        throw new Exception("No aunts matched requirements");
    }

    private bool AuntMatchesReqs(DefaultDictionary<string, int> auntData)
    {
        foreach (var propertySet in auntData)
        {
            var key = propertySet.Key;
            var value = propertySet.Value;
            var requirement = Requirements[key];
            if (GreaterThan.Contains(key))
            {
                if (value <= requirement)
                    return false;
            }
            else if (LessThan.Contains(key))
            {
                if (value >= requirement)
                    return false;
            }
            else if (Requirements[key] != value)
                return false;
        }
        return true;
    }
}
