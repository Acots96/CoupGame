using CoupGame.GameLogic.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoupGame.GameLogic.Players
{
	// Class to store the data of a Player (name, coins and cards)
	// (A record would be a better type to store this information but gives a compiler error)
	public class PlayerData
	{
		public string Name;
		public int Coins;
		public CardType[] AvailableInfluences, LostInfluences;
	}

	// Class used to store player's data and the necessary methods to manage it
	public class Player
	{
		public string Name { get; private set; }
		public int Coins { get; private set; }

		public bool IsPlaying { get; set; } = true;

		private List<Card> _availableInfluences, _lostInfluences;

		public Actions.Action CurrentAction { get; set; }

		public Player(string name)
		{
			Name = name;
			_availableInfluences = new();
			_lostInfluences = new();
		}

		public void AddCoins(int coins)
		{
			Coins += coins;
		}

		public int RemoveCoins(int coins)
		{
			int current = coins;
			Coins = Mathf.Max(0, Coins - coins);
			return current - Coins;
		}

		public void AddCard(Card card)
		{
			_availableInfluences.Add(card);
		}

		public Card RemoveAnyCardType(params CardType[] cardTypes)
		{
			if (_availableInfluences.Count == 0)
			{
				return null;
			}

			if (cardTypes == null || cardTypes.Length == 0)
			{
				Card c = _availableInfluences[new System.Random().Next(_availableInfluences.Count)];
				_availableInfluences.Remove(c);
				return c;
			}
			else
			{
				List<CardType> types = new(cardTypes);
				Card c = _availableInfluences.FirstOrDefault(x => types.Contains(x.Type));
				_availableInfluences.Remove(c);
				return c;
			}
		}

		public bool HasAnyInfluence(params CardType[] cardTypes)
		{
			List<CardType> types = new(cardTypes);
			return _availableInfluences.Any(x => types.Contains(x.Type));
		}

		public void LoseRandomInfluence()
		{
			int idx = new System.Random().Next(_availableInfluences.Count);

			Card influence = _availableInfluences[idx];
			_availableInfluences.RemoveAt(idx);
			_lostInfluences.Add(influence);
		}

		public PlayerData GetInfo()
		{
			return new()
			{
				Name = Name,
				Coins = Coins,
				AvailableInfluences = _availableInfluences.Select(x => x.Type).ToArray(),
				LostInfluences = _lostInfluences.Select(x => x.Type).ToArray()
			};
		}
	}
}
