using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day10IntFrequencyProcessor
{
    public void Main()
    {
        Console.WriteLine("Day 10 Int Frequency Processor");
        var starting = "1113222113";
        var stoppy = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < 50; i++) {
            starting = ProcessString(starting);
        }
        stoppy.Stop();
        Console.WriteLine($"Processed string - length is {starting.Length} . Processed in {stoppy.ElapsedMilliseconds}");
    }

    public string ProcessString(string input) { 
        string output = "";
        char currentChar = input[0];
        int currentCount = 1;
        foreach (var ch in input.Skip(1))
        {
            if (ch != currentChar)
            {
                output += $"{currentCount}{currentChar}";
                currentChar = ch;
                currentCount = 1;
            }
            else
            {
                currentCount++;
            }
        }
        output += $"{currentCount}{currentChar}";
        return output;
    }
}
