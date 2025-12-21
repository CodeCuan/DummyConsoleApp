using DummyConsoleApp.AdventOfCoding.Advent2025;

namespace DummyConsoleApp.Test.Aoc2025;

public class AoCDay6Tests
{
    public Day06MathHomework sut = new();

    [Theory]
    [InlineData(@"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   + ", 4277556)]
    public void SolveProblems_Returns_Correct_Total(string input, long expectedTotal)
    {
        // Act
        var result = sut.SolveProblems(input);
        // Assert
        Assert.Equal(expectedTotal, result);
    }
    [Theory]
    [InlineData(@"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   + ", 3263827)]
    public void SolveProblems_complex_Returns_Correct_Total(string input, long expectedTotal)
    {
        // Act
        var result = sut.SolveProblems(input, false);
        // Assert
        Assert.Equal(expectedTotal, result);
    }
}
