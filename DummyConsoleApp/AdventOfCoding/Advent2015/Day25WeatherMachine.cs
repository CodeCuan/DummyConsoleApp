using System.Diagnostics;
using DummyConsoleApp.AdventOfCoding.Data;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day25WeatherMachine
{
    public void Main()
    {
        Console.WriteLine("Day 25 Weather Machine");
        var codeString = $"[{AdventData2015.Day25Row}, {AdventData2015.Day25Column}]";
        var stoppy = Stopwatch.StartNew();
        var codeNum = GetOrderAtPosition(AdventData2015.Day25Row, AdventData2015.Day25Column);
        var code = GetPassword(codeNum);
        stoppy.Stop();
        Console.WriteLine(
            $"Code at position at {codeString} ({codeNum}) is {code} . Generated after {stoppy.ElapsedMilliseconds} ms"
        );
    }

    private object GetPassword(
        long passIterations,
        long seedNumber = 20151125,
        long multiplyBy = 252533,
        long divideBy = 33554393
    )
    {
        for (long i = 1; i < passIterations; i++)
        {
            seedNumber = (seedNumber * multiplyBy) % divideBy;
        }
        return seedNumber;
    }

    public long GetOrderAtPosition(int row, int column)
    {
        int order = 0;
        for (int c = 1; c <= column; c++)
        {
            order += c;
        }
        for (int r = 2; r <= row; r++)
        {
            order += (column + r - 2);
        }
        return order;
    }
}
