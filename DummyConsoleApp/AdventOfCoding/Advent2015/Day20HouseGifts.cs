using Google.OrTools.ConstraintSolver;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day20HouseGifts
{
    public void Main()
    {
        Console.WriteLine("Day 20 House Gifts");
        LogSample();
        var stoppy = Stopwatch.StartNew();
        var houseNum = GetMinHouseNumber(34000000, false);
        stoppy.Stop();
        Console.WriteLine($"House number receiving at least 34,000,000 presents: {houseNum} . Completed in {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        houseNum = GetMinHouseNumber(34000000, false, presentsPerHouse: 11, maxGifts: 50);
        stoppy.Stop();
        Console.WriteLine($"House number receiving at least 34,000,000 presents: {houseNum} . Completed in {stoppy.ElapsedMilliseconds} ms");
    }

    public int GetMinHouseNumber(int targetPresents, bool log, int presentsPerHouse = 10, int? maxGifts = null)
    {
        int houseNumber = 1;
        while (true)
        {
            var presents = CountGifteres(houseNumber, maxGifts) * presentsPerHouse;
            if (presents >= targetPresents)
                return houseNumber;
            houseNumber++;
            if (log && houseNumber % 100 == 0)
                Console.WriteLine($"Checked up to house {houseNumber}, {presents} presents");
        }
    }

    public void LogSample()
    {
        for (int houseNumber = 1; houseNumber <= 10; houseNumber++)
        {
            var presents = CountGifteres(houseNumber);
            Console.WriteLine($"House {houseNumber} gets {presents} presents");
        }
    }

    public int CountGifteres(int houseNumber, int? maxGifts = null, bool log = false)
    {
        var factors = GetFactors(houseNumber, maxGifts);
        if (log)
            Console.WriteLine($"House {houseNumber} factors: {string.Join(", ", factors)}");
        int totalPresents = factors.Sum(f => f);
        return totalPresents;
    }

    public List<int> GetFactors(int number, int? maxGifts)
    {
        var factors = new List<int>();
        var minNumber = 1;
        if (maxGifts.HasValue)
            minNumber = Math.Max(1, number / maxGifts.Value);
        for (int i = 1; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                factors.Add(i);
                var complement = number / i;
                if (complement != i)
                    factors.Add(complement);
            }
        }
        return factors.Where(f => f >= minNumber).ToList(); ;
    }
}
