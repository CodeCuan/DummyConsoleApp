using System.Numerics;

namespace DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

public static class DictionaryExtensions
{
    public static void AddOrIncrement<TKey, TValue>(this Dictionary<TKey, TValue> myDictionary, TKey key, TValue incrementValue)
        where TValue : INumber<TValue>
        where TKey : notnull
    {
        if (myDictionary.ContainsKey(key))
        {
            myDictionary[key] += incrementValue;
        }
        else
        {
            myDictionary[key] = incrementValue;
        }
    }

    public static void AddOrIncrement<TKey, TValue>(this Dictionary<TKey, TValue> myDictionary, IEnumerable<TKey> keys, TValue incrementValue)
        where TValue : INumber<TValue>
        where TKey : notnull

    {
        foreach (var key in keys)
            myDictionary.AddOrIncrement(key, incrementValue);
    }

    //public static void AddToList<TKey, TValue>(this Dictionary<TKey, List<TValue>> myDictionary, TKey key, TValue incrementValue)
    //    where TKey : notnull
    //{
    //    if (myDictionary.ContainsKey(key))
    //    {
    //        myDictionary[key].Add(incrementValue);
    //    }
    //    else
    //    {
    //        myDictionary[key] = [incrementValue];
    //    }
    //}

    public static void AddToList<TKey, TValue, TDictionary>(this TDictionary myDictionary, TKey key, TValue incrementValue)
        where TDictionary : IDictionary<TKey, List<TValue>>
        where TKey : notnull
    {
        if (myDictionary.ContainsKey(key))
        {
            myDictionary[key].Add(incrementValue);
        }
        else
        {
            myDictionary[key] = [incrementValue];
        }
    }
}