using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day11ServerConnections
{
    public void Main()
    {
        var stoppy = Stopwatch.StartNew();
        var paths = GetPaths(AdventData2025.Day11ServerConnections);
        stoppy.Stop();
        Console.WriteLine($"Paths from me to out: {paths} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy = Stopwatch.StartNew();
        paths = GetPathsDacFft(AdventData2025.Day11ServerConnections);
        stoppy.Stop();
        Console.WriteLine($"Paths from svr to out with dac/fft: {paths} . Took {stoppy.ElapsedMilliseconds} ms");
    }

    public long GetPaths(string input)
    {
        var rack = new ServerRack(input);
        return rack.GetPathsToEnd();
    }

    public long GetPathsDacFft(string input)
    {
        var rack = new ServerRack(input);
        return rack.GetPathsToEndDacFft();

    }

    private class ServerRack
    {
        Dictionary<string, ServerSlot> slots = [];
        public ServerRack(string input)
        {
            var lines = DataParser.SplitDataLine(input);
            lines.Add("out:");
            slots = lines
                .Select(line => new ServerSlot(line))
                .ToDictionary(ss => ss.key, ss => ss);
            foreach (var slot in slots)
            {
                slot.Value.LinkChildren(slots);
            }
        }

        public long GetPathsToEndDacFft()
        {
            var startingSlot = slots["svr"];
            var dacSlot = slots["dac"];
            var fftSlot = slots["fft"];
            var toDac = GetPathsToDestination(startingSlot, "dac", []);
            var toFft = GetPathsToDestination(startingSlot, "fft", []);
            var dacToFft = GetPathsToDestination(dacSlot, "fft", []);
            var fftToDac = GetPathsToDestination(fftSlot, "dac", []);
            var dacToEnd = GetPathsToDestination(dacSlot, "out", []);
            var fftToEnd = GetPathsToDestination(fftSlot, "out", []);

            var totalPaths = toDac * dacToFft * fftToEnd
                + toFft * fftToDac * dacToEnd;
            return totalPaths;
        }

        public long GetPathsToEnd()
        {
            var startingSlot = slots["you"];
            Dictionary<string, long> pathsToEnd = [];
            return GetPathsToDestination(startingSlot, "out", pathsToEnd);
        }

        public long GetPathsToDestination(
            ServerSlot slot,
            string destination,
            Dictionary<string, long> pathsCache)
        {
            long paths = 0;
            foreach (var child in slot.children)
            {
                if (child.key == destination)
                {
                    paths += 1;
                    continue;
                }
                if (pathsCache.TryGetValue(child.key, out var pathCount))
                {
                    paths += pathCount;
                }
                else
                {
                    paths += GetPathsToDestination(child, destination, pathsCache);
                }
            }
            pathsCache[slot.key] = paths;
            return paths;
        }

    }
    private class ServerSlot
    {
        public string key;
        public List<string> childrenKeys;
        public List<ServerSlot> children = [];
        public override string ToString()
        {
            return $"{key}: {string.Join(", ", childrenKeys)}";
        }
        public ServerSlot(string input)
        {
            var sections = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            key = sections[0].TrimEnd(':');
            childrenKeys = sections.Skip(1).ToList();
        }

        internal void LinkChildren(Dictionary<string, ServerSlot> slots)
        {
            foreach (var childKey in childrenKeys)
            {
                this.children.Add(slots[childKey]);
            }
        }
    }
}
