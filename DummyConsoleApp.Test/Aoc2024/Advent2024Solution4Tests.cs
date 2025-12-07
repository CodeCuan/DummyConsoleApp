using DummyConsoleApp.AdventOfCoding.Advent2024;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp.Test.Aoc2024;

public class Advent2024Solution4Tests
{
    public Advent2024Solution4 sut = new Advent2024Solution4();

    [Theory]
    [InlineData(@"", 161)]
    public void Solve_ReturnsExpectedResult(string input, int expected)
    {
        var result = sut.Search(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX", 18)]
    public void Solve2_ReturnsExpectedResult(string input, int expected)
    {
        var result = sut.Search(input);
        Assert.Equal(expected, result);
    }
}
