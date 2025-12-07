using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day7TachyonBeam
{
    public int splits = 0;
    public void Main()
    {
        Console.WriteLine("Day 7 Tachyon Beam");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        ProcessMultiBeam(AdventData2025.Day7TachyonMap);
        ProcessBeamTimelines(AdventData2025.Day7TachyonMap);
        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
    }

    public int ProcessMultiBeam(string input)
    {
        splits = 0;
        var data = DataParser.ParseDataIntoString(input);
        HashSet<int> activeBeam = [data.First().IndexOf('S')];
        foreach (var dataLine in data.Skip(1))
        {
            activeBeam = ProcessMultiBeam(activeBeam, dataLine);
        }
        Console.WriteLine($"Beam processed, total splits {splits}");
        return splits;
    }

    public HashSet<int> ProcessMultiBeam(HashSet<int> beams, List<char> dataLine)
    {
        HashSet<int> newBeamCollection = new HashSet<int>();
        foreach (var beam in beams)
        {
            var isSplit = dataLine[beam] == '^';
            IEnumerable<int> newBeams = isSplit
                ? [beam + 1, beam - 1]
                : [beam];
            newBeamCollection.AddRange(newBeams);
            if(isSplit)
                splits++;
        }
        return newBeamCollection;
    }

    public long ProcessBeamTimelines(string input)
    {
        var data = DataParser.ParseDataIntoString(input);
        Dictionary<int, long> activeBeam = new() {
            { data.First().IndexOf('S'), 1 }
        };
        foreach (var dataLine in data.Skip(1))
        {
            activeBeam = ProcessTimelines(activeBeam, dataLine);
        }
        var timelines = activeBeam.Values.Sum();
        Console.WriteLine($"Beam processed, total timelines {timelines}");
        return timelines;
    }

    public Dictionary<int, long> ProcessTimelines(Dictionary<int, long> beamTimelines, List<char> dataLine)
    {
        Dictionary<int, long> newTimelines = new();
        foreach (var timeline in beamTimelines)
        {
            IEnumerable<int> newBeams = dataLine[timeline.Key] == '^'
                ? [timeline.Key + 1, timeline.Key - 1]
                : [timeline.Key];
            newTimelines.AddOrIncrement(newBeams, timeline.Value);
        }
        return newTimelines;
    }
}



