using DummyConsoleApp.AdventOfCoding.Utilities;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day08EscapeData
{
    private const string sampleFile = "Day08DataSample.txt";
    private const string dataFile = "Day08DataInput.txt";
    public void Main()
    {
        Console.WriteLine("Day 8 Escape Data");
        var stoppy = Stopwatch.StartNew();
        var charCount = EvaluateLinesFromFile(dataFile, false);
        stoppy.Stop();
        Console.WriteLine($"Sample - Total characters of code - characters in memory: {charCount} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var invertedCharCount = EvaluateLinesFromFile(dataFile, true);
        stoppy.Stop();
        Console.WriteLine($"Sample - Total characters to represent escaped strings - original code characters: {invertedCharCount} . Took {stoppy.ElapsedMilliseconds} ms");

    }

    public int EvaluateLinesFromFile(string file, bool invert)
    {
        var lines = DataReader.ReadLines(file, 2015);
        return invert
            ? lines.Sum(ReverseEvaluateLine)
            : lines.Sum(EvaluateLine);
    }

    private static Regex NeedEscapeCharacters = new(@"[\\""]", RegexOptions.Compiled);
    public int ReverseEvaluateLine(string line)
    {
        return 2 + NeedEscapeCharacters.Matches(line).Count;
    }

    private static Regex HexadecRegex = new(@"\\x[0-9a-f]{2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public int EvaluateLine(string line)
    {
        var trimmedLine = line;
        var codeChars = line.Length;
        trimmedLine = trimmedLine.Replace(@"\\", ".");
        trimmedLine = trimmedLine.Replace(@"\""", ".");
        trimmedLine = trimmedLine.Trim('"');
        trimmedLine = HexadecRegex.Replace(trimmedLine, ".");
        var lineLength = trimmedLine.Length;
        return codeChars - lineLength;
    }
}
