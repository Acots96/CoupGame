using CoupGame.GameLogic.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoupGame.Controller
{
	// Actions controller for an agent player to let the AI choose
	public class PlayerAIController : ActionsController
	{
		public override void ChooseAction(List<ActionData> actions,
			System.Action<ActionsController, int> callback)
		{
			StartCoroutine(DelayedChoice(actions, callback, 1f));
		}

		private IEnumerator DelayedChoice(List<ActionData> actions,
			System.Action<ActionsController, int> callback, float delay)
		{
			yield return new WaitForSeconds(delay);
			callback.Invoke(this, new System.Random().Next(actions.Count));
		}
	}
}
