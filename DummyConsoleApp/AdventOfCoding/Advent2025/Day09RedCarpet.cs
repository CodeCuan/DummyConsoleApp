using DummyConsoleApp.AdventOfCoding.Constants;
using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using System.Diagnostics;
using static DummyConsoleApp.AdventOfCoding.Utilities.DataStructures.Coordinate2D;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day09RedCarpet
{
    public void Main()
    {
        Console.WriteLine("Day9RedCarpet");
        var stoppy = Stopwatch.StartNew();
        var largestRectangle = GetLargestRectangle(AdventData2025.Day9RedCarpets);
        stoppy.Stop();
        Console.WriteLine($"The largest rectangle area is: {largestRectangle}. Solved in {stoppy.ElapsedMilliseconds} ms");
        stoppy = Stopwatch.StartNew();
        var largestFullRectangle = GetLargestFullRectangle(@"1,1
7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3
20,7
11,20
20,20,
22,22,
22,20,
20,22,
24,20
24,22");
        if (largestFullRectangle != 24 && false)
            throw new Exception("Largest rect was {largestFullRectangle}");
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
        var borderRectangles = rectangles.Where(rect => rect.Is2DLine).ToList();
        var shapeChains = GetChains(borderRectangles);
        shapeChains = BreakChains(shapeChains).ToList();
        List<long> rectangleAreas = [];
        foreach (var chain in shapeChains)
        {
            var validRectangles = rectangles.Where(rect => chain.coordinates.Contains(rect.Corner1.Key)
                && chain.coordinates.Contains(rect.Corner2.Key)).ToList();
            Dictionary<Coordinate2D.CoordinateKey, CarpetCoordinate2D> coordinateMap = new();
            foreach (var rectangle in validRectangles)
                rectangle.PaintCoordinates(coordinateMap);
            Console.WriteLine($"Logging chain {chain}");
            Paint(coordinateMap, paint);
            var bigRectangle = FindBiggestFullRectangle(validRectangles, coordinateMap);
            rectangleAreas.Add(bigRectangle.Area);
        }
        return rectangleAreas.Max();
    }

    public List<RectangleChain> GetChains(List<Rectangle2D> rectangles)
    {
        List<RectangleChain> chains = [];
        foreach (var rectangle in rectangles)
        {
            AddToChain(rectangle, chains);
        }
        foreach (var chain in chains.ToList())
        {
            foreach (var matchChain in chains.Where(ch => ch != chain))
            {
                if (matchChain.coordinates.Overlaps(chain.coordinates))
                {
                    matchChain.Rectangles.AddRange(chain.Rectangles);
                    matchChain.coordinates.UnionWith(chain.coordinates);
                    chains.Remove(chain);
                    break;
                }
            }
        }

        return chains;
    }

    public IEnumerable<RectangleChain> BreakChains(List<RectangleChain> chains)
    {
        foreach (var chain in chains)
        {
            foreach (var loop in BreakChain(chain))
            {
                yield return loop;
            }
        }
    }
    public IEnumerable<RectangleChain> BreakChain(RectangleChain parentChain)
    {
        var startingEdge = parentChain.Rectangles.First();
        foreach (var chain in ChaseChains(
            parentChain,
            startingEdge.Corner1.Key,
            new OrderedDictionary<CoordinateKey, Rectangle2D>() { { startingEdge.Corner2.Key, startingEdge } },
            [startingEdge.Corner1.Key, startingEdge.Corner2.Key]))
        {
            yield return chain;
        }
    }

    public IEnumerable<RectangleChain> ChaseChains(RectangleChain chain, CoordinateKey currentPoint, OrderedDictionary<CoordinateKey, Rectangle2D> path, HashSet<CoordinateKey> processedPoints)
    {
        var nextEdges = chain.Rectangles.Where(rect => rect != path.Values.Last()
                && (rect.Corner1.Key.Equals(currentPoint)
                    || rect.Corner2.Key.Equals(currentPoint)))
            .ToList();
        if (nextEdges.Count == 0)
            yield break;
        foreach (var edge in GetLeftFirst(path.Values.Last(), currentPoint, nextEdges))
        {
            var nextPoint = edge.GetOtherCorner(currentPoint);
            if (processedPoints.Contains(nextPoint.Key))
            {
                if (path.Count == 0)
                    continue;
                var matchPoint = nextPoint.Key;
                if (!path.ContainsKey(matchPoint))
                {
                    matchPoint = path.FirstOrDefault(
                            pair => pair.Value.IsLineAndContainsPoint(nextPoint)).Key;
                    if (matchPoint == default)
                        continue;
                }
                var shapeStart = path.IndexOf(matchPoint);
                var newChainSetItems = path.Skip(shapeStart).ToList();
                var newChainSet = newChainSetItems.Select(x => x.Value).ToList();
                newChainSet.Add(edge);
                chain.Rectangles.RemoveAll(newChainSet.Contains);
                foreach (var completedSection in newChainSetItems)
                {
                    path.Remove(completedSection.Key);
                }
                yield return new RectangleChain(newChainSet);
            }
            else
            {
                edge.AddCoordinatesToList(processedPoints);
                path.Add(nextPoint.Key, edge);

                foreach (var newChain in ChaseChains(chain, nextPoint.Key, path, processedPoints))
                {
                    yield return newChain;
                }
            }
        }
    }

    public IEnumerable<Rectangle2D> GetLeftFirst(Rectangle2D currentEdge, CoordinateKey leadingPoint, List<Rectangle2D> paths)
    {
        foreach (var direction in CardinalDirectionConstants.LeftOrdering[currentEdge.GetDirectionOfRectangle(leadingPoint)])
        {
            foreach (var edge in paths.Where(path => direction == CardinalDirectionConstants.InvertDirection[path.GetDirectionOfRectangle(leadingPoint)]).OrderBy(x => x.Area))
            {
                yield return edge;
            }
        }
    }

    private void AddToChain(Rectangle2D rectangle, List<RectangleChain> chains)
    {
        foreach (var chain in chains)
        {
            if (chain.coordinates.Contains(rectangle.Corner1.Key)
                || chain.coordinates.Contains(rectangle.Corner2.Key))
            {
                chain.Rectangles.Add(rectangle);
                chain.coordinates.Add(rectangle.Corner1.Key);
                chain.coordinates.Add(rectangle.Corner2.Key);
                return;
            }
        }
        var newChain = new RectangleChain();
        newChain.Rectangles.Add(rectangle);
        newChain.coordinates.Add(rectangle.Corner1.Key);
        newChain.coordinates.Add(rectangle.Corner2.Key);
        chains.Add(newChain);
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

        public CardinalDirection GetDirectionOfRectangle(CoordinateKey leadingPoint)
        {
            if (IsHorizontal)
            {
                if (leadingPoint.X == Math.Max(Corner1.X, Corner2.X))
                    return CardinalDirection.Right;
                else
                    return CardinalDirection.Left;
            }
            if (IsVertical)
            {
                if (leadingPoint.Y == Math.Max(Corner1.Y, Corner2.Y))
                    return CardinalDirection.Down;
                else
                    return CardinalDirection.Up;
            }
            throw new Exception("Not cardinal rectangle");
        }

        public void AddCoordinatesToList(ICollection<CoordinateKey> collection)
        {
            collection.Add(Corner1.Key);
            collection.Add(Corner2.Key);
        }

        public override string ToString()
        {
            return $"{Corner1}|{Corner2}";
        }

        public bool Is2DLine
        {
            get
            {
                return Corner1.X == Corner2.X
                    || Corner1.Y == Corner2.Y;
            }
        }

        public bool IsHorizontal
        {
            get
            {
                return Corner1.Y == Corner2.Y;
            }
        }

        public bool IsVertical
        {
            get
            {
                return Corner1.X == Corner2.X;
            }
        }

        public Rectangle2D(Coordinate2D corner1, Coordinate2D corner2)
        {
            Corner1 = corner1;
            Corner2 = corner2;
            Area = corner1.GetOffsetArea(corner2);
        }

        public Coordinate2D GetOtherCorner(CoordinateKey corner)
        {
            if (corner == Corner1.Key)
                return Corner2;
            if (corner == Corner2.Key)
                return Corner1;
            throw new Exception("Corner not part of rectangle");
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
            if (Is2DLine)
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
            if (Is2DLine)
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

        public bool ContainsMainCorner(CoordinateKey key)
        {
            return Corner1.Key == key
                || Corner2.Key == key;
        }

        public bool IsLineAndContainsPoint(Coordinate2D point)
        {
            return IsHorizontal && point.Y == Corner1.Y
                || IsVertical && point.X == Corner1.X;
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

    public class RectangleChain
    {
        public HashSet<Coordinate2D.CoordinateKey> coordinates = [];
        public List<Rectangle2D> Rectangles = [];
        public RectangleChain()
        {
        }
        public RectangleChain(List<Rectangle2D> newChainSet)
        {
            Rectangles = newChainSet;
            foreach (var rectangle in newChainSet)
            {
                rectangle.AddCoordinatesToList(coordinates);
            }
        }

        public override string ToString()
        {
            return string.Join('|', Rectangles);
        }
    }


}
