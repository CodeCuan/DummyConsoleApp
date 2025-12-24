using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day06Lights
{
    public void Main()
    {
        Console.WriteLine("Day 6 Lights");
        var stoppy = Stopwatch.StartNew();
        var lights = CountLights(AdventData2015.Day6LightToggles, false);
        stoppy.Stop();
        Console.WriteLine($"Number of lights on: {lights} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var lightsAdvanced = CountLights(AdventData2015.Day6LightToggles, true);
        stoppy.Stop();
        Console.WriteLine($"Total brightness of lights: {lightsAdvanced} . Took {stoppy.ElapsedMilliseconds} ms");
    }

    public DefaultDictionary<Coordinate2D.CoordinateKey, bool> lights = new();
    public DefaultDictionary<Coordinate2D.CoordinateKey, int> lights2 = new();

    public int CountLights(string input, bool advanced)
    {
        var lightOperations = DataParser.SplitLines(input)
            .Select(line => new LightOperation(line))
            .ToList();
        lights = [];
        lights2 = [];
        foreach (var lightOperation in lightOperations)
        {
            if (advanced)
                lightOperation.PerformOperation(lights2);
            else
                lightOperation.PerformOperation(lights);
        }
        return advanced
            ? lights2.Values.Sum()
            : lights.Count(light => light.Value);
    }



    public class LightOperation
    {
        public LightOperation(string input)
        {
            var sections = input.Split(' ');
            var currentIndex = 0;
            if (sections[0] == "turn")
            {
                currentIndex++;
                Operation = sections[currentIndex] == "on" ? OperationType.TurnOn : OperationType.TurnOff;
            }
            else if (sections[0] == "toggle")
            {
                Operation = OperationType.Toggle;
            }
            currentIndex++;
            From = new Coordinate2D(sections[currentIndex]);
            currentIndex += 2;
            To = new Coordinate2D(sections[currentIndex]);
        }
        public Coordinate2D From;
        public Coordinate2D To;
        public OperationType Operation;
        public enum OperationType
        {
            TurnOn,
            TurnOff,
            Toggle
        }

        public override string ToString()
        {
            return $"{Operation} {From} - {To}";
        }

        internal void PerformOperation(DefaultDictionary<Coordinate2D.CoordinateKey, bool> lights)
        {
            foreach (var coordinate in GetCoordinateRange())
            {
                switch (Operation)
                {
                    case OperationType.TurnOn:
                        lights[coordinate] = true;
                        break;
                    case OperationType.TurnOff:
                        lights[coordinate] = false;
                        break;
                    case OperationType.Toggle:
                        lights[coordinate] = !lights[coordinate];
                        break;
                }
            }
        }

        internal void PerformOperation(DefaultDictionary<Coordinate2D.CoordinateKey, int> lights)
        {
            foreach (var coordinate in GetCoordinateRange())
            {
                switch (Operation)
                {
                    case OperationType.TurnOn:
                        lights[coordinate] += 1;
                        break;
                    case OperationType.TurnOff:
                        if(lights[coordinate] > 0)
                            lights[coordinate] -= 1;
                        break;
                    case OperationType.Toggle:
                        lights[coordinate] += 2;
                        break;
                }
            }
        }

        private IEnumerable<Coordinate2D.CoordinateKey> GetCoordinateRange()
        {
            var xMin = Math.Min(From.X, To.X);
            var xMax = Math.Max(From.X, To.X);
            var yMin = Math.Min(From.Y, To.Y);
            var yMax = Math.Max(From.Y, To.Y);
            for (long x = xMin; x <= xMax; x++)
            {
                for (long y = yMin; y <= yMax; y++)
                {
                    yield return new Coordinate2D.CoordinateKey(x, y);
                }
            }

        }
    }
}
