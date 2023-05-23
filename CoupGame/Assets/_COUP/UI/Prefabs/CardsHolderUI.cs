using CoupGame.GameLogic.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoupGame.UI
{
	// Class used to represent the visual data of a cards group (players, deck)
	public class CardsHolderUI : MonoBehaviour
	{
		[SerializeField] private LayoutGroup _cardsLayout;

		private List<CardUI> _cards;

		private void Awake()
		{
			_cards = new();
		}

		public void AddCard(CardUI card)
		{
			_cards.Add(card);
			card.UpdateParent(_cardsLayout.transform);
		}

		public void ClearCards()
		{
			foreach (CardUI c in _cards)
			{
				Destroy(c.gameObject);
			}
			_cards.Clear();
		}
	}
}
