using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day15Ingredients
{
    private const string Sample = @"Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3";
    public void Main()
    {
        Console.WriteLine("Day 15: Science for Hungry People");
        var stoppy = Stopwatch.StartNew();
        Console.WriteLine($"Sample Score: {GetTotalScore(Sample)} . Took {stoppy.ElapsedMilliseconds} ms");
        stoppy.Restart();
        var totalScore = GetTotalScore(AdventData2015.Day15IngredientData);
        Console.WriteLine($"Total Score: {totalScore} in {stoppy.ElapsedMilliseconds}ms");
        stoppy.Restart();
        var totalScoreWithCalories = GetTotalScore(AdventData2015.Day15IngredientData, caloryReq: 500);
        stoppy.Stop();
        Console.WriteLine($"Total Score with 500 calories: {totalScoreWithCalories} in {stoppy.ElapsedMilliseconds}ms");

    }

    public long GetTotalScore(string input, int maxSpoonfuls = 100, int? caloryReq = null)
    {
        var ingredients = DataParser.SplitLines(input).Select(i => new Ingredient(i)).ToList();
        var scores = GetScores(ingredients, ingredientSpace: maxSpoonfuls, caloryReq: caloryReq);
        return scores.Max();
    }

    public IEnumerable<long> GetScores(List<Ingredient> ingredients, int activeIndex = 0, int ingredientSpace = 100, int? caloryReq = null)
    {
        var activeIngredient = ingredients[activeIndex];
        activeIndex++;
        if (activeIndex == ingredients.Count)
        {
            activeIngredient.Spoonfuls = ingredientSpace;
            yield return CalculateIngredients(ingredients, caloryReq);
            yield break;
        }
        
        for (int i = 0; i <= ingredientSpace; i++)
        {
            activeIngredient.Spoonfuls = i;
            foreach (var score in GetScores(ingredients, activeIndex, ingredientSpace - i, caloryReq: caloryReq))
            {
                yield return score;
            }
        }
    }


    private static long CalculateIngredients(List<Ingredient> ingredients, int? caloryReq)
    {
        if(caloryReq.HasValue 
            && ingredients.Sum(i => i.Calories * i.Spoonfuls) != caloryReq.Value)
            return 0;
        List<long> scores = [ ingredients.Sum(i => i.Capacity * i.Spoonfuls),
             ingredients.Sum(i => i.Durability * i.Spoonfuls),
             ingredients.Sum(i => i.Flavor * i.Spoonfuls),
             ingredients.Sum(i => i.Texture * i.Spoonfuls)];
        if (scores.Any(s => s <= 0))
            return 0;
        var totalScore = scores.Aggregate(1L, (currentProduct, value) => currentProduct * value);
        return totalScore;

    }

    public class Ingredient
    {
        public Ingredient(string input)
        {
            var inputParts = input.Split([' ', ':', ','], StringSplitOptions.RemoveEmptyEntries);
            Name = inputParts[0];
            Capacity = int.Parse(inputParts[2]);
            Durability = int.Parse(inputParts[4]);
            Flavor = int.Parse(inputParts[6]);
            Texture = int.Parse(inputParts[8]);
            Calories = int.Parse(inputParts[10]);
        }
        public string Name { get; set; }
        public long Capacity { get; set; }
        public long Durability { get; set; }
        public long Flavor { get; set; }
        public long Texture { get; set; }
        public long Calories { get; set; }

        public int Spoonfuls = 0;

        public override string ToString()
        {
            return $"{Spoonfuls}*{Name}";
        }
    }
}
