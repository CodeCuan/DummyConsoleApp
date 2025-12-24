using DummyConsoleApp.AdventOfCoding.Advent2015;

namespace DummyConsoleApp.Test.Aoc2015;

public class Day05StringEvaluateTests
{
    public Day05StringEvaluate sut = new();

    [Theory]
    [InlineData("ugknbfddgicrmopn", true)]
    [InlineData("jchzalrnumimnmhp", false)]
    [InlineData("haegwjzuvuyypxyu", false)]
    [InlineData("dvszwmarrgswjxmb", false)]
    public void StringIsValid_ReturnsExpected(string input, bool expected)
    {
        var result = sut.StringIsValid(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("qjhvhtzxzqqjkmpb", true)]
    [InlineData("uurcxstgmygtbstg", false)]
    [InlineData("ieodomkazucvgmuy", false)]
    public void StringIsValidAdvanced_ReturnsExpected(string input, bool expected)
    {
        var result = sut.StringIsValidAdvanced(input);
        Assert.Equal(expected, result);
    }
}
