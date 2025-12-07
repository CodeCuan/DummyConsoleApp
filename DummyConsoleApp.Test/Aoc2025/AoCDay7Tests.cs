using DummyConsoleApp.AdventOfCoding.Advent2025;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp.Test.Aoc2025;

public class AoCDay7Tests
{
    private Day7TachyonBeam sut = new();
    private const string TestData = @".......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............";

    [Fact]
    public void ProcessBeam_TestData_ReturnsExpected()
    {
        // Arrange
        var expected = 21;

        // Act
        var result = sut.ProcessMultiBeam(TestData);
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessTimelines_TestData_ReturnsExpected()
    {
        // Arrange
        var expected = 40;

        // Act
        var result = sut.ProcessBeamTimelines(TestData);
        // Assert
        Assert.Equal(expected, result);
    }

}
