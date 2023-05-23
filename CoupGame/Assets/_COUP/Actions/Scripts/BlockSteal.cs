using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	public class BlockSteal : BlockAction
	{
		public override List<CardType> RequiredCard => new() { CardType.Ambassador, CardType.Captain };

		public BlockSteal(ActionContext context, Action actionToBlock) : base(context, actionToBlock)
		{
			Name = "Block Steal";
			EffectDescription = $"Block Steal action from {context.CurrentPlayer.Name}";
		}

		public override void Perform()
		{
			// Nothing happens because Steal does not pay coins,
			// and the action does not succeed.
		}
	}
}
