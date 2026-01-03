using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day24BoxWeights
{
    public void Main()
    {
        Console.WriteLine("Day 24 Box Weights");
        var stoppy = Stopwatch.StartNew();
        var minEntanglement = GetMinEntanglement(AdventData2015.Day24BoxWeights);
        stoppy.Stop();
        Console.WriteLine($"Minimum entanglement for 3 groups: {minEntanglement} (calculated in {stoppy.ElapsedMilliseconds} ms)");
    }

    public int GetMinEntanglement(string input) { 
        var boxes = DataParser.SplitDataLineToLong(input).OrderDescending().ToList();
        var boxSets = GetAllValidCombinations(boxes).ToList();
        return 0;
    }

    private IEnumerable<List<long>> GetAllValidCombinations(List<long> items) {
        var totalWeight = items.Sum();
        if (totalWeight % 3 != 0)
            throw new Exception($"Total input {totalWeight} invalid");
        var targetWeight = totalWeight / 3;
        var minBoxes = GetMinBoxes(items, targetWeight);
        var possibleCombinations = GetAllPossibleCombinations(items, targetWeight, minBoxes).ToList();
        while (possibleCombinations.Any())
        { 
            var checkCombination = possibleCombinations.First();
            possibleCombinations.RemoveAt(0);
            var matchItems = possibleCombinations.Where(pc => !pc.Intersect(checkCombination).Any()).ToList();
            if (matchItems.Count > 0)
            {
                yield return checkCombination;
                foreach (var matchItem in matchItems)
                {
                    yield return matchItem;
                    possibleCombinations.Remove(matchItem);
                }
            }
        }

    }

    private int GetMinBoxes(List<long> items, long targetWeight)
    {
        long total = 0;
        var counter = 0;
        foreach (var item in items) {
            counter++;
            total += item;
            if (total >= targetWeight)
                return counter;
        }
        return items.Count;
    }

    private IEnumerable<List<long>> GetAllPossibleCombinations(List<long> items, long target, int minCount = 1) {
        bool foundMatch = false;
        for (int i = minCount; i <= items.Count; i++) {
            foreach (var combination in items.GetCombinations(i)) {
                if (combination.Sum() == target)
                {
                    yield return combination;
                    foundMatch = true;
                }
            }
            if (foundMatch)
                yield break;
        }
    }
}
