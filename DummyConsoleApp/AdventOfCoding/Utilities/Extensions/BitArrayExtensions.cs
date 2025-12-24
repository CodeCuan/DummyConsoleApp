using System.Collections;

namespace DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

public static class BitArrayExtensions
{
    public static int ToInt(this BitArray bitArray)
    {
        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];
    }
}
