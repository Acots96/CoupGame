using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoupGame.UI
{
	// This class is meant to hold the data of the game, including not only the cards
	// with their visual data, but also the prefabs used for each visual element
	[CreateAssetMenu(fileName = "GameData", menuName = "Coup/GameData")]
	public class GameData : ScriptableObject
	{
		[SerializeField] private Color _reverseColor;
		public Color ReverseColor => _reverseColor;

		[SerializeField] private List<CardVisualData> _cards;

		[Space]

		[SerializeField] private GameObject _playerInputPrefab;
		public GameObject PlayerInputPrefab => _playerInputPrefab;

		[SerializeField] private GameObject _playerAgentPrefab;
		public GameObject PlayerAgentPrefab => _playerAgentPrefab;

		[SerializeField] private GameObject _cardPrefab;
		public GameObject CardPrefab => _cardPrefab;

		[SerializeField] private GameObject _deckPrefab;
		public GameObject DeckPrefab => _deckPrefab;

		public List<CardVisualData> GetCardsVisualData()
		{
			return _cards;
		}
	}
}
