using DummyConsoleApp.AdventOfCoding.Advent2015;
using Newtonsoft.Json.Linq;

namespace DummyConsoleApp.Test.Aoc2015;

public class Day12JsonDataTests
{
    Day12JsonData sut = new();

    [Theory]
    [InlineData(@"[1,2,3]", 6)]
    [InlineData(@"{""a"":2,""b"":4}", 6)]
    [InlineData(@"[[[3]]]", 3)]
    [InlineData(@"[]", 0)]
    [InlineData(@"{""a"":{""b"":4},""c"":-1}", 3)]
    [InlineData(@"{""a"":[-1,1]}",0)]
    [InlineData(@"[-1,{""a"":1}]",0)]
    public void CountNumbers_CountsCorrectly(string json, int expected)
    {
        var jToken = JToken.Parse(json);
        var result = sut.CountNumbersInNode(jToken);
        Assert.Equal(expected, result);
    }
}
