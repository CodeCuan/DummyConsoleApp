using DummyConsoleApp.AdventOfCoding.Data;

namespace DummyConsoleApp.AdventOfCoding.Advent2024;

public class Advent2024Solution1
{
    public void Main()
    {
        GetSimilarityScore();
    }

    public AdventDataSet adventData;
    public List<int> list1;
    public List<int> list2;
    public Advent2024Solution1()
    {
        adventData = GetAdventData();
        list1 = adventData.list1;
        list2 = adventData.list2;
    }
    public void ChallengeOneDistance()
    {
        list1.Sort();
        list2.Sort();
        int sum = 0;
        for (int i = 0; i < list1.Count; i++)
        {
            sum += Math.Abs(list1[i] - list2[i]);
        }
        Console.WriteLine(sum);
    }

    public void GetSimilarityScore()
    {

        var frequency = list2.GroupBy(x => x)
                         .ToDictionary(g => g.Key, g => g.Count());
        var score = list1.Sum(x => frequency.ContainsKey(x) ? x * frequency[x] : 0);
        Console.WriteLine(score);
    }

    public static AdventDataSet GetAdventData()
    {
        var input = AdventData2024.PuzzleOneCoordinates;
        AdventDataSet dataSet = new AdventDataSet();
        foreach (var entry in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = entry.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            dataSet.list1.Add(int.Parse(parts[0]));
            dataSet.list2.Add(int.Parse(parts[1]));
        }
        return dataSet;
    }
}

public class AdventDataSet()
{
    public List<int> list1 = new List<int>();
    public List<int> list2 = new List<int>();

}
