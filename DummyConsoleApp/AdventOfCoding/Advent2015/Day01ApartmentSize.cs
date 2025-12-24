using DummyConsoleApp.AdventOfCoding.Data;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day01ApartmentSize
{
    public void Main() {
        var stoppy = Stopwatch.StartNew();
        var floorSize = CalculateApartmentSize(AdventData2015.Day1Apartment);
        var basementEntry = CalculateBasementEntry(AdventData2015.Day1Apartment);
        stoppy.Stop();
        Console.WriteLine($"Day 1 Apartment Size: {floorSize} (Execution Time: {stoppy.ElapsedMilliseconds} ms)");
        Console.WriteLine($"Day 1 Basement Entry Position: {basementEntry} (Execution Time: {stoppy.ElapsedMilliseconds} ms)");
    }

    public int CalculateApartmentSize(string input)
    {
        var floor = input.Count(c => c == '(') - input.Count(c => c == ')');
        return floor;
    }

    public int CalculateBasementEntry(string input)
    {
        int count = 0;
        var position = 0;
        foreach (var direction in input) { 
            position += direction == '(' ? 1 : -1;
            count++;
            if(position < 0)
                return count;
        }
        throw new Exception("Basement not reached");
    }
}
