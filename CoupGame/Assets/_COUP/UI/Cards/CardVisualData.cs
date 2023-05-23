using CoupGame.GameLogic;
using UnityEngine;

namespace CoupGame.UI
{
	// Class used to create scriptable objects with the visual data of a card
	// (it could have an image instead of a plain color)
	[CreateAssetMenu(fileName = "CardVisualData", menuName = "Coup/Card Visual Data")]
	public class CardVisualData : ScriptableObject
	{
		[SerializeField] private CardType _type;
		public CardType Type => _type;

		[SerializeField] private Color _imageColor;
		public Color ImageColor => _imageColor;
	}
}
