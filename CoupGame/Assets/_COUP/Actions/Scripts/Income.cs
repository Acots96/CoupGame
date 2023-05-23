
namespace CoupGame.GameLogic.Actions
{
	public class Income : Action
    {
		public Income(ActionContext context) : base(context)
		{
			Name = "Income";
			EffectDescription = "Take 1 coin";
		}

		public override void Perform()
		{
			Context.CurrentPlayer.AddCoins(Context.Game.RemoveCoins(1));
		}
	}
}
