using DummyConsoleApp.AdventOfCoding.Advent2025;

namespace DummyConsoleApp.Test.Aoc2025;

public class AoCDay10Tests
{
    Day10SwitchLights sut = new();
    
    [Theory]
    [InlineData(2, "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}")]
    [InlineData(3, "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}")]
    [InlineData(2, "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}")]
    public void GetMinCount_ForLights_Works(int expectedCount, string line) { 
        var value = sut.GetSwitchCountForLine(line, false);
        Assert.Equal(expectedCount, value);
    }

    [Theory]
    [InlineData(10, "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}")]
    [InlineData(12, "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}")]
    [InlineData(11, "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}")]
    public void GetMinCount_ForVoltage_Works(int expectedCount, string line)
    {
        var value = sut.GetSwitchCountForLine(line, true);
        Assert.Equal(expectedCount, value);
    }
}
    