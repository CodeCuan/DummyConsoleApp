using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day21Rpg
{
    EntityBattler Boss = new();

    public void Main()
    {
        Console.WriteLine("Day 21 RPG");
        var stoppy = Stopwatch.StartNew();
        var bestPlayer = GetMinCostToWinPlayer(AdventData2015.Day21RpgBossJson);
        stoppy.Stop();
        Console.WriteLine($"Minimum cost to win: {bestPlayer.items.Sum(i => i.cost)} .(calculated in {stoppy.ElapsedMilliseconds} ms)");
        Console.WriteLine($"Player had attack {bestPlayer.damage} and def {bestPlayer.armor} with armour set: {string.Join(" | ", bestPlayer.items)}");

        stoppy.Restart();
        var worstPlayer = GetMaxCostToLosePlayer(AdventData2015.Day21RpgBossJson);
        stoppy.Stop();
        Console.WriteLine($"Maximum cost to lose: {worstPlayer.items.Sum(i => i.cost)} .(calculated in {stoppy.ElapsedMilliseconds} ms)");
        Console.WriteLine($"Player had attack {worstPlayer.damage} and def {worstPlayer.armor} with armour set: {string.Join(" | ", worstPlayer.items)}");
    }

    public int GetMinCostToWin(string bossInput)
    {
        var player = GetMinCostToWinPlayer(bossInput);
        return player.items.Sum(i => i.cost);
    }

    private EntityBattler GetMinCostToWinPlayer(string bossInput)
    {
        SetBoss(bossInput);
        foreach (var inputSet in GetItemCombinations().OrderBy(items => items.Sum(i => i.cost)))
        {
            var player = new EntityBattler(100, inputSet);
            if (WinsBattle(player))
            {
                return player;
            }
        }
        throw new Exception("Could not win");
    }

    private EntityBattler GetMaxCostToLosePlayer(string bossInput)
    {
        SetBoss(bossInput);
        foreach (var inputSet in GetItemCombinations()
            .OrderByDescending(items => items.Sum(i => i.cost)))
        {
            var player = new EntityBattler(100, inputSet);
            if (!WinsBattle(player))
            {
                return player;
            }
        }
        throw new Exception("Could not lose");
    }

    private void SetBoss(string input)
    {
        Boss = JsonConvert.DeserializeObject<EntityBattler>(input)
            ?? throw new Exception("deserialize failed");
    }

    private bool WinsBattle(EntityBattler player)
    {
        return player.DiesOnRound(Boss.damage) >= Boss.DiesOnRound(player.damage);
    }

    private IEnumerable<List<Item>> GetItemCombinations()
    {
        foreach (var weapon in Weapons)
            foreach (var armor in Armors)
                foreach (var ringSet in SelectRings())
                    yield return [weapon, armor, .. ringSet];
    }

    private IEnumerable<List<Item>> SelectRings()
    {
        yield return [];
        foreach (var ring in Rings)
        {
            yield return [ring];
        }
        foreach (var ringSet in Rings.GetCombinations(2))
        {
            yield return ringSet;
        }
    }

    private class EntityBattler
    {
        public int hp;
        public int damage;
        public int armor;
        public List<Item> items;

        public int DiesOnRound(int damage)
        {
            double actualDamage = Math.Max(1, damage - armor);
            return (int)Math.Ceiling(hp / actualDamage);
        }
        public EntityBattler()
        {
            items = [];
        }
        public EntityBattler(int hp, List<Item> items)
        {
            this.hp = hp;
            damage = items.Sum(i => i.damage);
            armor = items.Sum(i => i.armor);
            this.items = items;
        }
    }

    private class Item(int cost, int damage, int armor, string name)
    {
        public readonly int damage = damage;
        public readonly int armor = armor;
        public readonly int cost = cost;
        public readonly string name = name;
        public override string ToString()
        {
            return $"({cost}) {name}: {damage}|{armor}";
        }
    }

    private List<Item> Weapons = [
        new Item(8, 4, 0, "Dagger"),
        new Item(10, 5, 0,"Shortsword"),
        new Item(25, 6, 0,"Warhammer"),
        new Item(40, 7, 0,"Longsword"),
        new Item(74, 8, 0,"Greataxe")
        ];

    private List<Item> Armors = [
        new Item(13, 0, 1, "Leather"),
        new Item(31, 0, 2, "Chainmail"),
        new Item(53, 0, 3, "Splitmail"),
        new Item(75, 0, 4, "Bandedmail"),
        new Item(102, 0, 5, "Platemail"),
        new Item(0, 0, 0, "Unarmored")
        ];

    private List<Item> Rings = [
        new Item(25, 1, 0, "Damage Ring +1"),
        new Item(50, 2, 0, "Damage Ring +2"),
        new Item(100, 3, 0, "Damage Ring +3"),
        new Item(20, 0, 1, "Defense Ring +1"),
        new Item(40, 0, 2, "Defense Ring +2"),
        new Item(80, 0, 3, "Defense Ring +3")
        ];
}