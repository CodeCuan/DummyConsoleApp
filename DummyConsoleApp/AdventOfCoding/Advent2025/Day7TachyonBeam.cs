using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day7TachyonBeam
{
    public bool LogLines = false;
    public int splits = 0;
    public void Main()
    {
        Console.WriteLine("Day 7 Tachyon Beam");
        Problem2();
    }

    public void Problem1()
    {
        ProcessMultiBeam(AdventData2025.Day7TachyonMap);
    }

    public void Problem2()
    {
        ProcessBeamTimelines(AdventData2025.Day7TachyonMap);
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
        HashSet<int> newBeams = new HashSet<int>();
        foreach (var beam in beams)
        {
            if (dataLine[beam] == '^')
            {
                newBeams.Add(beam + 1);
                newBeams.Add(beam - 1);
                splits++;
            }
            else
            {
                newBeams.Add(beam);
            }
        }
        return newBeams;
    }

    public long ProcessBeamTimelines(string input)
    {
        var data = DataParser.ParseDataIntoString(input);
        Dictionary<int, long> activeBeam = new Dictionary<int, long>();
        activeBeam[data.First().IndexOf('S')] = 1;
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
            if (dataLine[timeline.Key] == '^')
            {
                newTimelines.AddOrIncrement(timeline.Key + 1, timeline.Value);
                newTimelines.AddOrIncrement(timeline.Key - 1, timeline.Value);
                splits++;
            }
            else
            {
                newTimelines.AddOrIncrement(timeline.Key, timeline.Value);
            }
        }
        return newTimelines;
    }
}

public static class Extensions
{
    public static void AddOrIncrement(this Dictionary<int, long> myDictionary, int key, long incrementValue)
    {
        if (myDictionary.ContainsKey(key))
        {
            myDictionary[key] += incrementValue;
        }
        else
        {
            myDictionary[key] = incrementValue;
        }
    }
}

