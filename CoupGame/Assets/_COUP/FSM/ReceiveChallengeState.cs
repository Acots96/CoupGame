using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Players;
using System.Collections.Generic;
using System.Linq;

namespace CoupGame.GameLogic.FSM
{
	// State to ask other players if any wants to challenge current player's action
	public class ReceiveChallengeState : State
	{
		private Action _actionToChallenge;

		private Player _currentPlayer;
		private List<Player> _otherPlayers;

		public ReceiveChallengeState(FiniteStateMachine fsm, Action actionToChallenge) : base(fsm)
		{
			_actionToChallenge = actionToChallenge;
		}

		public override void Enter()
		{
			_currentPlayer = Fsm.Game.GetCurrentPlayer();
			_otherPlayers = Fsm.Game.GetPlayersExcept(_currentPlayer);

			ActionContext ctx = new(Fsm.Game, _currentPlayer);

			// Ask other players if they want to challenge current player's action
			Dictionary<Player, List<Action>> actionsForPlayers = new();
			foreach (Player player in _otherPlayers)
			{
				List<Action> actions = new()
				{
					new Default(ctx, "Ignore", $"Ignore {_currentPlayer.Name} {_actionToChallenge.Name}"),
					new Challenge(ctx, _currentPlayer, _actionToChallenge)
				};
				actionsForPlayers.Add(player, actions);
			}
			Fsm.Game.SendActionsToPlayers(actionsForPlayers);
		}

		public override State Update()
		{
			// Wait until all other players choose an action
			if (_otherPlayers.All(x => x.CurrentAction != null))
			{
				List<Player> challengers = new();
				foreach (Player p in _otherPlayers)
				{
					if (p.CurrentAction is Challenge)
					{
						challengers.Add(p);
					}
				}

				// If anyone wants to challenge
				if (challengers.Count > 0)
				{
					Player challenger = Fsm.Game.ChooseRandomPlayer(challengers);
					return new SolveChallengeState(Fsm, challenger, _currentPlayer);
				}
				// No one wants to challenge? Next turn
				else
				{
					// Is the current player the only one remaining?
					if (Fsm.Game.NextTurn() != null)
					{
						return new PlayerTakeActionState(Fsm);
					}
				}
			}

			return null;
		}

		public override void Exit()
		{

		}
	}
}
