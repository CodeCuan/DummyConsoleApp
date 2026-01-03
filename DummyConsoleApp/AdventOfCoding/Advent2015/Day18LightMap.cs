using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.ComponentModel.Design;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day18LightMap
{
    public void Main()
    {
        log = true;
        Console.WriteLine($"Sample has {CountLights(@".#.#.#
...##.
#....#
..#...
#.#..#
####..
", 5, true)}");
        log = false;
        Console.WriteLine("Day 18 Light Map");
        var lightCount = CountLights(AdventData2015.Day18LightMap);
        Console.WriteLine($"After cycles there are {lightCount} lights on");

        var lightCountXCorners = CountLights(AdventData2015.Day18LightMap, forceCorners: true);
        Console.WriteLine($"After cycles sans corners there are {lightCountXCorners} lights on");

    }
    private bool log = false;

    public int CountLights(string input, int cycleCount = 100, bool forceCorners = false)
    {
        ParseMap(input, forceCorners);
        for (int i = 0; i < cycleCount; i++)
        {
            LogMap();
            CycleMap(forceCorners);
        }
        return LightMap.Values.Count(v => v);
    }

    private void LogMap()
    {
        if (!log)
            return;
        var xMax = LightMap.Max(m => m.Key.X);
        var yMax = LightMap.Max(m => m.Key.Y);
        for (int y = 0; y <= yMax; y++)
        {
            for (int x = 0; x <= xMax; x++)
            {
                var key = new Coordinate2D.CoordinateKey(x, y);
                Console.Write(LightMap[key] ? '#' : '.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();

    }

    private void CycleMap(bool forceCorners)
    {
        var lightMap = new DefaultDictionary<Coordinate2D.CoordinateKey, bool>();
        foreach (var light in LightMap.ToList())
        {
            if(forceCorners
                && corners.Contains(light.Key))
            {
                lightMap[light.Key] = true;
                continue;
            }

            var neighbours = CountNeighbours(light.Key);
            if (light.Value && neighbours == 2
                || neighbours == 3)
            {
                lightMap[light.Key] = true;
            }
            else
            {
                lightMap[light.Key] = false;
            }
        }
        LightMap = lightMap;
    }

    private int CountNeighbours(Coordinate2D.CoordinateKey key)
    {
        int neighbours = 0;
        for (long x = key.X - 1; x <= key.X + 1; x++)
        {
            for (long y = key.Y - 1; y <= key.Y + 1; y++)
            {
                if (x == key.X && y == key.Y)
                    continue;
                var neighbourKey = new Coordinate2D.CoordinateKey(x, y);
                if (LightMap[neighbourKey])
                {
                    neighbours++;
                }
            }
        }
        return neighbours;
    }

    public void ParseMap(string input, bool setCorners)
    {
        LightMap = [];
        corners = [];
        LightMap.SetOnGet = false;
        var y = 0;
        foreach (var line in DataParser.SplitLines(input))
        {
            var x = 0;
            foreach (var lightChar in line)
            {
                LightMap[new Coordinate2D.CoordinateKey(x, y)] = (lightChar == '#');
                x++;
            }
            maxX = x-1;
            y++;
        }
        maxY = y-1;
        corners.Add(new(0, 0));
        corners.Add(new(0, maxY));
        corners.Add(new(maxX, 0));
        corners.Add(new(maxX, maxY));
        if(setCorners)
            foreach(var corner in corners)
                LightMap[corner] = true;
    }

    public DefaultDictionary<Coordinate2D.CoordinateKey, bool> LightMap = [];
    public long maxX;
    public long maxY;
    public HashSet<Coordinate2D.CoordinateKey> corners = [];
}
