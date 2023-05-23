using CoupGame.GameLogic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoupGame.GameLogic
{
	// Class to store the data of the deck (coins and cards)
	// (A record would be a better type to store this information but gives a compiler error)
	public class DeckData
    {
        public int Coins;
        public CardType[] Cards;
    }

	// Class used to store deck's data and the necessary methods to manage it
	public class CourtDeck
    {
        private int _coins;
        private List<Card> _cards;

        public CourtDeck(int coins, List<Card> cards)
        {
            _coins = coins;

            _cards = new List<Card>(cards);
            Shuffle();
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
        }

        // Return the amount removed
        public int RemoveCoins(int coins)
        {
            int c = _coins;
            _coins = Math.Max(0, _coins - coins);
            return c - _coins;
        }

        public void PushCard(Card card, bool shuffle)
        {
            _cards.Add(card);

            if (shuffle)
            {
                Shuffle();
            }
        }

        public Card PopCard()
        {
            if (_cards.Count == 0)
            {
                return null;
            }

            int last = _cards.Count - 1;

			Card c = _cards[last];
            _cards.RemoveAt(last);

            return c;
        }

        public void Shuffle()
        {
            Random random = new();

            Card c;
			for (int i = 0; i < _cards.Count; i++)
			{
				int r = i + (int)(random.NextDouble() * (_cards.Count - i));
				c = _cards[r];
				_cards[r] = _cards[i];
				_cards[i] = c;
			}
		}

        public DeckData GetData()
        {
            return new DeckData()
            {
                Coins = _coins,
                Cards = _cards.Select(x => x.Type).ToArray()
			};
        }
    }
}
