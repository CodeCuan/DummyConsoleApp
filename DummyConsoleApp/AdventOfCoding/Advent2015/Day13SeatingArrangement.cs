using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day13SeatingArrangement
{
    public void Main()
    {
        var optimalHappinessWithMe2 = GetOptimalHappiness(AdventData2015.Day13SeatingArrangements, true);

        Console.WriteLine("Day 13: Seating Arrangement");
        var stoppy = Stopwatch.StartNew();
        var optimalHappiness = GetOptimalHappiness(AdventData2015.Day13SeatingArrangements, false);
        stoppy.Stop();
        Console.WriteLine($"Optimal happiness: {optimalHappiness} . Worked out in {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var optimalHappinessWithMe = GetOptimalHappiness(AdventData2015.Day13SeatingArrangements, true);
        stoppy.Stop();
        Console.WriteLine($"Optimal happiness with me: {optimalHappinessWithMe} . Worked out in {stoppy.ElapsedMilliseconds} ms");
    }

    public int GetOptimalHappiness(string data, bool addMe)
    {
        var peopleData = InitData(data);
        if(addMe)
            peopleData["Me"] = new SeatPerson("Me");

        var peopleChains = peopleData.Values.GetChains().ToList();
        var bestChain =  peopleChains.MaxBy(EvaluateChain)
            ?? throw new InvalidOperationException("No seating chains found");
        return EvaluateChain(bestChain);
    }

    private int EvaluateChain(List<SeatPerson> chain) { 
        int totalHappiness = 0;
        for (int i = 0; i < chain.Count-1; i++)
        {
            var person = chain[i];
            var neighBour = chain[i + 1];
            totalHappiness += person.happinessChanges[neighBour];
            totalHappiness += person.offsetHappinessChanges[neighBour];
        }
        totalHappiness += chain.Last().happinessChanges[chain.First()];
        totalHappiness += chain.Last().offsetHappinessChanges[chain.First()];

        return totalHappiness;
    }

    private Dictionary<string, SeatPerson> InitData(string input ) {
        Dictionary<string, SeatPerson> people = [];
        var lines = DataParser.SplitLines(input);
        foreach (var line in lines)
        {
            var lineSections = line.Split(' ');
            var personName = lineSections[0];
            var targetName = lineSections[10].TrimEnd('.');
            var gain = lineSections[2] == "gain" ? 1 : -1;
            var happinessChange = gain * int.Parse(lineSections[3]);
            if (!people.TryGetValue(personName, out var sourcePerson))
            {
                sourcePerson = new SeatPerson(personName);
                people[personName] = sourcePerson;
            }
            if (!people.TryGetValue(targetName, out var targetPerson))
            {
                targetPerson = new SeatPerson(targetName);
                people[targetName] = targetPerson;
            }
            sourcePerson.happinessChanges[targetPerson] = happinessChange;
        }

        foreach (var person in people.Values)
        {
            foreach (var neighbourSet in person.happinessChanges)
            {
                neighbourSet.Key.offsetHappinessChanges[person] += neighbourSet.Value;
            }
        }
        return people;
    }

    public class SeatPerson {
        public string name;
        public DefaultDictionary<SeatPerson, int> happinessChanges = [];
        public DefaultDictionary<SeatPerson, int> offsetHappinessChanges = [];
        public SeatPerson(string name) { 
            this.name = name;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
