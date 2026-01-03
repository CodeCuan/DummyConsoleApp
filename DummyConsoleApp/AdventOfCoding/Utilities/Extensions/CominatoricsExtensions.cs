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

    public static IEnumerable<List<TEntry>> GetChains<TEntry>(this ICollection<TEntry> entries)
        => entries.GetChains([]);


    public static IEnumerable<List<TEntry>> GetChains<TEntry>(this ICollection<TEntry> entries, List<TEntry> activeChain)
    {
        var unprocessed = entries.Where(entries => !activeChain.Contains(entries)).ToList();
        foreach (var entry in unprocessed)
        {
            var newActiveChain = new List<TEntry>(activeChain) { entry };
            if (unprocessed.Count == 1)
                yield return newActiveChain;
            else
            {
                foreach(var subChain in unprocessed.GetChains(newActiveChain))
                {
                    yield return subChain;
                }
            }
        }
    }
}
