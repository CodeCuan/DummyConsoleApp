using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day03Routes
{
    public void Main()
    {
        Console.WriteLine("Day 3 Routes");
        var stoppy = Stopwatch.StartNew();
        var houses = CountHouses(AdventData2015.Day3Routes);
        stoppy.Stop();
        Console.WriteLine($"Houses receiving at least one present: {houses} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var roboHouses = CountHousesWithRobo(AdventData2015.Day3Routes);
        stoppy.Stop();
        Console.WriteLine($"Houses receiving at least one present with Robo-Santa: {roboHouses} . Took {stoppy.ElapsedMilliseconds} ms");
    }


    public int CountHouses(string input)
    {
        long x = 0;
        long y = 0;
        HashSet<Coordinate2D.CoordinateKey> coordinates = [new Coordinate2D.CoordinateKey(x, y)];
        foreach (var direction in input)
        {
            switch (direction)
            {
                case '>':
                    x++;
                    break;
                case '<':
                    x--;
                    break;
                case '^':
                    y++;
                    break;
                case 'v':
                    y--;
                    break;
                default:
                    throw new Exception("Invalid direction");
            }
            coordinates.Add(new Coordinate2D.CoordinateKey(x, y));
        }
        return coordinates.Count;
    }

    public int CountHousesWithRobo(string input)
    {
        Dictionary<bool, Coordinate2D> deliverers = new()
        {
            [true] = new Coordinate2D(0, 0),   // Santa
            [false] = new Coordinate2D(0, 0)   // Robo-Santa
        };
        HashSet<Coordinate2D.CoordinateKey> coordinates = [new Coordinate2D.CoordinateKey(0,0)];
        bool isSanta = true;
        foreach (var direction in input)
        {
            switch (direction)
            {
                case '>':
                    deliverers[isSanta].X++;
                    break;
                case '<':
                    deliverers[isSanta].X--;
                    break;
                case '^':
                    deliverers[isSanta].Y++;
                    break;
                case 'v':
                    deliverers[isSanta].Y--;
                    break;
                default:
                    throw new Exception("Invalid direction");
            }
            coordinates.Add(deliverers[isSanta].Key);
            isSanta = !isSanta;
        }
        return coordinates.Count;
    }
}
