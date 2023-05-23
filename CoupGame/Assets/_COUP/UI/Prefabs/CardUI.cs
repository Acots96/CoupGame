using CoupGame.GameLogic.Players;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoupGame.UI
{
	// Class used to represent the visual data of a card
	public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		// A card has the main image and the snitcher image, which is used
		// to allow the player to know the color of the reversed card by moving
		// the mouse over the card and showing a little square with the color
		[SerializeField] private Image _cardImage, _snitcherImage;

		public Color ReverseColor { get; set; }
		public Color NormalColor { get; set; }

		private bool _isReversed;
		public bool IsReversed
		{
			get => _isReversed;
			set
			{
				_isReversed = value;
				if (_isReversed) _cardImage.color = ReverseColor;
				else _cardImage.color = NormalColor;
				// TODO: For some reason, the color will not change even though this is correct :(
				//Debug.Log($"========= CARD {_isReversed}: {_cardImage.color}");
			}
		}

		public bool CanSnitch { get; set; } = false;

		private void Start()
		{
			IsReversed = true;
			ResetSnitchCardColor();
		}

		public void UpdateParent(Transform newParent)
		{
			transform.parent = newParent;
			transform.localPosition = newParent.localPosition;
		}

		public void SnitchCardColor(bool snitch)
		{
			if (!CanSnitch)
			{
				ResetSnitchCardColor();
				return;
			}

			_snitcherImage.color = snitch ? NormalColor : ReverseColor;
		}

		public void ResetSnitchCardColor()
		{
			_snitcherImage.color = _cardImage.color;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			SnitchCardColor(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			SnitchCardColor(false);
		}
	}
}
