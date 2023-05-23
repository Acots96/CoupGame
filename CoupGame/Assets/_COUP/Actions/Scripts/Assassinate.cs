using CoupGame.GameLogic.Players;
using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class Assassinate : Action
    {
		public override int CoinsNeeded => 3;
		public override List<CardType> RequiredCard => new() { CardType.Assassin };

		public override BlockAction BlockAction => new BlockAssassination(Context, this);

		private Player _assassinVictim;

		public Assassinate(ActionContext context, Player assassinVictim) : base(context)
		{
			Name = $"Assassinate {assassinVictim.Name}";
			EffectDescription = $"Pay 3 coins (choose {assassinVictim.Name} to lose influence)";

			_assassinVictim = assassinVictim;
		}

		public override void Perform()
		{
			Context.Game.AddCoins(Context.CurrentPlayer.RemoveCoins(CoinsNeeded));
			_assassinVictim.LoseRandomInfluence();
		}
	}
}
