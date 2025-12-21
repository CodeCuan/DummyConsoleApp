using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day12PuzzleForms
{
    const int shapes = 6;

    public void Main()
    {
        Console.WriteLine("Day 12 Puzzle Forms");
        var sample = CountRegions(AdventData2025.Day12SampleData);
        Console.WriteLine($"Valid Sample Regions: {sample}");
        var actual = CountRegions(AdventData2025.Day12PuzzleData);
        Console.WriteLine($"Valid actual Regions: {actual}");

    }

    public int CountRegions(string input)
    {
        List<PuzzleShape> puzzleShapes = [];
        var data = DataParser.SplitDataLine(input);
        var treeRegions = data.Skip(shapes * 4).Select(line => new PuzzleRegion(line)).ToList();
        for (int shapeCount = 0; shapeCount < shapes; shapeCount++) {
            puzzleShapes.Add(new PuzzleShape(data.Skip(4 * shapeCount + 1).Take(3)));
        }

        int validRegions = 0;
        foreach (var region in treeRegions) {
            if(region.IsValid(puzzleShapes))
            {
                validRegions++;
            }
        }

        return validRegions;
    }

    public class PuzzleShape {
        public int populatedCount = 0;
        public int gapCount = 0;
        public List<string> puzzleShape = [];
        public PuzzleShape(IEnumerable<string> inputs) {
            foreach (var input in inputs)
            {
                populatedCount += input.Count(c => c == '#');
                gapCount += input.Count(c => c == '.');
                puzzleShape.Add(input);
            }
        }
        public override string ToString()
        {
            return string.Join("|", puzzleShape);
        }

    }

    public class PuzzleRegion {
        public int x;
        public int y;
        public List<int> puzzleRequirements = [];
        public PuzzleRegion(string input) {
            var sections = input.Split([' ', ':', 'x'], StringSplitOptions.RemoveEmptyEntries).ToList();
            x = int.Parse(sections[0]);
            y = int.Parse(sections[1]);
            puzzleRequirements = sections.Skip(2).Select(int.Parse).ToList();
        }

        internal bool IsValid(List<PuzzleShape> puzzleShapes)
        {
            var xs = x / 3;
            var ys = y / 3;
            var easyCount = xs * ys;

            if (puzzleRequirements.Sum() < easyCount)
                return true;
            int totalReqs = 0;
            for(int pi = 0; pi < puzzleRequirements.Count; pi++)
            {
                var shape = puzzleShapes[pi];
                var required = puzzleRequirements[pi];
                totalReqs += shape.populatedCount * required;
            }
            if (totalReqs > x * y)
                return false;
            Console.WriteLine($"Unable to verify {x}-{y}: {string.Join(' ', puzzleRequirements)}");
            return true;
        }
    }
}
