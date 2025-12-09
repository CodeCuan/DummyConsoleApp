namespace DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

public static class HashsetExtensions
{
    public static void AddRange<TValue>(this HashSet<TValue> myHashSet, IEnumerable< TValue> values)
    {
        foreach (var item in values)
            myHashSet.Add(item);
    }
}