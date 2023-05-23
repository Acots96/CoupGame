using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class Tax : Action
    {
		public override List<CardType> RequiredCard => new() { CardType.Duke };

		public Tax(ActionContext context) : base(context)
		{
			Name = "Tax";
			EffectDescription = "Take 3 coins";
		}

		public override void Perform()
		{
			Context.CurrentPlayer.AddCoins(Context.Game.RemoveCoins(3));
		}
	}
}
