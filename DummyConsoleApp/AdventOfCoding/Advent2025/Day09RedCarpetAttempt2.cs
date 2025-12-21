using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using SkiaSharp;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day09RedCarpetAttempt2
{
    const string sampleData = @"7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3";
    const string mySample = @"0,50
5,50
5,55
10,55
10,60
15,60
15,64
20,64
20,67
25,67
25,70
30,70
30,73
35,73
35,75
40,75
40,78
45,78
45,79
45,80
50,80
50,79
55,79
55,78
60,78
60,76
65,76
65,74
70,74
70,71
75,71
75,69
80,69
80,65
85,65
85,61
90,61
90,54
95,54
95,50
100,50
65,50
75,50";
    public void Main()
    {
        long largestFullRectangle;
        Console.WriteLine("Day9RedCarpet");
        var stoppy = Stopwatch.StartNew();
        strokeWidth = 1;

        //largestFullRectangle = GetLargestRectangle(mySample, runLabel: "Sample");
         // Console.WriteLine($"sample rectangle size {largestFullRectangle}");
        strokeWidth = 10;
        //if (largestFullRectangle != 24 && false)
        //    throw new Exception("Largest rect was {largestFullRectangle}");
        scale = .1;
        bestRectangle = new Rectangle2D(new Coordinate2D("94523,48719"), new Coordinate2D("4733,32241"));
        largestFullRectangle = GetLargestRectangle(AdventData2025.Day9RedCarpets, false, runLabel: "big");
        stoppy.Stop();
        Console.WriteLine($"The largest full rectangle area is: {largestFullRectangle}. Solved in {stoppy.ElapsedMilliseconds} ms");
    }
    Rectangle2D? bestRectangle = null;

    List<Coordinate2D> redCarpets = [];
    SortedDictionary<long, List<Coordinate2D>> redCarpetByX = [];
    SortedDictionary<long, List<Coordinate2D>> redCarpetByY = [];
    SortedDictionary<long, LineVertical> verticalLines = [];
    SortedDictionary<long, LineHorizontal> horizontalLines = [];
    List<Shape> shapes = [];
    string runLabel = "";
    bool log = false;
    int strokeWidth = 0;
    double scale = 10;
    public long GetLargestRectangle(string input, bool log = true, string runLabel = "")
    {
        this.runLabel = runLabel;
        this.log = log;
        InitData(input);
        Console.WriteLine($"init data complete, processing lines into shapes");
        var rectangle = new Rectangle2D(new Coordinate2D("5991,68827"), new Coordinate2D("94523,50049"));
        OutputShape("Straight lines",
            horizontalLines.Values,
            verticalLines.Values,
            redCarpets,
            redRectangle: rectangle);

        ProcessLinesIntoShapes();
        OutputShapes();
        Console.WriteLine($"Lines processed into shapes, finding potential rectangles");
        var rectangles = GetRectangles();
        rectangles = rectangles.OrderByDescending(r => r.area).ToList();
        Console.WriteLine($"Rectangles processed - finding largest valid rectangle");
        var largestValidRectangle = FindValidRectangle(rectangles);

        return largestValidRectangle.area;
    }

    private Rectangle2D FindValidRectangle(List<Rectangle2D> rectangles)
    {
        foreach (var rect in rectangles)
        {
            foreach (var shape in shapes)
            {
                if (shape.ContainsRectangle(rect))
                {
                    DrawRectangleInShape(shape, rect);
                    return rect;
                }
                // Console.WriteLine($"Rectangle {rect} invalid");
            }
        }
        throw new Exception("No rectangles are in shapes");
    }

    private List<Rectangle2D> GetRectangles()
    {
        List<Coordinate2D> unusedCarpets = [.. redCarpets];
        List<Rectangle2D> rectangles = [];
        foreach (var redCarpet in redCarpets)
        {
            unusedCarpets.Remove(redCarpet);
            rectangles.AddRange(unusedCarpets.Select(unusedCarpet => new Rectangle2D(redCarpet, unusedCarpet)));
        }
        return rectangles;
    }

    private void ProcessLinesIntoShapes()
    {
        List<Shape> openShapes = [];
        shapes = [];
        var rangeStart = verticalLines.Keys.First();
        var rangeEnd = verticalLines.Keys.Last();
        var checkpointWidth = (rangeEnd - rangeStart) / 10;
        var nextCheckpoint = rangeStart + checkpointWidth;
        for (long xCoord = rangeStart; xCoord <= rangeEnd; xCoord++)
        {
            if (xCoord == nextCheckpoint)
            {
                Console.WriteLine($"Processed data {xCoord} / {rangeEnd}");
                nextCheckpoint += checkpointWidth;
            }
            if (verticalLines.TryGetValue(xCoord, out var verticalLine))
            {
                var lineIsProcessed = false;
                foreach (var openShape in openShapes)
                {
                    if (openShape.TryRegisterLine(verticalLine))
                        lineIsProcessed = true;
                }
                if (!lineIsProcessed)
                {
                    var newShape = new Shape();
                    newShape.RegisterFirstLine(verticalLine);
                    openShapes.Add(newShape);
                }
            }
            foreach (var hLine in horizontalLines)
            {
                if (hLine.Value.Contains(xCoord))
                {
                    foreach (var openShape in openShapes)
                    {
                        openShape.TryRegisterPoint(hLine.Value.Y);
                    }
                }
            }

            // join open shape 'wavefronts'
            for (int shapeIndex = 0; shapeIndex < openShapes.Count; shapeIndex++)
            {
                var baseShape = openShapes[shapeIndex];
                foreach (var comparisonShape in openShapes.Skip(shapeIndex + 1))
                {
                    comparisonShape.JoinWaveFront(baseShape);
                }
            }

            // close completed shapes 
            foreach (var shape in openShapes.ToList())
            {
                if (shape.ActiveLine != null)
                {
                    shape.RegisterActiveLine(openShapes);
                }
                else if (shape.ActiveLine == null)
                {
                    openShapes.Remove(shape);
                    shapes.Add(shape);
                }
            }
        }
        shapes.AddRange(openShapes);
    }

    private void InitData(string input)
    {
        redCarpetByX = [];
        redCarpetByY = [];
        verticalLines = [];
        horizontalLines = [];
        redCarpets = DataParser.SplitDataLine(input)
             .Select(inputLine => new Coordinate2D(inputLine))
             .ToList();

        foreach (var carpet in redCarpets)
        {
            redCarpetByX.AddToList(carpet.X, carpet);
            redCarpetByY.AddToList(carpet.Y, carpet);
        }

        foreach (var coordinateSet in redCarpetByX.Where(kv => kv.Value.Count > 1))
        {
            verticalLines[coordinateSet.Key] = new LineVertical(coordinateSet.Value);
        }
        foreach (var coordinateSet in redCarpetByY.Where(kv => kv.Value.Count > 1))
        {
            horizontalLines[coordinateSet.Key] = new LineHorizontal(coordinateSet.Value);
        }
    }



    #region private classes

    private class Rectangle2D
    {
        public long x1;
        public long y1;
        public long x2;
        public long y2;
        public long area;
        public Rectangle2D(Coordinate2D corner1, Coordinate2D corner2)
        {
            x1 = Math.Min(corner1.X, corner2.X);
            x2 = Math.Max(corner1.X, corner2.X);
            y1 = Math.Min(corner1.Y, corner2.Y);
            y2 = Math.Max(corner1.Y, corner2.Y);
            area = (x2 - x1 + 1) * (y2 - y1 + 1);
        }

        public IEnumerable<long> GetXCoords() => [x1, x2];
        public IEnumerable<long> GetYCoords() => [y1, y2];

        public override string ToString()
        {
            return $"{x1}-{x2}|{y1}-{y2} ({area})";
        }
    }

    private class Shape
    {
        public List<LineVertical> verticalLines = [];
        public Dictionary<long, LineVertical>? verticalLinesIndexed;
        public LineVertical? ActiveLine = null;
        public bool isOpen = true;
        public List<long> activePoints = new List<long>();
        public bool hasFlatLine = false;

        public Shape() { }
        public Shape(Shape shape, LineVertical newLine)
        {
            verticalLines = shape.verticalLines.ToList();
            verticalLines.Add(newLine);
        }

        public bool TryRegisterLine(LineVertical lineVertical)
        {
            var previousLine = verticalLines.Last();
            if (lineVertical.Overlaps(previousLine))
            {
                ActiveLine = new LineVertical(lineVertical);
                hasFlatLine = true;
                return true;
            }
            return false;
        }

        internal bool ContainsRectangle(Rectangle2D rect)
        {
            verticalLinesIndexed ??= verticalLines.ToDictionary(vl => vl.X, vl => vl);
            foreach (long x in rect.GetXCoords())
            {
                if (!verticalLinesIndexed.TryGetValue(x, out var xLine))
                    return false;
                if (!xLine.Contains(rect.y1) || !xLine.Contains(rect.y2))
                    return false;
            }
            Console.WriteLine($"Rect has valid corners: {rect}");

            for (long x = rect.x1; x <= rect.x2; x++)
            {
                var xLine = verticalLinesIndexed[x];
                if (!xLine.Contains(rect.y1) || !xLine.Contains(rect.y2))
                    return false;
            }
            return true;
        }

        internal bool JoinWaveFront(Shape baseShape)
        {
            if (ActiveLine == null || baseShape.ActiveLine == null)
                return false;
            if (ActiveLine.Overlaps(baseShape.ActiveLine))
            {
                ActiveLine.JoinLine(baseShape.ActiveLine);
                return true;
            }
            return false;
        }

        internal void RegisterActiveLine(List<Shape> openShapes)
        {
            if (ActiveLine == null)
                throw new Exception("Acitve line is null");
            if (activePoints.Count > 3 && !hasFlatLine)
            {
                activePoints.Sort();
                var line1 =
                ActiveLine = new LineVertical(ActiveLine.X, activePoints[0], activePoints[1]);
                var newLine = new LineVertical(ActiveLine.X, activePoints[2], activePoints[3]);
                openShapes.Add(new Shape(this, newLine));
            }
            verticalLines.Add(ActiveLine);
            ActiveLine = null;
            hasFlatLine = false;
            activePoints.Clear();
        }

        internal void RegisterFirstLine(LineVertical verticalLine)
        {
            ActiveLine = new LineVertical(verticalLine);

        }

        internal void TryRegisterPoint(long y)
        {
            if (verticalLines.Count == 0)
                return;
            var previousLine = verticalLines.Last();
            if (previousLine.Contains(y))
            {
                activePoints.Add(y);
                if (ActiveLine != null)
                {
                    ActiveLine.Extend(y);
                }
                else
                    ActiveLine = new LineVertical(previousLine.X + 1, y, y);
            }
        }
    }

    private class LineVertical
    {
        public long X { get; set; }
        public long YStart { get; set; }
        public long YEnd { get; set; }
        public LineVertical(long x, long yStart, long yEnd)
        {
            X = x;
            YStart = yStart;
            YEnd = yEnd;
        }
        public LineVertical(ICollection<Coordinate2D> coordinates)
        {
            X = coordinates.First().X;
            YStart = coordinates.Min(c => c.Y);
            YEnd = coordinates.Max(c => c.Y);
        }

        public LineVertical(LineVertical lineVertical)
        {
            X = lineVertical.X;
            YStart = lineVertical.YStart;
            YEnd = lineVertical.YEnd;
        }

        public override string ToString()
        {
            return $"{X}|{YStart}->{YEnd}";
        }

        public bool Overlaps(LineVertical previousLine)
        {
            if (YStart >= previousLine.YStart && YStart <= previousLine.YEnd)
                return true;
            if (YEnd >= previousLine.YStart && YEnd <= previousLine.YEnd)
                return true;
            if (previousLine.YStart >= YStart && previousLine.YStart <= YEnd)
                return true;
            return false;
        }

        public bool Contains(long yCoord)
        {
            return yCoord >= YStart && yCoord <= YEnd;
        }

        internal void Extend(long y)
        {
            YStart = Math.Min(YStart, y);
            YEnd = Math.Max(YEnd, y);
        }

        internal void JoinLine(LineVertical activeLine)
        {
            YStart = Math.Min(YStart, activeLine.YStart);
            YEnd = Math.Max(YEnd, activeLine.YEnd);
            activeLine.YStart = YStart;
            activeLine.YEnd = YEnd;
        }
    }

    private class LineHorizontal
    {
        public long Y { get; set; }
        public long XStart { get; set; }
        public long XEnd { get; set; }
        public LineHorizontal(ICollection<Coordinate2D> coordinates)
        {
            Y = coordinates.First().Y;
            XStart = coordinates.Min(c => c.X);
            XEnd = coordinates.Max(c => c.X);
        }
        public override string ToString()
        {
            return $"{Y}|{XStart}->{XEnd}";
        }

        public bool Contains(long xCoord)
        {
            return xCoord >= XStart && xCoord <= XEnd;
        }
    }

    #endregion

    #region visualisation
    private void DrawRectangleInShape(Shape shape, Rectangle2D rect)
    {
        Console.WriteLine($"Valid rectangle was found: {rect}");
        Console.WriteLine($"In shape: {shape.verticalLines.First()} - {shape.verticalLines.Last()}");
        OutputShape("RectangleInShape", [], shape.verticalLines, [], rect, logFull: true);
    }

    private void OutputShapes()
    {
        int counter = 0;
        foreach (var shape in shapes)
        {
            counter++;
            if (counter > 10)
            {
                Console.WriteLine($"First ten shapes drawn, quitting draw operations");
                return;
            }
            var shapeLabel = $"Shape_{counter}";
            OutputShape(
                shapeLabel,
                [],
                shape.verticalLines,
                redCarpets,
                redRectangle: bestRectangle);
        }
    }

    private void OutputShape(
          string label,
          IEnumerable<LineHorizontal> horizontalLines,
          IEnumerable<LineVertical> verticalLines,
          IEnumerable<Coordinate2D> points,
          Rectangle2D? redRectangle = null,
          bool logFull = false)
    {
        if (!log)
            return;
        var xMax = redCarpetByX.Keys.Max();
        var yMax = redCarpetByY.Keys.Max();
        var width = (int)((xMax + 1) * scale);
        var height = (int)((yMax + 1) * scale);
        Console.WriteLine($"Drawing {label} image");
        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.White);
        canvas.Scale((float)scale);

        // Green pen for drawing lines
        using var greenPaint = new SKPaint
        {
            Color = SKColors.Green,
            StrokeWidth = strokeWidth,
            IsAntialias = false,
            Style = SKPaintStyle.Stroke
        };

        Console.WriteLine("Drawing Horizontal Lines");
        foreach (var hLine in horizontalLines)
        {
            canvas.DrawLine(hLine.XStart, hLine.Y, hLine.XEnd, hLine.Y, greenPaint);
        }
        Console.WriteLine("Drawing Vertical Lines");
        foreach (var vLine in verticalLines)
        {
            canvas.DrawLine(vLine.X, vLine.YStart, vLine.X, vLine.YEnd, greenPaint);
        }
        using var redPaint = new SKPaint { Color = SKColors.Red };
        foreach (var point in points)
        {
            canvas.DrawPoint(point.X, point.Y, redPaint);
        }
        if (redRectangle != null)
        {
            var redLineWidth = 1;
            if (scale < 1)
                redLineWidth = 10;
            using var redPaintLine = new SKPaint
            {
                Color = SKColors.Red,
                StrokeWidth = redLineWidth,
                IsAntialias = false,
                Style = SKPaintStyle.Stroke
            };
            foreach (var xCoord in redRectangle.GetXCoords())
                canvas.DrawLine(xCoord, redRectangle.y1, xCoord, redRectangle.y2, redPaintLine);
            foreach (var yCoord in redRectangle.GetYCoords())
                canvas.DrawLine(redRectangle.x1, yCoord, redRectangle.x2, yCoord, redPaintLine);

        }

        // Save to PNG file
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tmp"));
        var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tmp", $"run_{DateTime.Now:dd-hh-mm-ss}_{runLabel}_{label}.png");

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(outputPath);
        data.SaveTo(stream);
    }

    #endregion
}
