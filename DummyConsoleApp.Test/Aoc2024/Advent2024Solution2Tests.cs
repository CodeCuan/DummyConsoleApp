using DummyConsoleApp.AdventOfCoding.Advent2024;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.Test.Aoc2024
{
    public class Advent2024Solution2Tests
    {
        public Advent2024Solution2 sut = new Advent2024Solution2();

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, true)]
        [InlineData(new int[] { 5, 4, 3, 2, 1 }, true)]
        public void TestIsSafeReport(int[] reportArray, bool expected)
        {
            var report = reportArray.ToList();
            var result = sut.IsSafeReportBasic(report);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, true)]
        [InlineData(new int[] { 5, 4, 3, 2, 1 }, true)]
        [InlineData(new int[] { 5, 6, 3, 2, 1 }, true)]
        [InlineData(new int[] { 5, 6, 3, 2, 3 }, false)]

        public void TestIsSafeReportAdvanced(int[] reportArray, bool expected)
        {
            var report = reportArray.ToList();
            var result = sut.IsSafeReport(report);
            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData("7 6 4 2 1", true)]
        [InlineData("1 2 7 8 9", false)]
        [InlineData("9 7 6 2 1", false)]
        [InlineData("1 3 2 4 5", true)]
        [InlineData("8 6 4 4 1", true)]
        [InlineData("1 3 6 7 9", true)]
        [InlineData("1 2 2 4 5", true)]
        [InlineData("1 2 3 9 7", false)]
        [InlineData("1 2 3 4 17", true)]
        [InlineData("1 17 2", true)]
        [InlineData("1 5 5", false)]


        public void TestIsSafeReportAdvancedString(string reportArray, bool expected)
        {
            var reportData = DataParser.ParseDataLine(reportArray);
            var result = sut.IsSafeReport(reportData);
            Assert.Equal(expected, result);
        }

    }
}
