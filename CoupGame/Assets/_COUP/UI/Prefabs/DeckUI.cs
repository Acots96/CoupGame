using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoupGame.UI
{
	// Class used to represent the visual data of the deck
	public class DeckUI : MonoBehaviour
	{
		[SerializeField] private CardsHolderUI _cardsHolder;
		public CardsHolderUI CardsHolder => _cardsHolder;

		[SerializeField] private CoinsHolderUI _coinsHolder;
		public CoinsHolderUI CoinsHolder => _coinsHolder;
	}
}
