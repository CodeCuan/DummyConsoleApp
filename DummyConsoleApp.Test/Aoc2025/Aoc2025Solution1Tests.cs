using DummyConsoleApp.AdventOfCoding.Advent2025;
using DummyConsoleApp.AdventOfCoding.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp.Test.Aoc2025;

public class Aoc2025Solution1Tests
{
    private readonly Day1PasswordCracker sut;
    public Aoc2025Solution1Tests() { 
        sut = new Day1PasswordCracker();
    }

    [Fact]
    public void TestCrackPasswords()
    {
        var sampleData = new List<string>
        {
            "R1000"
        };

        TestAndAssert(sampleData, 10);

    }

    [Fact]
    public void TestCrackedPasswordsSample() {
        var sampleData = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        var data = DataParser.SplitDataLine(sampleData, true);
        TestAndAssert(data, 6);
    }

    private void TestAndAssert(List<string> data, int expectedValue) {
        var value = sut.CrackPasswordsAdvanced(data);

        Assert.Equal(expectedValue, value);
    }
}
