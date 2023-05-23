using CoupGame.GameLogic;
using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Players;
using CoupGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoupGame.Controller
{
	// Class acting as a controller and a bridge between the GameLogic an dthe UI
	public class GameManager : MonoBehaviour
	{
		public const int PLAYERS_COUNT = 4;

		[SerializeField] private TMP_Text _notificationsText;
		[SerializeField] private DeckUI _deckUI;
		[SerializeField] private List<PlayerUI> _players;

		[SerializeField] private Button _restartButton;

		[Space]

		// GameData holds all the visual data, even the prefabs, but dynamic instantiation
		// of visual elements is not implemented yet.
		[SerializeField] private GameData _gameData;
		private Dictionary<CardType, CardVisualData> _cardsVisualData;

		private GameLogic.CoupGame _game;

		private Dictionary<int, List<ActionData>> _actionsForPlayers;
		// Selected actions will be stored here
		private Dictionary<int, int> _selectedActions;
		// This action will be called when all available players have chosen an action
		private Action<Dictionary<int, int>> _actionsCallback;

		private void Awake()
		{
			_selectedActions = new();
		}

		private void Start()
		{
			Restart();
		}

		public void Restart()
		{
			_restartButton.interactable = false;

			_game = new(PLAYERS_COUNT);
			_game.PlayersChooseActions += OnSendActionsToPlayers;
			_game.CurrentPlayerChanged += OnCurrentPlayerChanged;
			_game.GameOver += OnGameOver;

			_cardsVisualData = new();
			foreach (CardVisualData data in _gameData.GetCardsVisualData())
			{
				_cardsVisualData.Add(data.Type, data);
			}

			InitializeDeckUI();
			InitializePlayersUI();

			_game.Start();
		}

		private void InitializeDeckUI()
		{
			DeckData data = _game.GetDeckData();

			// Coins
			_deckUI.CoinsHolder.SetCoins(data.Coins);

			// Cards
			// Just to show the image of a reverse card,
			// does not really matter which card or how many
			_deckUI.CardsHolder.ClearCards();
			_deckUI.CardsHolder.AddCard(GetCardUI(data.Cards[0]));
		}

		private void InitializePlayersUI()
		{
			Dictionary<int, PlayerData> playerDatas = _game.GetAllPlayersData();

			foreach (var playerData in playerDatas)
			{
				int idx = playerData.Key;
				PlayerData data = playerData.Value;

				PlayerUI player = _players[idx];

				// Name
				player.name = data.Name;
				player.Name = data.Name;

				// Coins
				player.CoinsHolder.SetCoins(data.Coins);

				// Cards
				player.CardsHolder.ClearCards();
				foreach (CardType type in data.AvailableInfluences)
				{
					CardUI c = GetCardUI(type);
					c.CanSnitch = idx == 0;
					player.CardsHolder.AddCard(c);
				}

				_players.Add(player);
			}
		}

		private CardUI GetCardUI(CardType type)
		{
			CardUI card = Instantiate(_gameData.CardPrefab).GetComponent<CardUI>();
			card.ReverseColor = _gameData.ReverseColor;
			card.NormalColor = _cardsVisualData[type].ImageColor;

			return card;
		}

		// Method called when the actions are ready to be sent to the players (event
		// Game.PlayersChooseActions)
		private void OnSendActionsToPlayers(
			Dictionary<int, List<ActionData>> actionsForPlayers,
			Action<Dictionary<int, int>> callback)
		{
			_actionsForPlayers = actionsForPlayers;

			// Actions are sent to each available player
			foreach (var pair in actionsForPlayers)
			{
				ActionsController controller = _players[pair.Key].GetComponent<ActionsController>();
				controller.ChooseAction(pair.Value, SelectAction);
			}

			// Callback method to call when all available players have chosen an action
			_actionsCallback = callback;

			// Update the UI elements
			UpdateUIElements();

			_selectedActions.Clear();
			// Wait until all players have chosen
			StartCoroutine(WaitForPlayersActions(actionsForPlayers));
		}

		// Method called when a single player has chosen an action
		private void SelectAction(ActionsController controller, int selectedAction)
		{
			PlayerUI player = controller.GetComponent<PlayerUI>();
			int playerIdx = _players.IndexOf(player);
			_selectedActions.Add(playerIdx, selectedAction);

			// Show the chosen action
			// (Only one action is shown in the UI,
			// the history of actions will be visible in the console)
			ActionData data = _actionsForPlayers[playerIdx][selectedAction];
			string notification = $"(TURN {_game.CurrentTurn + 1}) {player.Name} chose: {data.Name}, {data.Description}";
			_notificationsText.text = notification;
			Debug.Log(notification);
		}

		private IEnumerator WaitForPlayersActions(Dictionary<int, List<ActionData>> actionsForPlayers)
		{
			// Wait until all players have chosen
			yield return new WaitUntil(() => actionsForPlayers.Count == _selectedActions.Count);
			_actionsCallback?.Invoke(_selectedActions);

			// Update the UI elements
			UpdateUIElements();
		}

		private void UpdateUIElements()
		{
			// Deck
			DeckData deckData = _game.GetDeckData();
			_deckUI.CoinsHolder.SetCoins(deckData.Coins);

			// Players
			var playersData = _game.GetAllPlayersData();
			foreach (var data in playersData)
			{
				// Update player's coins
				PlayerUI player = _players[data.Key];
				player.CoinsHolder.SetCoins(data.Value.Coins);

				// Update player's influences
				player.CardsHolder.ClearCards();
				List<(CardType, bool)> cards = new();
				foreach (CardType type in data.Value.AvailableInfluences)
				{
					// Available influences
					CardUI c = GetCardUI(type);
					c.IsReversed = true;
					c.CanSnitch = data.Key == 0;
					player.CardsHolder.AddCard(c);
					
					cards.Add((type, false));
				}
				foreach (CardType type in data.Value.LostInfluences)
				{
					// Lost influences
					CardUI c = GetCardUI(type);
					c.IsReversed = false;
					player.CardsHolder.AddCard(c);
					
					cards.Add((type, true));
				}

				string s = "";
				foreach (var c in cards)
				{
					string avalable = c.Item2 ? "available" : "lost";
					s += $"{c.Item1} ({avalable}), ";
				}
				Debug.Log($"[{player.Name} influences: {s}");
			}
		}

		private void OnCurrentPlayerChanged(int currentPlayerIdx)
		{
			for (int i = 0; i < _players.Count; i++)
			{
				_players[currentPlayerIdx].ActivateTurn(i == currentPlayerIdx);
			}
		}

		// Method called when the game is over
		private void OnGameOver(string playerWon)
		{
			// Show a notification
			string notification = $"{playerWon} won!!!";
			_notificationsText.text = notification;
			Debug.Log(notification);

			// Activate the restart button
			_restartButton.interactable = true;
		}
	}
}
