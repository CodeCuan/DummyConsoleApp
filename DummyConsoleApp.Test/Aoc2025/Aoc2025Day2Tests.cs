using DummyConsoleApp.AdventOfCoding.Advent2025;
using DummyConsoleApp.AdventOfCoding.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp.Test.Aoc2025
{
    public class Aoc2025Day2Tests
    {
        [Theory]
        [InlineData(112233, false)]
        [InlineData(11, true)]
        [InlineData(1212, true)]
        [InlineData(121212, false)]
        public void IsValidCode_SolutionOne(int input, bool expectedResult) {

            Assert.Equal(expectedResult, Day02CodeValidator.IsValidCodeSimple(input));
        }

        [Theory]
        [InlineData(112233, false)]
        [InlineData(11, true)]
        [InlineData(1212, true)]
        [InlineData(121212, true)]
        public void IsValidCode_SolutionTwo(int input, bool expectedResult)
        {

            Assert.Equal(expectedResult, Day02CodeValidator.IsValidCode(input));
        }
        [Theory]
        [InlineData("11-22", 33)]
        [InlineData("99-115", 210)]
        [InlineData("1188511885-1188511885", 1188511885)]
        [InlineData("824824821-824824827", 824824824)]

        public void IsValidSum_SolutionTwo(string input, int expectedResult) {
            
            Assert.Equal(expectedResult, new Day02CodeValidator().GetInvalidNumberSum(input, false));

        }
    }
}
