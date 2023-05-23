using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Players;
using System.Collections.Generic;

namespace CoupGame.GameLogic.FSM
{
	// State to allow current player to challenge the blocking action from another player
	public class AskForChallengeState : State
	{
		private Player _currentPlayer;
		private Action _firstPlayerAction;

		private Player _blocker;

		public AskForChallengeState(FiniteStateMachine fsm, Player blocker) : base(fsm)
		{
			_blocker = blocker;
		}

		public override void Enter()
		{
			_currentPlayer = Fsm.Game.GetCurrentPlayer();
			_firstPlayerAction = _currentPlayer.CurrentAction;

			ActionContext ctx = new(Fsm.Game, _currentPlayer);

			// Ask current player to accept the block or challenge it
			List<Action> actions = new()
			{
				new Default(ctx, "Accept block", $"Accept the block from {_blocker.Name}"), // Accept the block
				new Challenge(ctx, _blocker, _blocker.CurrentAction) // Challenge the block
			};

			Dictionary<Player, List<Action>> actionsForPlayers = new()
			{
				{ _currentPlayer, actions }
			};

			Fsm.Game.SendActionsToPlayers(actionsForPlayers);
		}

		public override State Update()
		{
			// Wait until player chooses an action
			if (_currentPlayer.CurrentAction != null)
			{
				Action action = _currentPlayer.CurrentAction;

				// Current player ignores the block?
				if (action is Default)
				{
					// Block succeeds
					_blocker.CurrentAction.Perform();

					// Is the current player the only one remaining?
					if (Fsm.Game.NextTurn() != null)
					{
						return new PlayerTakeActionState(Fsm);
					}
				}
				// Current player challenges the blocker
				else
				{
					_currentPlayer.CurrentAction = _firstPlayerAction;
					
					// Tell both players to show the correct influence
					return new SolveChallengeState(Fsm, _currentPlayer, _blocker);
				}
			}

			return null;
		}

		public override void Exit()
		{

		}
	}
}
