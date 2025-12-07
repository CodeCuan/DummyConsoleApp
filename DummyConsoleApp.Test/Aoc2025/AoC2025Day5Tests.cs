using DummyConsoleApp.AdventOfCoding.Advent2025;

namespace DummyConsoleApp.Test.Aoc2025;

public class AoC2025Day5Tests
{
    public Day5IngredientCounter sut = new();
    [Theory]
    [InlineData(@"3 -5
10-14
16-20
12-18", 14)]
    [InlineData(@"1-10
10-14
11-20
14-18", 20)]
    [InlineData(@"1-10
10-14
14-18
11-20", 20)]
    public void CountIngredients_Returns_Correct_Count(string rangesInput, long expectedCount)
    {
        // Act
        var result = sut.CountRanges(rangesInput);

        // Assert
        Assert.Equal(expectedCount, result);
    }

}
