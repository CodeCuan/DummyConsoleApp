using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using System.Diagnostics;


namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day8JunctionBoxes
{
    List<LinkedCoordinate3D> Coordinates = [];
    public void Main()
    {
        Console.WriteLine("Day8JunctionBoxes");
        var stoppy = Stopwatch.StartNew();
        var circuitSize = CheckThreeLargestCircuits(AdventData2025.Day8JunctionBoxes, 10);
        stoppy.Stop();
        Console.WriteLine($"Three largest circuits multiplied size: {circuitSize}");
    }

    public int CheckThreeLargestCircuits(string input, int connectionCount)
    {
        Coordinates = DataParser.SplitDataLine(input)
            .Select(inputLine => new LinkedCoordinate3D(inputLine))
            .ToList();

        SetClosestNeighbours();
        var orderedCoords = Coordinates
            .OrderBy(coord => coord.ClosestDistanceSquared)
            .ToList();

        List<HashSet<LinkedCoordinate3D>> connectedCircuits = [];
        for (int i = 0; i < connectionCount; i++)
        {
            var coord = orderedCoords.First();
            coord.Connections.Add(
                coord.ClosestConnection
                ?? throw new Exception("Closest connection not set"));
            coord.ClosestConnection.Connections.Add(coord);
            orderedCoords.Remove(coord);
            if (coord.ClosestConnection.ClosestConnection == coord)
                orderedCoords.Remove(coord.ClosestConnection);

            if (!TryAddCircuit(coord, connectedCircuits))
                i--;
        }
        var circuits = connectedCircuits
            .OrderByDescending(circuit => circuit.Count)
            .Take(3)
            .ToList();
        return circuits.Aggregate(1, (product, circuit) => product * circuit.Count);
    }

    private bool TryAddCircuit(LinkedCoordinate3D coordinate, List<HashSet<LinkedCoordinate3D>> circuits)
    {
        var otherCoord = coordinate.ClosestConnection ?? throw new Exception("Closest connection not set");
        var circuitOne = circuits.Where(circuit => circuit.Contains(coordinate)).FirstOrDefault();
        var circuitTwo = circuits.Where(circuit => circuit.Contains(otherCoord)).FirstOrDefault();

        if (circuitOne != null
            && circuitTwo != null)
        {
            if (circuitOne == circuitTwo)
                return false;
            circuits.Remove(circuitTwo);
            circuitOne.AddRange(circuitTwo);
            return true;
        }
        if (circuitOne != null)
        { 
            circuitOne.Add(otherCoord);
            return true;
        }
        if (circuitTwo != null)
        { 
            circuitTwo.Add(coordinate);
            return true;
        }

        circuits.Add([coordinate, otherCoord]);
        return true;
    }

    private void SetClosestNeighbours()
    {
        foreach (var coordinate in Coordinates)
        {
            foreach (var otherCoordinate in Coordinates)
            {
                if (coordinate == otherCoordinate)
                    continue;
                var distanceSquared = coordinate.GetDistanceSquared(otherCoordinate);
                if (distanceSquared < coordinate.ClosestDistanceSquared)
                {
                    coordinate.ClosestDistanceSquared = distanceSquared;
                    coordinate.ClosestConnection = otherCoordinate;
                    coordinate.ClosestDistanceSquared = distanceSquared;
                }
            }
        }
    }

    private class LinkedCoordinate3D : Coordinate3D
    {
        public List<LinkedCoordinate3D> Connections = [];

        public LinkedCoordinate3D? ClosestConnection = null;

        public double ClosestDistanceSquared = double.MaxValue;
        public LinkedCoordinate3D(string input) : base(input)
        {
        }
    }
}
