using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class BlockAssassination : BlockAction
	{
		public override List<CardType> RequiredCard => new() { CardType.Contessa };

		public BlockAssassination(ActionContext context, Action actionToBlock) : base(context, actionToBlock)
		{
			Name = "Block Assassination";
			EffectDescription = $"Block Assassination action from {context.CurrentPlayer.Name}";
		}

		public override void Perform()
		{
			Context.Game.AddCoins(Context.CurrentPlayer.RemoveCoins(3));
		}
	}
}
