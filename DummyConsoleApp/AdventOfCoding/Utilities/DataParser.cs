using System.Data;

namespace DummyConsoleApp.AdventOfCoding.Utilities;

public class DataParser
{
    public static List<List<int>> ParseData(string input)
    {
        var result = new List<List<int>>();
        foreach (var entry in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            result.Add(ParseDataLine(entry));
        }
        return result;
    }

    public static List<int> ParseDataLine(string input)
    {
        List<int> partsList = new List<int>();
        var parts = input.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            partsList.Add(int.Parse(part));
        }
        return partsList;
    }

    public static List<string> SplitDataLine(string input, bool toLower = false)
    {
        return input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(row => toLower
                ? row.Trim().ToLower()
                : row.Trim())
            .ToList();
    }

    public static List<List<char>> ParseDataIntoString(string input)
    {
        var result = new List<List<char>>();
        foreach (var entry in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            result.Add(entry.ToList());
        }
        return result;
    }
}