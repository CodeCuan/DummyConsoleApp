using System.Diagnostics;
using Combinatorics.Collections;
using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day24BoxWeights
{
    private const string Sample =
        @"1
2
3
4
5
7
8
9
10
11";

    public void Main()
    {
        Console.WriteLine("Day 24 Box Weights");
        var stoppy = Stopwatch.StartNew();
        var minEntanglement = GetMinEntanglement(AdventData2015.Day24BoxWeights);
        stoppy.Stop();
        Console.WriteLine(
            $"Minimum entanglement for 3 groups: {minEntanglement} (calculated in {stoppy.ElapsedMilliseconds} ms)"
        );
        stoppy.Restart();
        var minEntanglement4 = GetMinEntanglement(AdventData2015.Day24BoxWeights, 4);
        stoppy.Stop();
        Console.WriteLine(
            $"Minimum entanglement for 4 groups: {minEntanglement4} (calculated in {stoppy.ElapsedMilliseconds} ms)"
        );
    }

    public long GetMinEntanglement(string input, int boxCount = 3)
    {
        var boxes = DataParser.SplitDataLineToLong(input).OrderDescending().ToList();
        var boxSets = GetAllValidCombinations(boxes, boxCount).ToList();
        var bestBoxSet =
            boxSets.MinBy(GetEntanglement) ?? throw new Exception("No valid box set found");
        return GetEntanglement(bestBoxSet);
    }

    private long GetEntanglement(List<long> items)
    {
        return items.Aggregate(1L, (acc, val) => acc * val);
    }

    private IEnumerable<List<long>> GetAllValidCombinations(List<long> items, int boxCount)
    {
        var totalWeight = items.Sum();
        if (totalWeight % boxCount != 0)
            throw new Exception($"Total input {totalWeight} invalid");
        var targetWeight = totalWeight / boxCount;

        var minCount = GetMinCount(items, targetWeight);

        for (int count = minCount; count <= items.Count; count++)
        {
            bool matchFound = false;
            var combinations = new Combinations<long>(items, count);

            foreach (var combination in combinations)
            {
                if (combination.Sum() == targetWeight)
                {
                    yield return combination.ToList();
                    matchFound = true;
                }
            }
            if (matchFound)
            {
                yield break;
            }
        }
    }

    private int GetMinCount(List<long> baseList, long targetWeight)
    {
        long total = 0;
        var counter = 0;
        foreach (var item in baseList)
        {
            counter++;
            total += item;
            if (total >= targetWeight)
                return counter;
        }
        return baseList.Count;
    }
}
