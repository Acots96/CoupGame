using CoupGame.GameLogic.Players;
using System.Collections.Generic;

namespace CoupGame.GameLogic.Actions
{
	// Class to store the name and the description (effect) of an action
	public struct ActionData
	{
		public string Name;
		public string Description;
	}

	// Class used by the action to have access to the game and the current player
	public class ActionContext
	{
		public CoupGame Game { get; set; }
		public Player CurrentPlayer { get; set; }

		public ActionContext(CoupGame game, Player currentPlayer)
		{
			Game = game;
			CurrentPlayer = currentPlayer;
		}
	}

	// Base class for any action in the game
	public abstract class Action
	{
		public virtual string Name { get; protected set; }
		public virtual string EffectDescription { get; protected set; }

		public virtual int CoinsNeeded => 0;
		/// <summary>
		/// Only one of these is required
		/// </summary>
		public virtual List<CardType> RequiredCard => new();

		// Action required to block this action (Some actions can be blocked)
		public virtual BlockAction BlockAction => null;

		protected ActionContext Context { get; private set; }

		protected Action(ActionContext context)
		{
			Context = context;
		}

		public abstract void Perform();

		public virtual ActionData GetData()
		{
			return new ActionData() { Name = Name, Description = EffectDescription };
		}
	}
}
