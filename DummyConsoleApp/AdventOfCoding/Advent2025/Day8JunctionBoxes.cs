using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using System.Diagnostics;


namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day8JunctionBoxes
{
    List<Coordinate3D> Coordinates = [];
    public void Main()
    {
        Console.WriteLine("Day8JunctionBoxes");
        var stoppy = Stopwatch.StartNew();
        var circuitSize = CheckThreeLargestCircuits(AdventData2025.Day8JunctionBoxes, 1000);
        stoppy.Stop();
        Console.WriteLine($"Three largest circuits multiplied size: {circuitSize}. Solved in {stoppy.ElapsedMilliseconds} ms");
        stoppy = Stopwatch.StartNew();
        var wallDistance = CheckWallDistance(AdventData2025.Day8JunctionBoxes);
        stoppy.Stop();
        Console.WriteLine($"The wall distance product is: {wallDistance}. Solved in {stoppy.ElapsedMilliseconds} ms");

    }

    public int CheckThreeLargestCircuits(string input, int connectionCount)
    {
        Coordinates = DataParser.SplitDataLine(input)
            .Select(inputLine => new Coordinate3D(inputLine))
            .ToList();

        var closestNeighbours = GetClosestDistances();

        List<HashSet<Coordinate3D>> connectedCircuits = [];
        foreach (var connection in closestNeighbours.Take(connectionCount))
        { 
            AddCircuit(connection, connectedCircuits);
        }
        var circuits = connectedCircuits
            .OrderByDescending(circuit => circuit.Count)
            .Take(3)
            .ToList();
        return circuits.Aggregate(1, (product, circuit) => product * circuit.Count);
    }

    public long CheckWallDistance(string input)
    {
        Coordinates = DataParser.SplitDataLine(input)
            .Select(inputLine => new Coordinate3D(inputLine))
            .ToList();
        var closestNeighbours = GetClosestDistances();
        List<HashSet<Coordinate3D>> connectedCircuits = [];
        foreach (var connection in closestNeighbours)
        {

            AddCircuit(connection, connectedCircuits);
            Coordinates.Remove(connection.coord1);
            Coordinates.Remove(connection.coord2);
            if (Coordinates.Count == 0)
            {
                return connection.coord2.X * connection.coord1.X;
            }
        }
        throw new Exception("Could not connect all boxes");
    }

    private void AddCircuit(LinkedCoordinates3D coordinates, List<HashSet<Coordinate3D>> circuits)
    {
        var circuitOne = circuits.Where(circuit => circuit.Contains(coordinates.coord1)).FirstOrDefault();
        var circuitTwo = circuits.Where(circuit => circuit.Contains(coordinates.coord2)).FirstOrDefault();

        if (circuitOne != null
            && circuitTwo != null)
        {
            if (circuitOne == circuitTwo)
                return;
            circuits.Remove(circuitTwo);
            circuitOne.AddRange(circuitTwo);
        }

        if (circuitOne != null)
        {
            circuitOne.Add(coordinates.coord2);
            return;
        }
        if (circuitTwo != null)
        {
            circuitTwo.Add(coordinates.coord1);
            return;
        }

        circuits.Add([coordinates.coord1, coordinates.coord2]);
    }

    private List<LinkedCoordinates3D> GetClosestDistances()
    {
        List<LinkedCoordinates3D> links = [];
        List<Coordinate3D> unprocessedCoordinates = [.. Coordinates];
        foreach (var coordinate in Coordinates)
        {
            unprocessedCoordinates.Remove(coordinate);
            foreach (var neighbour in unprocessedCoordinates)
            {
                links.Add(
                        new LinkedCoordinates3D(coordinate, neighbour)
                    );
            }
        }
        return links.OrderBy(link => link.DistanceSquared).ToList();
    }


    private class LinkedCoordinates3D
    {
        public Coordinate3D coord1;
        public Coordinate3D coord2;

        public double DistanceSquared;
        public LinkedCoordinates3D(Coordinate3D coord1, Coordinate3D coord2) 
        {
            this.coord1 = coord1;
            this.coord2 = coord2;
            DistanceSquared = coord1.GetDistanceSquared(coord2);
        }
    }
}
