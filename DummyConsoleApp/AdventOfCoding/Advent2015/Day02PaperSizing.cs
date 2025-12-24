using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day02PaperSizing
{
    public void Main()
    {
        Console.WriteLine("Day 2 Paper Sizing");
        InitPoints(AdventData2015.Day2PaperSizes);
        var sampleArea = GetSurfaceArea();
        Console.WriteLine($"Sample Total Surface Area: {sampleArea}");

        var ribbonLength = GetRibbonLength();
        Console.WriteLine($"Sample Total Ribbon Length: {ribbonLength}");
    }

    IList<Coordinate3D> points = [];

    public void InitPoints(string input) {
        points = DataParser
            .SplitLines(input)
            .Select(line => new Coordinate3D(line, 'x'))
            .ToList();
    }

    public long GetSurfaceArea()
    {
        return points.Sum(GetWrappingForShape);
    }

    public long GetRibbonLength()
    {
        return points.Sum(GetRibbonForShape);
    }

    public long GetWrappingForShape(Coordinate3D shape)
    {
        IList<long> sideAreas = [
            shape.X * shape.Y,
            shape.Y * shape.Z,
            shape.Z * shape.X
            ];
        var totalArea = 2 * sideAreas.Sum();
        var extraArea = sideAreas.Min();
        return totalArea + extraArea;
    }

    public long GetRibbonForShape(Coordinate3D shape)
    {
        IList<long> sideAreas = [
            shape.X + shape.Y,
            shape.Y + shape.Z,
            shape.Z + shape.X
            ];
        return 2 * sideAreas.Min() + shape.GetVolume();
    }
}
