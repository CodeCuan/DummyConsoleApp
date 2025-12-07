using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025;

public class Day1PasswordCracker
{
    List<string> codes;
    const int startingPos = 50;
    public Day1PasswordCracker() { 
    }

    public void Main()
    {
        codes = DataParser.SplitDataLine(AdventData2025.Day1Rotations, true);
        CrackPasswordsAdvanced();
    }

    public void CrackPasswords() {

        int password = 0;
        int position = startingPos;
        foreach (var code in codes)
        {
            var direction = code[0];
            var steps = int.Parse(code.Substring(1));
            if (direction == 'l')
                position -= steps;
            else
                position += steps;

            position = (position + 100) % 100;
            if (position == 0)
                password++;
        }
        Console.WriteLine($"The password is: {password}");
    }

    public int CrackPasswordsAdvanced(List<string>? inputData = null)
    {
        if (inputData != null)
            codes = inputData;
        int password = 0;
        int position = startingPos;
        foreach (var code in codes)
        {
            var direction = code[0];
            var steps = int.Parse(code.Substring(1));
            password += AdjustLargeRotation(ref steps);
            var positionWasZero = position == 0;
            position = direction == 'l' 
                ? position - steps 
                : position + steps;

            if (!positionWasZero
                && (Math.Abs(position) >= 100
                    || position <= 0))
                password++;

            position = (position + 100) % 100;
        }
        Console.WriteLine($"The password is: {password}");
        return password;
    }

    private static int AdjustLargeRotation(ref int rotation) {
        var turns = Math.Abs(rotation / 100);
        rotation = rotation % 100;
        return turns;
    }
}
