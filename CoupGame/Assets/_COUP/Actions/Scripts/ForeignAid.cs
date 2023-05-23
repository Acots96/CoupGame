
namespace CoupGame.GameLogic.Actions
{
	public class ForeignAid : Action
    {
		public override BlockAction BlockAction => new BlockForeignAid(Context, this);

		public ForeignAid(ActionContext context) : base(context)
		{
			Name = "Foreign Aid";
			EffectDescription = "Take 2 coins";
		}

		public override void Perform()
		{
			Context.CurrentPlayer.AddCoins(Context.Game.RemoveCoins(2));
		}
	}
}
