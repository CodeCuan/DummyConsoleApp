using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day19MoleculeFactory
{
    public void Main()
    {
        CountMinTransformsBasic(SampleData.SampleMolecule, SampleData.SampleTransformers, log: true);
        Console.WriteLine("Day 19 Molecule Factory");
        var stoppy = System.Diagnostics.Stopwatch.StartNew();
        var distinctCount = CountDistinct(AdventData2015.Day19InitialState, AdventData2015.Day19Transformers);
        stoppy.Stop();
        Console.WriteLine($"Distinct molecule count: {distinctCount} (calculated in {stoppy.ElapsedMilliseconds} ms)");
        stoppy.Restart();
        var minTransforms = CountMinTransformsBasic(AdventData2015.Day19InitialState, AdventData2015.Day19Transformers);
        stoppy.Stop();

        Console.WriteLine($"Minimum transforms to reach 'e': {minTransforms} (calculated in {stoppy.ElapsedMilliseconds} ms)");
    }

    public int CountMinTransformsBasic(string currentMolecule, string transformers, string destination = "e", bool log = false)
    {
        var moleculeTransformers = DataParser.SplitLines(transformers).Select(i => new MoleculeTransfomer(i)).GroupBy(mt => mt.To.Length).ToDictionary(g => g.Key, g => g.ToList());
        int transformationNo = 0;
        while (true)
        {
            if (log)
                Console.WriteLine($"{transformationNo}: {currentMolecule}");
            transformationNo++;
            bool foundMatch = false;
            foreach (var transformer in moleculeTransformers.OrderByDescending(mt => mt.Key).SelectMany(ts => ts.Value))
            {
                var firstMatch = transformer.GetReverseTransforms(currentMolecule)
                    .Take(1).ToList();
                if (firstMatch.Count > 0)
                {
                    currentMolecule = firstMatch.First();
                    foundMatch = true;
                    break;
                }
            }


          if(currentMolecule == destination)
                return transformationNo;
            if (!foundMatch)
                throw new Exception($"Failed to progress past {currentMolecule}");
        }
    }

    public int CountDistinct(string initial, string transformers)
    {
        var moleculeTransformers = DataParser.SplitLines(transformers).Select(i => new MoleculeTransfomer(i)).ToList();
        var distinctMolecules = new HashSet<string>();
        foreach (var transformer in moleculeTransformers)
        {
            distinctMolecules.AddRange(transformer.GetTransforms(initial));
        }
        return distinctMolecules.Count;
    }

    private class MoleculeTransfomer
    {
        public string From { get; set; }
        public string To { get; set; }
        public Regex FromRegex { get; set; }
        public Regex ToRegex { get; set; }
        public override string ToString()
        {
            return $"{From}->{To}";
        }
        public MoleculeTransfomer(string input)
        {
            var sections = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            From = sections[0];
            To = sections[2];
            FromRegex = new(From);
            ToRegex = new(To);
        }

        internal IEnumerable<string> GetTransforms(string input)
        {
            foreach (Match match in FromRegex.Matches(input))
            {
                var transformed = input.Substring(0, match.Index)
                    + To
                    + input.Substring(match.Index + From.Length);
                yield return transformed;
            }
            ;
        }

        internal IEnumerable<string> GetReverseTransforms(string input)
        {
            foreach (Match match in ToRegex.Matches(input))
            {
                var transformed = input.Substring(0, match.Index)
                    + From
                    + input.Substring(match.Index + To.Length);
                yield return transformed;
            }
            ;
        }
    }
    private static class SampleData
    {
        public const string SampleMolecule = @"HOH";
        public const string SampleTransformers = @"e => H
e => O
H => HO
H => OH
O => HH";
    }
}
