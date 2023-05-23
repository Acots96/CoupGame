using CoupGame.GameLogic.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace CoupGame.Controller
{
	// Base class for any player that can take actions
	public abstract class ActionsController : MonoBehaviour
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="actions">List of available actions to choose</param>
		/// <param name="callback">Callback to call when the action is selected</param>
		public abstract void ChooseAction(List<ActionData> actions,
			System.Action<ActionsController, int> callback);
	}
}
