using CoupGame.GameLogic.Players;

namespace CoupGame.GameLogic.Actions
{
	public class Coup : Action
    {
		public override int CoinsNeeded => 7;

		private Player _coupVictim;

		public Coup(ActionContext context, Player coupVictim) : base(context)
		{
			Name = $"Coup to {coupVictim.Name}";
			EffectDescription = $"Pay 7 coins (choose {coupVictim.Name} to lose influence)";

			_coupVictim = coupVictim;
		}

		public override void Perform()
		{
			Context.Game.AddCoins(Context.CurrentPlayer.RemoveCoins(CoinsNeeded));
			_coupVictim.LoseRandomInfluence();
		}
	}
}
