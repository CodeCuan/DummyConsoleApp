using System.Numerics;

namespace DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

public static class HashsetExtensions
{
    public static void AddRange<TValue>(this HashSet<TValue> myHashSet, IEnumerable< TValue> values)
        where TValue : INumber<TValue>
    {
        foreach (var item in values)
            myHashSet.Add(item);
    }
}