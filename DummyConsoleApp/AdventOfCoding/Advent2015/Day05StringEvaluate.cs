using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day05StringEvaluate
{
    public void Main()
    {
        Console.WriteLine("Day 5 String Evaluate");
        var stoppy = Stopwatch.StartNew();
        var niceCount = CountNiceStrings(AdventData2015.Day5Words);
        stoppy.Stop();
        Console.WriteLine($"Number of nice strings: {niceCount} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var niceCountAdvanced = CountNiceStrings(AdventData2015.Day5Words, advanced: true);
        stoppy.Stop();
        Console.WriteLine($"Number of nice strings (advanced): {niceCountAdvanced} . Took {stoppy.ElapsedMilliseconds} ms");
    }

    public int CountNiceStrings(string input, bool advanced = false)
    {
        var lines = DataParser.SplitLines(input);
        int niceCount = 0;
        foreach (var line in lines)
        {
            if (advanced
                ? StringIsValidAdvanced(line)
                : StringIsValid(line))
                niceCount++;
        }
        return niceCount;
    }

    private static Regex DoubleLetters = new(@"([a-z])\1", RegexOptions.Compiled);
    private static Regex InvalidCombinations = new(@"(ab|cd|pq|xy)", RegexOptions.Compiled);
    private static Regex Vowels = new(@"[aeiou]", RegexOptions.Compiled);
    public bool StringIsValid(string word) { 
        if(InvalidCombinations.IsMatch(word))
            return false;
        if (!DoubleLetters.IsMatch(word))
            return false;
        var vowelCount = Vowels.Matches(word).Count;
        if (vowelCount < 3)
            return false;
        return true;
    }

    private static Regex DoubleLettersTwice = new(@"([a-z]{2}).*\1", RegexOptions.Compiled);
    private static Regex ThreeLetterSet = new(@"([a-z]).\1", RegexOptions.Compiled);
    public bool StringIsValidAdvanced(string word)
    {
        if (!DoubleLettersTwice.IsMatch(word))
            return false;
        if(!ThreeLetterSet.IsMatch(word))
            return false;
        return true;
    }
}
