using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day17Containers
{
    private const string sample = @"20
15
10
5
5";
    private bool log = false;
    public void Main()
    {
        Console.WriteLine("Day 17 Containers");
        var combinations = GetCombinationCount(AdventData2015.Day17Containers, 150);
        Console.WriteLine($"Total combinations to reach 150 liters: {combinations}");

        var minCombinations = CountCombinationsMin(AdventData2015.Day17Containers, 150);
        Console.WriteLine($"Total combinations to reach 150 liters: {minCombinations}");
        log = true;
        GetCombinationCount(sample, 25);


    }

    public int GetCombinationCount(string input, int targetVolume)
    {
        var containerSizes = DataParser.SplitDataLineToLong(input).OrderDescending().ToList();
        return CountCombinations(containerSizes, targetVolume);
    }

    public int CountCombinationsMin(string input, int targetVolume)
    {
        var containerSizes = DataParser.SplitDataLineToLong(input).OrderDescending().ToList();
        return CountCombinationsMin(containerSizes, targetVolume);
    }

    private int CountCombinations(List<long> containers, int targetVolume, int currentIndex = 0)
    {

        var combinations = GetAllCombinations(containers, targetVolume, []).ToList();
        foreach (var combi in combinations)
        {
            if (log)
                Console.WriteLine(string.Join(", ", combi));
        }
        return combinations.Count;
    }

    private int CountCombinationsMin(List<long> containers, int targetVolume, int currentIndex = 0)
    {

        var combinations = GetAllCombinations(containers, targetVolume, [])
            .GroupBy(cl => cl.Count(i => i > 0));
        return combinations.MinBy(g => g.Key)!.Count();
    }

    private IEnumerable<List<long>> GetAllCombinations(List<long> containers, int targetVolume, List<long> activeList, int currentIndex = 0)
    {
        var activeSize = containers[currentIndex];
        var maxCount = targetVolume / activeSize;
        maxCount = Math.Min(maxCount, 1);
        currentIndex++;
        if (currentIndex == containers.Count)
        {
            if(targetVolume - maxCount* activeSize == 0)
            {
                activeList.Add(maxCount);
                yield return activeList;
            }
            yield break;
        }
        for (int i = 0; i <= maxCount; i++)
        {
            var remainingVolume = targetVolume - (int)(i * activeSize);
            if (remainingVolume == 0)
            {
                yield return [.. activeList, i];
            }
            else
            {
                var combinations = GetAllCombinations(containers,
                    remainingVolume, [.. activeList, i], currentIndex);
                foreach (var combo in combinations)
                {
                    yield return combo;
                }
            }
        }

    }
}
