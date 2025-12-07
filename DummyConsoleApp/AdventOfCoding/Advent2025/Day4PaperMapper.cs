using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day4PaperMapper
{

    private const char roll = '@';
    List<List<char>> rolls = [];
    int width;
    int height;
    bool logRolls = false;
    public void Main()
    {
        Console.WriteLine("Day 4 Paper Mapper");
        logRolls = true;
        //ParseRollsAvailable(sampleInput, true);
        var total = ParseRollsAvailable(AdventData2025.Day4Map, true);
        Console.WriteLine($"Total Removable Rolls: {total}");
    }

    public int ParseRollsAvailable(string input, bool recursive)
    {
        rolls = DataParser.ParseDataIntoString(input);
        if (logRolls)
        {
            foreach (var row in rolls)
            {
                Console.WriteLine(string.Join("", row));
            }
        }
        return ParseRollsAvailable(rolls, recursive);
    }

    public int ParseRollsAvailable(List<List<char>> input, bool recursive, int passNumber = 0)
    {
        width = rolls[0].Count;
        height = rolls.Count;
        int removableRolls = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var currentChar = rolls[y][x];
                if(currentChar == 'x')
                    rolls[y][x] = '.';
                if (currentChar != roll)
                    continue;
                var neighbours = ParseNeighbours(x, y);
                if (neighbours < 4)
                {
                    removableRolls++;
                    rolls[y][x] = 'x';
                }
            }
        }
        Console.WriteLine($"Removable Rolls pass {passNumber}: {removableRolls}");
        if (logRolls) { 
            foreach(var row in rolls)
            {
                Console.WriteLine(string.Join("", row));
            }
        }
        if (removableRolls > 0 && recursive)
        {
            removableRolls += ParseRollsAvailable(input, true, passNumber + 1);
        }

        return removableRolls;
    }

    public int ParseNeighbours(int x, int y)
    {
        int neighbours = 0;
        for (int ny = y - 1; ny <= y + 1; ny++)
        {
            for (int nx = x - 1; nx <= x + 1; nx++)
            {
                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    continue;
                if (nx == x && ny == y)
                    continue;
                if (rolls[ny][nx] == roll)
                    neighbours++;
            }
        }
        return neighbours;
    }
}
