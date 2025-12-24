using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day04Md5Hash
{
    public void Main() { 
        Console.WriteLine("Day 4 MD5 Hashes");
        var input = "yzbqklnj";
        var stoppy = Stopwatch.StartNew();
        var lowestNum = FindLowestNumberWithMd5Zeros(input, 5);
        stoppy.Stop();
        Console.WriteLine($"Input: {input}, has loweset number for 5: {lowestNum} . Took {stoppy.ElapsedMilliseconds} ms");

        stoppy.Restart();
        lowestNum = FindLowestNumberWithMd5Zeros(input, 6);
        stoppy.Stop();
        Console.WriteLine($"Input: {input}, has loweset number for 6: {lowestNum} . Took {stoppy.ElapsedMilliseconds} ms");
    }

    public int FindLowestNumberWithMd5Zeros(string key, int zeroes)
    {
        int number = 0;
        while (true)
        {
            var testString = $"{key}{number}";
            if (Md5HasZeros(testString, zeroes))
                return number;
            number++;
        }
    }

    public static bool Md5HasZeros(string input, int zeroCount=5)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);

        foreach(var character in hashBytes
            .SelectMany(hb => hb.ToString("x2"))
            .Take(zeroCount))
        {
            if (character != '0')
                return false;
        }

        return true;
    }
}