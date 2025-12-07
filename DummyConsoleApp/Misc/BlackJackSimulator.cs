using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ShellProgressBar;

#nullable disable
using static DummyConsoleApp.Misc.DeckOfCards;

namespace DummyConsoleApp.Misc
{
    internal class BlackJackSimulator
    {

        DeckOfCards deckManager = null;
        List<Card> cards = null;
        List<Card> dealerHand = new List<Card>();
        List<Card> playerHand = new List<Card>();
        public static void SimulateTests(int testCount, bool hit, bool logHands = false)
        {

            List<bool> results = new List<bool>();
            ProgressBar progressBar = null;
            if (!logHands)
                progressBar = new ProgressBar(testCount, "Running Tests");
            for (int i = 0; i < testCount; i++)
            {
                BlackJackSimulator sim = new BlackJackSimulator();
                if (logHands)
                    results.Add(sim.DoesWinLogHands(hit));
                else
                {
                    results.Add(sim.DoesWin(hit));
                    progressBar.Tick();
                }
            }
            if (!logHands)
                progressBar.Dispose();
            var wins = results.Count(result => result);

            Console.WriteLine($"Tests finished! {(hit ? "We chose to hit!" : "We chose to stand!")} {wins} out of {testCount} hands won.");
            Console.WriteLine($"Probability: ~{Math.Round(100M * wins / testCount, 2)}%");

        }
        public bool DoesWinLogHands(bool hit)
        {
            var doesWin = DoesWin(hit);
            Console.WriteLine($"We {(doesWin ? "Won" : "Lost")}! D{CountCards(dealerHand)} P{CountCards(playerHand)}");
            Console.WriteLine($"Full hands: Dealer {string.Join(", ", dealerHand)}");
            Console.WriteLine($"Full hands: Player {string.Join(", ", playerHand)}");


            return doesWin;
        }
        public bool DoesWin(bool hit)
        {
            deckManager = new DeckOfCards();
            cards = deckManager.cards;
            var dealerFirst = cards.First(card => card.number == 10);
            dealerHand.Add(dealerFirst);
            cards.Remove(dealerFirst);
            cards.Shuffle();
            dealerHand.Add(deckManager.Deal());
            if (dealerHand.Last().isAce)
                return false; //dealer blackjack
            playerHand = BuildDeckTo16().ToList();
            int playerValue;
            if (hit)
            {
                playerHand.Add(deckManager.Deal());
                playerValue = CountCards(playerHand);
                if (playerValue > 21)
                    return false;
            }
            else
                playerValue = playerHand.Count;
            var dealerValue = BuildDealerHand();
            if (dealerValue > 21)
                return true;
            return playerValue > dealerValue;
        }
        int BuildDealerHand()
        {
            while (true)
            {
                var dealerCount = CountCards(dealerHand);
                if (dealerCount > 16)
                    return dealerCount;
                dealerHand.Add(deckManager.Deal());
            }
        }
        List<Card> BuildDeckTo16()
        {
            int count = 0;
            int aces = 0;
            List<Card> playerHand = new List<Card>();
            foreach (var card in cards)
            {
                if (count + card.number <= 16)
                {
                    playerHand.Add(card);
                    count += card.number;
                    if (card.isAce)
                        aces++;
                    if (count == 16 || count == 6 && aces > 0)
                        break;
                    if (aces > 0)
                    {
                        var bestHand = CountCards(playerHand);
                        if (bestHand > 16)
                        {
                            deckManager.ShuffleDeck();
                            return BuildDeckTo16();//this is too good a hand, gotta bail
                        }
                    }
                }
            }
            foreach (var card in playerHand)
                cards.Remove(card);
            deckManager.ShuffleDeck();
            return playerHand;
        }

        public int CountCards(IEnumerable<Card> cardsToCount)
        {
            var total = 0;
            bool hasAces = false;
            foreach (var card in cardsToCount)
            {
                if (card.isAce)
                    hasAces = true;
                total += card.number;
            }
            if (total <= 11 && hasAces)
                total += 10;
            return total;
        }

    }
}
