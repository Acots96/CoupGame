using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Players;
using System.Collections.Generic;
using System.Linq;

namespace CoupGame.GameLogic.FSM
{
	// State to allow other players block current player's action
	public class OthersBlockState : State
	{
		private Action _actionToBlock;

		private Player _currentPlayer;
		private List<Player> _otherPlayers;

		public OthersBlockState(FiniteStateMachine fsm, Action actionToBlock) : base(fsm)
		{
			_actionToBlock = actionToBlock;
		}

		public override void Enter()
		{
			_currentPlayer = Fsm.Game.GetCurrentPlayer();
			_otherPlayers = Fsm.Game.GetPlayersExcept(_currentPlayer);

			ActionContext ctx = new(Fsm.Game, _currentPlayer);

			// Ask other players if any want to block current player's action
			List<Action> actions = new()
			{
				new Default(ctx, "Ignore", $"Ignore {_currentPlayer.Name} {_actionToBlock.Name}"),
				_actionToBlock.BlockAction
			};

			Dictionary<Player, List<Actions.Action>> actionsForPlayers = new();
			foreach (Player other in _otherPlayers)
			{
				actionsForPlayers.Add(other, actions);
			}

			Fsm.Game.SendActionsToPlayers(actionsForPlayers);
		}

		public override State Update()
		{
			// Wait until all other players choose an action
			if (_otherPlayers.All(x => x.CurrentAction != null))
			{
				// Get all other players who blocked current player's action
				List<Player> blockingPlayers = new();
				foreach (Player p in _otherPlayers)
				{
					if (p.CurrentAction is BlockAction)
					{
						blockingPlayers.Add(p);
					}
				}

				// No one wants to block? Next turn
				if (blockingPlayers.Count <= 0)
				{
					_currentPlayer.CurrentAction.Perform();

					// Is the current player the only one remaining?
					if (Fsm.Game.NextTurn() != null)
					{
						return new PlayerTakeActionState(Fsm);
					}
				}
				// At list 1 other player wants to block,
				else
				{
					// Ask current player to challenge the blocking or not
					return new AskForChallengeState(Fsm, Fsm.Game.ChooseRandomPlayer(blockingPlayers));
				}
			}

			return null;
		}

		public override void Exit()
		{

		}
	}
}
