using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.Diagnostics;
using static DummyConsoleApp.AdventOfCoding.Utilities.DataStructures.Coordinate2D;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day9RedCarpet
{
    public void Main()
    {
        Console.WriteLine("Day9RedCarpet");
        var stoppy = Stopwatch.StartNew();
        var largestRectangle = GetLargestRectangle(AdventData2025.Day9RedCarpets);
        stoppy.Stop();
        Console.WriteLine($"The largest rectangle area is: {largestRectangle}. Solved in {stoppy.ElapsedMilliseconds} ms");
        stoppy = Stopwatch.StartNew();
        var largestFullRectangle = GetLargestFullRectangle(@"7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3
50,50");

        largestFullRectangle = GetLargestFullRectangle(AdventData2025.Day9RedCarpets, false);
        stoppy.Stop();
        Console.WriteLine($"The largest full rectangle area is: {largestFullRectangle}. Solved in {stoppy.ElapsedMilliseconds} ms");

    }

    public long GetLargestRectangle(string input)
    {
        var rectangles = GetRectangles(input);
        var biggestRectangle = rectangles.MaxBy(rectangle => rectangle.Area)
            ?? throw new Exception("There are no rectangles");
        return biggestRectangle.Area;
    }

    public long GetLargestFullRectangle(string input, bool paint = true)
    {
        var rectangles = GetRectangles(input).OrderByDescending(rect => rect.Area).ToList();
        Dictionary<Coordinate2D.CoordinateKey, CarpetCoordinate2D> coordinateMap = new();
        foreach (var rectangle in rectangles)
            rectangle.PaintCoordinates(coordinateMap);
        Paint(coordinateMap, paint);
        var bigRectangle = FindBiggestFullRectangle(rectangles, coordinateMap);
        return bigRectangle.Area;
    }

    private Rectangle2D FindBiggestFullRectangle(List<Rectangle2D> rectangles, Dictionary<CoordinateKey, CarpetCoordinate2D> coordinateMap)
    {
        foreach (var rectangle in rectangles)
        {
            if (rectangle.IsValidRectangle(coordinateMap))
            {
                return rectangle;
            }
        }
        throw new Exception("No valid rectangle found");
    }

    private void FillGreenSquares(Dictionary<Coordinate2D.CoordinateKey, CarpetCoordinate2D> coordinateMap)
    {
        for (long x = coordinateMap.Values.Min(x => x.X); x <= coordinateMap.Values.Max(x => x.X); x++)
        {
            List<CarpetCoordinate2D> potentialGreens = [];
            bool startTracking = false;
            for (long y = coordinateMap.Values.Min(y => y.Y); y <= coordinateMap.Values.Max(y => y.Y); y++)
            {
                if (!coordinateMap.TryGetValue(new Coordinate2D.CoordinateKey(x, y), out var coordinate))
                {
                    if (!startTracking)
                        continue;
                    coordinate = new CarpetCoordinate2D(x, y);
                    coordinateMap[coordinate.Key] = coordinate;
                }
                if (!coordinate.IsNone)
                {
                    startTracking = true;
                    foreach (var coord in potentialGreens.Where(c => c.IsNone))
                    {
                        coord.IsGreen = true;
                    }
                    continue;
                }
                if (startTracking)
                    potentialGreens.Add(coordinate);
            }
        }
    }

    private List<Rectangle2D> GetRectangles(string input)
    {
        var redCarpets = DataParser.SplitDataLine(input)
         .Select(inputLine => new Coordinate2D(inputLine))
         .ToList();
        List<Coordinate2D> unusedCarpets = [.. redCarpets];
        List<Rectangle2D> rectangles = [];
        foreach (var redCarpet in redCarpets)
        {
            unusedCarpets.Remove(redCarpet);
            rectangles.AddRange(unusedCarpets.Select(unusedCarpets => new Rectangle2D(redCarpet, unusedCarpets)));
        }
        return rectangles;
    }

    public void Paint(Dictionary<Coordinate2D.CoordinateKey, CarpetCoordinate2D> coordinateMap, bool paint)
    {
        if (!paint)
            return;
        for (long y = coordinateMap.Values.Min(y => y.Y); y <= coordinateMap.Values.Max(y => y.Y); y++)
        {
            string line = "";
            for (long x = coordinateMap.Values.Min(x => x.X); x <= coordinateMap.Values.Max(x => x.X); x++)
            {

                var coordinateKey = new Coordinate2D.CoordinateKey(x, y);
                if (!coordinateMap.TryGetValue(coordinateKey, out var coordinate) || coordinate.IsNone)
                {
                    line += ".";
                    continue;
                }
                if (coordinate.IsRed)
                {
                    line += "#";
                }
                else if (coordinate.IsGreen)
                {
                    line += "X";
                }
            }
            Console.WriteLine(line);
        }
    }

    public class Rectangle2D
    {
        public Coordinate2D Corner1;
        public Coordinate2D Corner2;
        public long Area;
        public Rectangle2D(Coordinate2D corner1, Coordinate2D corner2)
        {
            Corner1 = corner1;
            Corner2 = corner2;
            Area = corner1.GetOffsetArea(corner2);
        }

        public void PaintCoordinates(Dictionary<Coordinate2D.CoordinateKey, CarpetCoordinate2D> coordinateMap)
        {
            foreach (var coordinateKey in GetPartialBorder())
            {
                if (!coordinateMap.TryGetValue(coordinateKey, out var coordinate))
                {
                    coordinate = new CarpetCoordinate2D(coordinateKey.X, coordinateKey.Y);
                    coordinateMap[coordinateKey] = coordinate;
                }
                if (!coordinate.IsRed)
                    if (coordinate.Key == Corner1.Key || coordinate.Key == Corner2.Key)
                        coordinate.IsRed = true;
                    else
                    {
                        coordinate.IsGreen = true;
                    }
            }

        }

        public bool IsValidRectangle(Dictionary<CoordinateKey, CarpetCoordinate2D> coordinateMap)
        {
            if (Corner1.X == Corner2.X
                || Corner1.Y == Corner2.Y)
                return true;
            var point1Direction = GetCornerDirectionPoint1();
            switch (point1Direction)
            {
                case CornerDirection.DownLeft:
                    var tmpCornerDL = Corner2;
                    Corner2 = Corner1;
                    Corner1 = tmpCornerDL;
                    point1Direction = CornerDirection.UpRight;
                    break;
                case CornerDirection.DownRight:
                    var tmpCornerDR = Corner2;
                    Corner2 = Corner1;
                    Corner1 = tmpCornerDR;
                    point1Direction = CornerDirection.UpLeft;
                    break;
            }
            switch (point1Direction)
            {
                case CornerDirection.UpLeft:
                    return IsValidInDirection(coordinateMap, Corner2.X, Corner1.Y, CardinalDirection.Right)
                        && IsValidInDirection(coordinateMap, Corner1.X, Corner2.Y, CardinalDirection.Down)
                        && IsValidInDirection(coordinateMap, Corner1.X, Corner2.Y, CardinalDirection.Left)
                        && IsValidInDirection(coordinateMap, Corner2.X, Corner1.Y, CardinalDirection.Up);
                case CornerDirection.UpRight:
                    return IsValidInDirection(coordinateMap, Corner1.X, Corner2.Y, CardinalDirection.Right)
                        && IsValidInDirection(coordinateMap, Corner1.X, Corner2.Y, CardinalDirection.Down)
                        && IsValidInDirection(coordinateMap, Corner2.X, Corner1.Y, CardinalDirection.Left)
                        && IsValidInDirection(coordinateMap, Corner2.X, Corner1.Y, CardinalDirection.Up);
                default:
                    throw new Exception("I messed up");
            }
        }

        private bool IsValidInDirection(Dictionary<CoordinateKey, CarpetCoordinate2D> coordinateMap, long x, long y, CardinalDirection cardinalDirection)
        {
            switch (cardinalDirection)
            {
                case CardinalDirection.Up:
                    return coordinateMap.Keys.Any(coord =>
                        coord.X == x && coord.Y <= y
                    );
                case CardinalDirection.Down:
                    return coordinateMap.Keys.Any(coord =>
                        coord.X == x && coord.Y >= y
                    );
                case CardinalDirection.Left:
                    return coordinateMap.Keys.Any(coord =>
                        coord.X <= x && coord.Y == y
                    );

                case CardinalDirection.Right:
                    return coordinateMap.Keys.Any(coord =>
                        coord.X >= x && coord.Y == y
                    );
                default:
                    throw new Exception($"I messed up - {cardinalDirection}");
            }
        }

        private CornerDirection GetCornerDirectionPoint1()
        {
            if (Corner1.X < Corner2.X)
                if (Corner1.Y < Corner2.Y)
                    return CornerDirection.UpLeft;
                else
                    return CornerDirection.DownLeft;
            else
                if (Corner1.Y < Corner2.Y)
                return CornerDirection.UpRight;
            else
                return CornerDirection.DownRight;
        }

        private IEnumerable<Coordinate2D.CoordinateKey> GetPartialBorder()
        {
            var allBorder = Corner1.X == Corner2.X
                || Corner1.Y == Corner2.Y;
            if (allBorder)
                for (long x = Math.Min(Corner1.X, Corner2.X); x <= Math.Max(Corner1.X, Corner2.X); x++)
                {
                    for (long y = Math.Min(Corner1.Y, Corner2.Y); y <= Math.Max(Corner1.Y, Corner2.Y); y++)
                    {
                        yield return new Coordinate2D.CoordinateKey(x, y);
                    }
                }
            yield return Corner1.Key;
            yield return Corner2.Key;
        }
    }

    public class CarpetCoordinate2D : Coordinate2D
    {
        public bool IsRed = false;
        public bool IsGreen = false;
        public bool IsNone { get { return !IsRed && !IsGreen; } }
        public CarpetCoordinate2D(long x, long y) : base(x, y)
        {
        }
    }
    public enum CornerDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public enum CardinalDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
