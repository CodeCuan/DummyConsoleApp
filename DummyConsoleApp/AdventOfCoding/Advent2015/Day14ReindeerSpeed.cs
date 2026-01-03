using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;
public class Day14ReindeerSpeed
{
    public void Main() {
        Console.WriteLine("Day 14: Reindeer Speed");
        var stoppy = Stopwatch.StartNew();
        var maxDistance = GetMaxTravel(2503, AdventData2015.Day14Speeds);
        stoppy.Stop();
        Console.WriteLine($"Max distance travelled: {maxDistance} . time taken {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var maxPoints = GetMaxPoints(2503, AdventData2015.Day14Speeds);
        stoppy.Stop();
        Console.WriteLine($"Max points: {maxPoints} . time taken {stoppy.ElapsedMilliseconds} ms");
    }

    private int GetMaxPoints(int totalTTime, string input)
    {
        DefaultDictionary<string, int> points = [];
        var inputLines = DataParser.SplitLines(input);
        for (int currentTime = 1; currentTime <= totalTTime; currentTime++)
        {
            var winningReindeers = inputLines.GroupBy(line => GetDistanceTravelled(currentTime, line));
            foreach (var reinDeer in winningReindeers.MaxBy(g => g.Key)!) {
                points[reinDeer]++;
            }
        }
        return points.Values.Max();
    }

    public int GetMaxTravel(int totalTTime, string input)
    {
        var maxDistance = DataParser.SplitLines(input).Max(line => GetDistanceTravelled(totalTTime, line));

        return maxDistance;
    }

    public int GetDistanceTravelled(int totalTTime, string record)
    {
        var data = record.Split(' ');
        var speed = int.Parse(data[3]);
        var flyTime = int.Parse(data[6]);
        var restTime = int.Parse(data[13]);
        var travelTime = GetTravelTime(totalTTime, flyTime, restTime);
        return speed * travelTime;
    }

    public int GetTravelTime(int totalTTime, int flyTime, int restTime) { 
        var fullCycles = totalTTime / (flyTime + restTime);
        var extraTime = Math.Min(flyTime, totalTTime % (flyTime + restTime));
        return fullCycles * flyTime + extraTime;
    }
}
