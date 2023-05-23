using CoupGame.GameLogic.Cards;
using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class Exchange : Action
    {
		public override List<CardType> RequiredCard => new() { CardType.Ambassador };

		public Exchange(ActionContext context) : base(context)
		{
			Name = "Exchange";
			EffectDescription = "Take 2 random cards from court deck, choose one to exchange with your face-down cards. Then return 2 cards to court deck";
		}

		public override void Perform()
		{
			// Get 2 cards from the deck
			Card fromDeck1 = Context.Game.TakeCardFromCourtDeck();
			Card fromDeck2 = Context.Game.TakeCardFromCourtDeck();

			// (this should be decided by the player...)

			// Get the card discarded by the player
			Card chosenToReturnFromPlayerCards = Context.CurrentPlayer.RemoveAnyCardType();

			// Choose which one of the deck will have the player
			// and which one will be returned to the deck
			if (new System.Random().NextDouble() < 0.5)
			{
				Context.CurrentPlayer.AddCard(fromDeck1);
				Context.Game.ReturnCardToCourtDeck(fromDeck2);
			}
			else
			{
				Context.CurrentPlayer.AddCard(fromDeck2);
				Context.Game.ReturnCardToCourtDeck(fromDeck1);
			}

			// Return to the deck the card chosen by the player
			Context.Game.ReturnCardToCourtDeck(chosenToReturnFromPlayerCards);
		}
	}
}
