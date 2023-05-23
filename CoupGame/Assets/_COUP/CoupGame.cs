using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Cards;
using CoupGame.GameLogic.FSM;
using CoupGame.GameLogic.Players;

namespace CoupGame.GameLogic
{
	public enum CardType { Duke, Assassin, Captain, Ambassador, Contessa }

	// Main class of the game that stores all the information and the methods to manage it
	public class CoupGame
	{
		public const int INITIAL_COINS_COURTDECK = 50;
		public const int INITIAL_COINS_PLAYER = 2;
		public const int INITIAL_CARDS_PLAYER = 2;
		public const int MIN_PLAYERS = 3;
		public const int MAX_PLAYERS = 5;

		private int _coins;
		private CourtDeck _courtDeck;

		private List<Player> _players;
		private int _currentPlayer;

		private int _currentTurn;
		public int CurrentTurn => _currentTurn;

		private FiniteStateMachine _fsm;

		public bool IsFinished { get; private set; }

		public delegate void SendActions(
			Dictionary<int, List<ActionData>> actionsForPlayers,
			Action<Dictionary<int, int>> callback);

		/// <summary>
		/// Subscribe to this event to receive the available actions for each player
		/// and the method that must be called when everyone has chosen an action
		/// </summary>
		/// <param name="actionsForPlayers">Key: player index, Value: available actions</param>
		/// <param name="callback">Key: player index, Value: action index</param>
		public event SendActions PlayersChooseActions;

		public Action<string> GameOver;

		public Action<int> CurrentPlayerChanged;

		private Dictionary<Player, List<Actions.Action>> _currentAvailableActions;

		public CoupGame(int players)
		{
			InitializeCourtDeck();
			InitializePlayers(players);

			_fsm = new FiniteStateMachine(this);
			_fsm.Start();
		}

		public void Start()
		{
			_fsm.Start();
		}

		private void InitializeCourtDeck()
		{
			List<Card> cards = new()
			{
				new(CardType.Duke),
				new(CardType.Duke),
				new(CardType.Duke),
				new(CardType.Assassin),
				new(CardType.Assassin),
				new(CardType.Assassin),
				new(CardType.Captain),
				new(CardType.Captain),
				new(CardType.Captain),
				new(CardType.Ambassador),
				new(CardType.Ambassador),
				new(CardType.Ambassador),
				new(CardType.Contessa),
				new(CardType.Contessa),
				new(CardType.Contessa)
			};

			_courtDeck = new CourtDeck(INITIAL_COINS_COURTDECK, cards);

			_courtDeck.Shuffle();
		}

		private void InitializePlayers(int players)
		{
			players = Math.Clamp(players, MIN_PLAYERS, MAX_PLAYERS);

			_players = new List<Player>(players);
			for (int i = 0; i < players; i++)
			{
				Player p = new($"Player{i + 1}");

				// Cards
				for (int c = 0; c < INITIAL_CARDS_PLAYER; c++)
				{
					p.AddCard(_courtDeck.PopCard());
				}

				// Coins
				p.AddCoins(_courtDeck.RemoveCoins(INITIAL_COINS_PLAYER));

				_players.Add(p);
			}

			// Choosse first player randomly
			_currentPlayer = new System.Random().Next(_players.Count);
			CurrentPlayerChanged?.Invoke(_currentPlayer);
		}

		public void AddCoins(int coins)
		{
			_coins += coins;
		}

		public int RemoveCoins(int coins)
		{
			int current = coins;
			_coins = Mathf.Max(0, _coins - coins);
			return current - _coins;
		}

		public Player NextTurn()
		{
			Player current = GetCurrentPlayer();

			_currentPlayer++;
			Player next = GetCurrentPlayer();

			// Find the next player available
			while (!next.IsPlaying && next != current)
			{
				_currentPlayer++;
				next = GetCurrentPlayer();
			}

			if (next == current)
			{
				// current wins!
				GameOver?.Invoke(current.Name);
				return null;
			}
			else
			{
				_currentTurn++;
				CurrentPlayerChanged?.Invoke(_currentPlayer % _players.Count);
				return GetCurrentPlayer();
			}
		}

		public Player GetCurrentPlayer()
		{
			return _players[_currentPlayer % _players.Count];
		}

		/// <summary>
		/// Key: index of the active player (from 0 to players count - 1)
		/// Value: player's information
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, PlayerData> GetAllPlayersData()
		{
			Dictionary<int, PlayerData> infos = new();
			for (int i = 0; i < _players.Count; i++)
			{
				Player p = _players[i];
				if (p.IsPlaying)
				{
					infos.Add(i, p.GetInfo());
				}
			}
			return infos;
		}

		public DeckData GetDeckData()
		{
			return _courtDeck.GetData();
		}

		public void SendActionsToPlayers(Dictionary<Player, List<Actions.Action>> actionsForPlayers)
		{
			_currentAvailableActions = new(actionsForPlayers);

			Dictionary<int, List<ActionData>> actionsPlayers = new(actionsForPlayers.Count);
			foreach (var pair in actionsForPlayers)
			{
				Player player = pair.Key;
				List<Actions.Action> actions = pair.Value;

				actionsPlayers.Add(
					_players.IndexOf(player),
					actions.Select(x => x.GetData()).ToList()
				);
			}

			// Clear actions in players that are not in this dictionary
			/*foreach (Player player in _players)
			{
				if (!actionsForPlayers.TryGetValue(player, out _))
				{
					player.CurrentAction = null;
				}
			}*/

			PlayersChooseActions?.Invoke(actionsPlayers, SelectActions);
		}

		// Method called when all the players have chosen an action
		public void SelectActions(Dictionary<int, int> selectedActions)
		{
			// Update actions in players
			foreach (var pair in selectedActions)
			{
				int playerIdx = pair.Key;
				int selectedAction = pair.Value;

				Player player = _players[playerIdx];
				player.CurrentAction = _currentAvailableActions[player][selectedAction];
			}

			// Update teh states machine
			_fsm.Update();
		}

		public List<Player> GetPlayersExcept(Player player)
		{
			return _players.FindAll(x => x.IsPlaying && !x.Equals(player));
		}

		public Card TakeCardFromCourtDeck()
		{
			return _courtDeck.PopCard();
		}

		public void ReturnCardToCourtDeck(Card card)
		{
			_courtDeck.PushCard(card, true);
		}

		public Card ReturnCardAndGetCard(Card card)
		{
			_courtDeck.PushCard(card, true);
			return _courtDeck.PopCard();
		}

		public Player ChooseRandomPlayer(List<Player> players)
		{
			return players[new System.Random().Next(players.Count)];
		}
	}
}
