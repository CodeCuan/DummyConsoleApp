using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day09TravellingSalesman
{
    public void Main()
    {
        Console.WriteLine("Day 9 Travelling Salesman");
        var stoppy = Stopwatch.StartNew();
        var minDistance = GetMinDistance(AdventData2015.Day9TravellingData, false);
        stoppy.Stop();
        Console.WriteLine($"Minimum distance to travel all countries: {minDistance} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var maxDistance = GetMinDistance(AdventData2015.Day9TravellingData, true);
        stoppy.Stop();
        Console.WriteLine($"Maximum distance to travel all countries: {maxDistance} . Took {stoppy.ElapsedMilliseconds} ms");

    }

    public int GetMinDistance(string input, bool useMax)
    {
        var nodeMap = InitNodes(input);
        var seedNode = nodeMap.First().Value;
        var chains = seedNode.GetChains([], seedNode);
        var bestChain = useMax
            ? chains.MaxBy(chain => GetChainLength(chain, useMax))
            : chains.MinBy(chain => GetChainLength(chain, useMax));
        ArgumentNullException.ThrowIfNull(bestChain);
        var bestDistance = GetChainLength(bestChain, useMax);
        return bestDistance;

    }

    private int GetChainLength(List<CountryConnection> chain, bool useMax)
    {
        var chainLength = chain.Sum(connection => connection.distance);
        return chainLength - (
            useMax
            ? chain.Min(connection => connection.distance)
            : chain.Max(connection => connection.distance));
    }
    private Dictionary<string, CountryNode> InitNodes(string input)
    {
        Dictionary<string, CountryNode> CountryNodes = [];

        foreach (var line in DataParser.SplitLines(input))
        {
            var countrySections = line.Split(' ');
            var c1Key = countrySections[0];
            var c2Key = countrySections[2];
            if (!CountryNodes.TryGetValue(c1Key, out var c1))
            {
                c1 = new CountryNode(c1Key);
                CountryNodes[c1Key] = c1;
            }
            if (!CountryNodes.TryGetValue(c2Key, out var c2))
            {
                c2 = new CountryNode(c2Key);
                CountryNodes[c2Key] = c2;
            }
            c1.Connections[c2] = int.Parse(countrySections[4]);
            c2.Connections[c1] = int.Parse(countrySections[4]);
        }
        return CountryNodes;
    }


    public class CountryNode
    {
        public Dictionary<CountryNode, int> Connections = [];
        public string Name;
        public CountryNode(string name)
        {
            Name = name;
        }

        public List<List<CountryConnection>> GetChains(HashSet<string> processedNodes, CountryNode seedNode)
        {
            processedNodes.Add(Name);
            List<List<CountryConnection>> allChains = [];
            foreach (var connection in Connections
                .Where(Connections => !processedNodes.Contains(Connections.Key.Name)))
            {
                HashSet<string> subChainSet = [.. processedNodes];
                var subChains = connection.Key.GetChains(subChainSet, seedNode);
                foreach (var subChain in subChains)
                {
                    subChain.Insert(0, new CountryConnection(connection.Key, connection.Value));
                }
                allChains.AddRange(subChains);
            }
            if (allChains.Count == 0)
            {
                allChains.Add([new CountryConnection(seedNode, Connections[seedNode])]);
            }
            return allChains;
        }
    }

    public class CountryConnection
    {
        CountryNode nextCountry;
        public int distance;
        public CountryConnection(CountryNode country, int dist)
        {
            nextCountry = country;
            distance = dist;
        }
        public override string ToString()
        {
            return $"{nextCountry.Name}|{distance}";
        }
    }
}
