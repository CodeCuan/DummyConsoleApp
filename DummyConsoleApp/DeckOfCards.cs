using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyConsoleApp
{
    internal class DeckOfCards
    {
        public List<Card> cards;

        public DeckOfCards(bool initialize = true) {
            if (!initialize)
                return;
            Init();
        }
        public void Init() {
            try
            {
                BuildDeck();

            }catch(Exception ex)
            {
                Console.WriteLine("Error, failed to build deck: " + ex.Message);
            }
        }
        public void BuildDeck() {
            cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 1; i <= 13; i++)
                {
                    cards.Add(new Card(suit, i));
                }
            }
        }
        public void ShuffleDeck() {
            cards.Shuffle();
        }
        public Card Deal() {
            if (cards.Count == 0)
                throw new Exception("Deck has no cards left");
            var returnCard = cards.First();
            cards.RemoveAt(0);
            return returnCard;
        }
        public IEnumerable<Card> Deal(int cardsCount) {
            for (int i = 0; i < cardsCount; i++) {
                yield return Deal();
            }
        }
        public class Card
        {
            public override string ToString()
            {
                return $"{suit.ToString().First()}{GetName()}";
            }
            private string GetName() {
                switch (royal) {
                    case RoyalCard.None:
                        if (isAce)
                            return "A";
                        return number.ToString();
                    default:
                        return royal.ToString().First().ToString();
                }
            }
            public Card(Suit suit, int number) {
                this.suit = suit;
                this.number = number;
                isAce = number == 1;
                switch (number) {
                    case 11:
                        this.royal = RoyalCard.Jack;
                        this.number = 10;
                        break;
                    case 12:
                        this.royal = RoyalCard.Queen;
                        this.number = 10;
                        break;
                    case 13:
                        this.royal = RoyalCard.King;
                        this.number = 10;
                        break;
                }
            }
            public Suit suit;
            public RoyalCard royal = RoyalCard.None;
            public int number;
            public bool isAce = false;
        }
        public enum RoyalCard {None, King, Queen, Jack }
        public enum Suit {Hearts, Diamonds, Clubs, Spades }
    }
}
