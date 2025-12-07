using DummyConsoleApp.AdventOfCoding.Advent2025;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.Test.Aoc2025;

public class Aoc2025Day1Tests
{
    private readonly Day1PasswordCracker sut;
    public Aoc2025Day1Tests() { 
        sut = new Day1PasswordCracker();
    }

    [Theory]
    [InlineData("R1000", 10)]
    [InlineData("L68\r\nL30\r\nR48\r\nL5\r\nR60\r\nL55\r\nL1\r\nL99\r\nR14\r\nL82",6)]
    public void CrackPasswords_WithOfficialSample(string sampleData, int expectedCount)
    {
        var data = DataParser.SplitDataLine(sampleData, true);
        var value = sut.CrackPasswordsAdvanced(data);
        Assert.Equal(expectedCount, value);
    }
}
