using Combinatorics.Collections;

namespace DummyConsoleApp.AdventOfCoding.Utilities.Extensions;

public static class CominatoricsExtensions
{
    public static IEnumerable<List<TEntry>> GetCombinations<TEntry>(
        this IList<TEntry> entries,
        int outputSize
    )
    {
        var combinations = new Combinations<TEntry>(entries, outputSize);
        foreach (var combination in combinations)
        {
            yield return combination.ToList();
        }
    }

    public static IEnumerable<List<TEntry>> GetChains<TEntry>(this ICollection<TEntry> entries) =>
        entries.GetChains([]);

    public static IEnumerable<List<TEntry>> GetChains<TEntry>(
        this ICollection<TEntry> entries,
        List<TEntry> activeChain
    )
    {
        var unprocessed = entries.Where(entries => !activeChain.Contains(entries)).ToList();
        foreach (var entry in unprocessed)
        {
            var newActiveChain = new List<TEntry>(activeChain) { entry };
            if (unprocessed.Count == 1)
                yield return newActiveChain;
            else
            {
                foreach (var subChain in unprocessed.GetChains(newActiveChain))
                {
                    yield return subChain;
                }
            }
        }
    }
}
