namespace DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

public static class CominatoricsExtensions
{
    public static IEnumerable<List<TEntry>> GetCombinations<TEntry>(this IList<TEntry> entries, int outputSize)
    {
        if (outputSize == 1)
        {
            foreach (var entry in entries)
                yield return [entry];
            yield break;
        }
        var unprocessed = entries.ToList();
        foreach (var entry in entries)
        {
            unprocessed.Remove(entry);
            foreach (var entry2 in unprocessed)
            {
                if (outputSize == 2)
                {
                    yield return new List<TEntry> { entry, entry2 };
                }
                else
                {
                    foreach (var subCombination in unprocessed.GetCombinations(outputSize - 1))
                    {
                        subCombination.Add(entry);
                        yield return subCombination;
                    }
                }
            }
        }
    }
}
