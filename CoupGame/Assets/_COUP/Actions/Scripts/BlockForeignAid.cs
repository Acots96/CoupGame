using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class BlockForeignAid : BlockAction
	{
		public override List<CardType> RequiredCard => new() { CardType.Duke };

		public BlockForeignAid(ActionContext context, Action actionToBlock) : base(context, actionToBlock)
		{
			Name = "Block Foreign Aid";
			EffectDescription = $"Block Foreign Aid action from {context.CurrentPlayer.Name}";
		}

		public override void Perform()
		{
			// Nothing happens because ForeignAid does not pay coins,
			// and the action does not succeed.
		}
	}
}
