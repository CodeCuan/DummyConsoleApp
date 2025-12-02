using DummyConsoleApp.AdventOfCoding.Advent2024;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp.Test.Aoc2024;

public class Advent2024Solution3Tests
{
    public Advent2024Solution3 sut = new Advent2024Solution3();

    [Theory]
    [InlineData("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", 161)]
    public void Solve_ReturnsExpectedResult(string input, int expected)
    {
        var result = sut.Solve(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", 48)]
    public void Solve2_ReturnsExpectedResult(string input, int expected)
    {
        var result = sut.Solve2(input);
        Assert.Equal(expected, result);
    }
}
