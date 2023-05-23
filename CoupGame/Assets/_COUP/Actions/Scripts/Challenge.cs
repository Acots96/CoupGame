using CoupGame.GameLogic.Players;

namespace CoupGame.GameLogic.Actions
{
	// Class used to send a challenge action from one player to another
	public class Challenge : Action
	{
		public Challenge(ActionContext context, Player otherPlayer, Action actionToChallenge) : base(context)
		{
			Name = $"Challenge {otherPlayer.Name}";
			EffectDescription = $"Challenge {otherPlayer.Name} to show the required influence to use {actionToChallenge.Name}";
		}

		public override void Perform()
		{

		}
	}
}
