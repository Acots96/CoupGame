using TMPro;
using UnityEngine;

namespace CoupGame.UI
{
	// Class used to manage the visual data of a player (coins and cards)
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text _nameText;

		[SerializeField] private CardsHolderUI _cardsHolder;
		public CardsHolderUI CardsHolder => _cardsHolder;

		[SerializeField] private CoinsHolderUI _coinsHolder;
		public CoinsHolderUI CoinsHolder => _coinsHolder;

		public string Name { get => _nameText.text; set => _nameText.text = value; }

		public void ActivateTurn(bool activate)
		{
			_nameText.fontStyle = activate ? FontStyles.Bold : FontStyles.Normal;
		}
	}
}
