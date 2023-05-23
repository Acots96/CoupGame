
namespace CoupGame.GameLogic.Actions
{
	// Class used to create "fake" actions to allow the player to choose options
	// like accepting a block or a challenge, for example
	public class Default : Action
	{
		public Default(ActionContext context, string name, string definition) : base(context)
		{
			if (!string.IsNullOrEmpty(name))
			{
				Name = name;
			}
			if (!string.IsNullOrEmpty(definition))
			{
				EffectDescription = definition;
			}
		}

		public override void Perform()
		{

		}
	}
}
