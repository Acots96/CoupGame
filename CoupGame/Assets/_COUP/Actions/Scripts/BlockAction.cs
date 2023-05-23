
namespace CoupGame.GameLogic.Actions
{
	// Base class for any block action
	public abstract class BlockAction : Action
	{
		protected Action ActionToBlock;

		protected BlockAction(ActionContext context, Action actionToBlock) : base(context)
		{
			ActionToBlock = actionToBlock;
		}
	}
}
