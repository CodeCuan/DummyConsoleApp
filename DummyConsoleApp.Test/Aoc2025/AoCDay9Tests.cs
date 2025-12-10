using DummyConsoleApp.AdventOfCoding.Advent2025;

namespace DummyConsoleApp.Test.Aoc2025;

public class AoCDay9Tests
{
    Day9RedCarpet sut = new Day9RedCarpet();

    [Fact]
    public void LargestRectangle_WithSampleData_CalculatesCorrectly()
    {
        var input = @"7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3";
        var result = sut.GetLargestRectangle(input);
        Assert.Equal(50, result);
    }
}
