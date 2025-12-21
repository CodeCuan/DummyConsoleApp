using DummyConsoleApp.AdventOfCoding.Advent2025;

namespace DummyConsoleApp.Test.Aoc2025
{
    public class AoC2025Day4Tests
    {
        public Day04PaperMapper sut = new();
        const string sampleInput = @"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.";

        [Fact]
        public void Day4Solution1Maps()
        {
            var result = sut.ParseRollsAvailable(sampleInput, false);
            Assert.Equal(13, result);
        }

        [Fact]
        public void Day4Solution2Maps()
        {
            var result = sut.ParseRollsAvailable(sampleInput, true);
            Assert.Equal(43, result);
        }
    }
}
