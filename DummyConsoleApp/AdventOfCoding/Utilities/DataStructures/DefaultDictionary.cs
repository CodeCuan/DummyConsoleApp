namespace DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;

public class DefaultDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>
    where TValue : new()
    where TKey : notnull
{
    public new TValue this[TKey key]
    {
        get
        {
            if (!TryGetValue(key, out TValue? val))
            {
                val = new();
                Add(key, val);
            }
            return val;
        }
        set
        {
            base[key] = value;
        }
    }
}
