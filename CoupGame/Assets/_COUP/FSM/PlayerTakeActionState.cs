using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Players;
using System.Collections.Generic;

namespace CoupGame.GameLogic.FSM
{
	// First state of a turn, where the available actions are sent to the current player
	public class PlayerTakeActionState : State
	{
		private List<Action> _availableActions;

		private Player _currentPlayer;

		public PlayerTakeActionState(FiniteStateMachine fsm) : base(fsm)
		{

		}

		public override void Enter()
		{
			_currentPlayer = Fsm.Game.GetCurrentPlayer();
			List<Player> otherPlayers = Fsm.Game.GetPlayersExcept(_currentPlayer);

			ActionContext ctx = new(Fsm.Game, _currentPlayer);

			_availableActions = new();

			// 10 or more coins forces player to use Coup
			if (_currentPlayer.Coins >= 10)
			{
				foreach (Player other in otherPlayers)
				{
					_availableActions.Add(new Coup(ctx, other));
				}
			}
			// Otherwise fill all possible actions
			else
			{
				List<Action> actions = FillActions(ctx, otherPlayers);
				List<Action> affordableACtions = RemoveUnaffordableActions(actions, _currentPlayer.Coins);
				_availableActions.AddRange(affordableACtions);
			}

			Dictionary<Player, List<Action>> actionsForPlayers = new()
			{
				{ _currentPlayer, _availableActions }
			};

			Fsm.Game.SendActionsToPlayers(actionsForPlayers);
		}

		public override State Update()
		{
			// Wait until player has chosen an action
			if (_currentPlayer.CurrentAction != null)
			{
				Action action = _currentPlayer.CurrentAction;

				// If this action can be blocked,
				// ask other players if they want to block it
				if (action.BlockAction != null)
				{
					return new OthersBlockState(Fsm, action);
				}

				// This action does not require a card, cannot be challenged
				if (action.RequiredCard.Count <= 0)
				{
					// Perform the action
					action.Perform();

					// Is the current player the only one remaining?
					if (Fsm.Game.NextTurn() != null)
					{
						return new PlayerTakeActionState(Fsm);
					}
				}
				// This action requires a card, can be challenged
				else
				{
					// Ask other players if they want to challenge current player's action
					return new ReceiveChallengeState(Fsm, action);
				}
			}

			return null;
		}

		public override void Exit()
		{
			
		}

		private List<Action> FillActions(ActionContext ctx, List<Player> otherPlayers)
		{
			List<Action> actions = new();

			actions.Add(new Income(ctx));

			actions.Add(new ForeignAid(ctx));

			foreach (Player other in otherPlayers)
			{
				actions.Add(new Coup(ctx, other));
			}

			actions.Add(new Tax(ctx));

			foreach (Player other in otherPlayers)
			{
				actions.Add(new Steal(ctx, other));
			}

			foreach (Player other in otherPlayers)
			{
				actions.Add(new Assassinate(ctx, other));
			}

			actions.Add(new Exchange(ctx));

			return actions;
		}

		private List<Action> RemoveUnaffordableActions(List<Action> actions, int coins)
		{
			List<Action> affordableActions = new();

			foreach (Action action in actions)
			{
				if (coins >= action.CoinsNeeded)
				{
					affordableActions.Add(action);
				}
			}

			return affordableActions;
		}
	}
}
