using CoupGame.GameLogic.Actions;
using CoupGame.GameLogic.Cards;
using CoupGame.GameLogic.Players;
using System.Collections.Generic;

namespace CoupGame.GameLogic.FSM
{
	// State that solves a challenge between 2 players and moves on to the next turn
	public class SolveChallengeState : State
	{
		private Player _challenger, _challenged;

		public SolveChallengeState(FiniteStateMachine fsm, Player challenger, Player challenged) : base(fsm)
		{
			_challenger = challenger;
			_challenged = challenged;
		}

		public override void Enter()
		{
			Action challengedAction = _challenged.CurrentAction;

			Player winner, loser;

			// Has the challenged player the required card for this challenge?
			if (_challenged.HasAnyInfluence(challengedAction.RequiredCard.ToArray()))
			{
				// Challenged player wins, challenger loses
				winner = _challenged;
				loser = _challenger;
			}
			else
			{
				// Challenged player loses, challenger wins
				winner = _challenger;
				loser = _challenged;
			}

			// If the current player is the winner, perform the initial action
			Player current = Fsm.Game.GetCurrentPlayer();
			if (winner == current)
			{
				current.CurrentAction.Perform();
			}

			// Loser loses an influence
			loser.LoseRandomInfluence();

			// Winner returns the card to the deck, shuffles, and gets a new card
			Card removedCard = winner.RemoveAnyCardType(winner.CurrentAction.RequiredCard.ToArray());
			Card newCard = Fsm.Game.ReturnCardAndGetCard(removedCard);
			winner.AddCard(newCard);

			// Ask players to move to next turn

			winner.CurrentAction = null;
			loser.CurrentAction = null;

			ActionContext ctx = new(Fsm.Game, Fsm.Game.GetCurrentPlayer());

			Dictionary<Player, List<Action>> actionsForPlayers = new()
			{
				{ winner, new() { new Default(ctx, "Challenge won", $"You won the challenge {winner.Name}!") } },
				{ loser, new() { new Default(ctx, "Challenge lost", $"You lost the challenge {loser.Name}...") } }
			};

			Fsm.Game.SendActionsToPlayers(actionsForPlayers);
		}

		public override State Update()
		{
			// Wait until both players select the "End turn" action
			if (_challenger.CurrentAction is Default 
				&& _challenged.CurrentAction is Default)
			{
				// Is the current player the only one remaining?
				if (Fsm.Game.NextTurn() != null)
				{
					return new PlayerTakeActionState(Fsm);
				}
			}

			return null;
		}

		public override void Exit()
		{

		}
	}
}

