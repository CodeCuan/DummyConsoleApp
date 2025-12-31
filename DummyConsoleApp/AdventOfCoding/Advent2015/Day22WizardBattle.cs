using DummyConsoleApp.AdventOfCoding.Data;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day22WizardBattle
{
    public void Main()
    {
        Console.WriteLine("Day 22 Wizard Battle");
        var stoppy = Stopwatch.StartNew();
        var cheapestBattle = GetCheapestBattle();
        stoppy.Stop();
        Console.WriteLine($"Cheapest battle costs {cheapestBattle.GetMpSpent()} mana. (calculated in {stoppy.ElapsedMilliseconds} ms)");
        Console.WriteLine($"Spells used: {string.Join(", ", cheapestBattle.usedSpells.Select(s => s.name))}");
        stoppy.Restart();
        var hardCheapestBattle = GetCheapestBattle(true);
        stoppy.Stop();
        Console.WriteLine($"Cheapest hard mode battle costs {hardCheapestBattle.GetMpSpent()} mana. (calculated in {stoppy.ElapsedMilliseconds} ms)");
        Console.WriteLine($"Spells used: {string.Join(", ", hardCheapestBattle.usedSpells.Select(s => s.name))}");
        hardCheapestBattle.LogBattle();


    }



    private Battle GetCheapestBattle(bool hardMode = false)
    {
        List<Battle> battlesToProcess = [new() { damagePlayer = hardMode }];
        List<Battle> wonBattles = [];
        int minMpNeeded = int.MaxValue;
        while (battlesToProcess.Count > 0)
        {
            List<Battle> nextRound = [];
            foreach (var battle in battlesToProcess)
            {
                foreach (var round in battle.GetRounds())
                {
                    if (round.Won)
                    {
                        wonBattles.Add(round);
                        minMpNeeded = Math.Min(minMpNeeded, round.GetMpSpent());
                    }
                    else if (round.GetMpSpent() < minMpNeeded)
                    {
                        nextRound.Add(round);
                    }
                }
            }
            battlesToProcess = nextRound;
        }
        return wonBattles.MinBy(b => b.GetMpSpent()) ?? throw new Exception("Could not win");
    }

    private class Battle
    {
        public Battle() { }
        public Battle(Battle battle)
        {
            usedSpells = [.. battle.usedSpells];
            activeEffects = new Dictionary<Spell, int>(battle.activeEffects);
            PlayerStatus = new Player(battle.PlayerStatus);
            boss = new Boss(battle.boss);
            damagePlayer = battle.damagePlayer;
        }
        public List<Spell> usedSpells = [];
        public Dictionary<Spell, int> activeEffects = [];
        public Player PlayerStatus = new();
        public bool Won = false;
        public Boss boss = new();
        public bool damagePlayer = false;


        public IEnumerable<Battle> GetRounds()
        {
            foreach (var spell in availableSpells)
            {
                var battle = new Battle(this);
                if (battle.TryDoCompleteRound(spell))
                    yield return battle;
            }
        }

        private bool TryDoCompleteRound(Spell spell)
        {
            if (!TryDoPlayerTurnAndAddSpell(spell))
                return false;
            if (boss.hp <= 0)
            {
                Won = true;
                return true;
            }
            DoBossRound();
            if (PlayerStatus.hitPoints <= 0)
                return false;
            if (boss.hp <= 0)
                Won = true;
            return true;
        }

        private bool TryDoPlayerTurnAndAddSpell(Spell spell)
        {
            if (damagePlayer)
            {
                PlayerStatus.hitPoints--;
                if (PlayerStatus.hitPoints <= 0)
                    return false;
            }
            ProcessEffects();
            usedSpells.Add(spell);
            if (boss.hp <= 0)
            {
                Won = true;
                return true;
            }
            if (activeEffects.ContainsKey(spell))
                return false;
            if (spell.effectiveTurns > 0)
                activeEffects[spell] = spell.effectiveTurns;
            PlayerStatus.manaPoints -= spell.cost;
            if (PlayerStatus.manaPoints < 0)
                return false;
            boss.hp -= spell.initialDamage;
            PlayerStatus.hitPoints += spell.initialHeal;
            if (spell.effectArmor > 0)
            {
                PlayerStatus.armor += spell.effectArmor;
            }
            return true;
        }

        private void DoBossRound()
        {
            ProcessEffects();
            if (boss.hp <= 0)
            {
                Won = true;
                return;
            }
            PlayerStatus.TakeDamage(boss.damage);
        }

        private void ProcessEffects()
        {
            foreach (var effect in activeEffects.Keys)
            {
                PlayerStatus.manaPoints += effect.effectMana;
                boss.hp -= effect.effectDamage;
                activeEffects[effect] -= 1;
            }
            foreach (var effect in activeEffects.Where(e => e.Value <= 0).ToList())
            {
                if (effect.Key.effectArmor > 0)
                {
                    PlayerStatus.armor -= effect.Key.effectArmor;
                }
                activeEffects.Remove(effect.Key);
            }
        }

        public int GetMpSpent()
        {
            return usedSpells.Sum(us => us.cost);
        }

        public void LogBattle()
        {
            var battle = new Battle();
            int roundNo = 0;
            foreach (var spell in usedSpells)
            {
                roundNo++;
                battle.TryDoPlayerTurnAndAddSpell(spell);
                Console.WriteLine($"R{roundNo} {spell.name}: {battle.PlayerStatus.hitPoints} hp, {battle.PlayerStatus.armor} armor, {battle.PlayerStatus.manaPoints} mana. Boss at {battle.boss.hp}. Effects {string.Join(", ", battle.activeEffects.Select(kvp => $"{kvp.Key.name}:{kvp.Value}"))}");
                roundNo++;
                if (battle.boss.hp > 0)
                    battle.DoBossRound();
                Console.WriteLine($"R{roundNo} Boss: {battle.PlayerStatus.hitPoints} hp, {battle.PlayerStatus.armor} armor, {battle.PlayerStatus.manaPoints} mana. Boss at {battle.boss.hp}. Effects {string.Join(", ", battle.activeEffects.Select(kvp => $"{kvp.Key.name}:{kvp.Value}"))}");
            }

        }
    }

    private class Boss
    {
        public Boss() { }
        public Boss(Boss boss)
        {
            hp = boss.hp;
            damage = boss.damage;
        }
        public int hp = AdventData2015.Day22BossHp;
        public int damage = AdventData2015.Day22BossDamage;
    }

    private class Player
    {
        public int hitPoints = 50;
        public int manaPoints = 500;
        public int armor = 0;
        public Player() { }
        public Player(Player player)
        {
            hitPoints = player.hitPoints;
            manaPoints = player.manaPoints;
            armor = player.armor;
        }

        internal void TakeDamage(int damage)
        {
            hitPoints -= Math.Max(1, damage - armor);
        }
    }

    private class Spell(string name, int cost)
    {
        public string name = name;
        public int cost = cost;
        public int initialDamage = 0;
        public int initialHeal = 0;
        public int effectiveTurns = 0;
        public int effectDamage = 0;
        public int effectArmor = 0;
        public int effectMana = 0;
    }

    private static List<Spell> availableSpells = [
        new("Misile", 53){initialDamage = 4 },
        new("Drain", 73){initialDamage = 2, initialHeal = 2 },
        new("Shield", 113){effectiveTurns = 6, effectArmor = 7 },
        new("Poison", 173){effectiveTurns = 6, effectDamage = 3 },
        new("Recharge", 229){effectiveTurns = 5, effectMana = 101 }
        ];
}
