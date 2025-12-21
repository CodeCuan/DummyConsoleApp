using DummyConsoleApp.AdventOfCoding.Advent2025;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.Test.Aoc2025;

public class AoC2025Day3Tests
{
    public Day03BatteryCalculator sut = new Day03BatteryCalculator();

    [Theory]
    [InlineData("123456789", 89, 2)]
    [InlineData("987654321111111", 987654321111, 12)]
    [InlineData("234234234234278", 434234234278, 12)]

    public void BankCalculator_Calculates(string inputLine, long totalCount, int batteryCount) {
        var inputData = DataParser.ParseDataLine(inputLine, true);
        var result = sut.GetVoltage(inputData, batteryCount);
        Assert.Equal(totalCount, result);
    }
}
