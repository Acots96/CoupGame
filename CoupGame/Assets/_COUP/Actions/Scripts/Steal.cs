using CoupGame.GameLogic.Players;
using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class Steal : Action
    {
		public override List<CardType> RequiredCard => new() { CardType.Captain };

		public override BlockAction BlockAction => new BlockSteal(Context, this);

		private Player _stealVictim;

		public Steal(ActionContext context, Player stealVictim) : base(context)
		{
			Name = "Steal";
			EffectDescription = $"Take 2 coins from {stealVictim.Name}";

			_stealVictim = stealVictim;
		}

		public override void Perform()
		{
			Context.CurrentPlayer.AddCoins(_stealVictim.RemoveCoins(2));
		}
	}
}
