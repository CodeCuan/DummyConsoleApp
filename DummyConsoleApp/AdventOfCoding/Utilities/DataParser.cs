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