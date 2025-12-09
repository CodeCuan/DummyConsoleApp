using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.Data;

namespace DummyConsoleApp.AdventOfCoding.Utilities;

public class DataParser
{
    public static List<List<int>> ParseDataIntoIntLists(string input, bool noSeperator = false)
    {
        var result = new List<List<int>>();
        foreach (var entry in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            result.Add(ParseDataLine(entry, noSeperator));
        }
        return result;
    }

    public static List<int> ParseDataLine(string input, bool noSeperator = false)
    {
        List<int> partsList = new List<int>();
        IEnumerable<string> parts = noSeperator
            ? input.ToArray().Select(x => x.ToString())    
            : input.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            partsList.Add(int.Parse(part));
        }
        return partsList;
    }

    public static List<string> SplitDataLine(string input, bool toLower = false, bool trim = true)
    {
        return input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(row => FormatLine(row, toLower, trim))
            .ToList();
    }

    private static string FormatLine(string line, bool toLower, bool trim) {
        if (toLower)
            line = line.ToLower();
        return trim
            ? line.Trim()
            : line;
    }

    public static List<long> SplitDataLineToLong(string input)
    {
        return input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
    }

    public static List<List<char>> ParseDataIntoString(string input)
    {
        var result = new List<List<char>>();
        foreach (var entry in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            result.Add(entry.Trim().ToList());
        }
        return result;
    }

    public static List<Coordinate3D> ParseDataIntoCoordinate3D(string input)
    {
        return input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(row => new Coordinate3D(input))
            .ToList();
    }
}