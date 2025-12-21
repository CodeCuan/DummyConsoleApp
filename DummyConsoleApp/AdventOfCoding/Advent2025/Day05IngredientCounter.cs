using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025
{
    public class Day05IngredientCounter
    {
        public void Main()
        {
            Console.WriteLine("Day 5 Ingredient Counter");
            CountIngredients(AdventData2025.Day5Ranges, AdventData2025.Day5Ingredients);
            CountRanges(AdventData2025.Day5Ranges);
        }
        public int CountIngredients(string rangesInput, string ingredientsInput)
        {
            var ranges = DataParser.SplitDataLine(rangesInput);
            var ingredients = DataParser.SplitDataLineToLong(ingredientsInput);
            var rangesList = new List<Range>();
            foreach (var range in ranges)
            {
                var r = new Range(range);
                rangesList.Add(r);
            }
            var freshIngredients = ingredients.Where(ingredient => rangesList.Any(r => r.InRange(ingredient)));
            var freshCount = freshIngredients.Count();
            Console.WriteLine($"Fresh Ingredients Count: {freshCount}");
            return freshCount;
        }

        public long CountRanges(string rangesInput)
        {
            var inputs = DataParser.SplitDataLine(rangesInput);
            List<Range> ranges = new List<Range>();
            foreach (var input in inputs)
            {
                var range = new Range(input);
                ProcessRanges(ranges, range);
                if (!range.deadRange)
                {
                    ranges.Add(range);
                }
            }
            var totalCount = ranges.Sum(r => r.Count);
            Console.WriteLine($"Fresh Ingredients Range Count: {totalCount}");
            totalCount = ranges.Where(r => !r.deadRange).Sum(r => r.Count);
            Console.WriteLine($"Fresh Ingredients Range Count: {totalCount}");
            return totalCount;
        }

        private void ProcessRanges(IEnumerable<Range> ranges, Range newRange)
        {
            foreach (var existingRange in ranges)
            {
                existingRange.RemoveOverlap(newRange);
                if (newRange.deadRange)
                    return;
            }
        }

        public class Range
        {
            public override string ToString()
            {
                return $"{min}-{max}";
            }
            public bool deadRange = false;
            public long min;
            public long max;
            public long Count => max - min + 1;
            public bool InRange(long value)
            {
                return value >= min && value <= max;
            }

            public Range(string input)
            {
                var parts = input.Split('-');
                min = long.Parse(parts[0]);
                max = long.Parse(parts[1]);
            }

            public bool Contains(Range range)
            {
                return InRange(range.min) && InRange(range.max);
            }

            public void RemoveOverlap(Range range)
            {
                var containsMin = InRange(range.min);
                var containsMax = InRange(range.max);
                if (containsMin && containsMax)
                {
                    range.deadRange = true;
                }
                else if (containsMin)
                {
                    range.min = max + 1;
                }
                else if (containsMax)
                {
                    range.max = min - 1;
                }
                else if (range.Contains(this))
                {
                    deadRange = true;
                }
                if (range.min > range.max)
                {
                    range.deadRange = true;
                }
            }
        }
    }
}
