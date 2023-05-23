using CoupGame.GameLogic.Actions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoupGame.Controller
{
	// Actions controller for a real player, to show the options in the dropdown
	public class PlayerInputController : ActionsController
	{
		[SerializeField] private TMP_Dropdown _actionsDropdown;
		[SerializeField] private Button _chooseActionButton;

		public override void ChooseAction(List<ActionData> actions,
			Action<ActionsController, int> callback)
		{
			// Display every action as string in the dropdown 
			List<string> options = new();
			foreach (ActionData action in actions)
			{
				options.Add($"{action.Name}: {action.Description}");
			}

			_actionsDropdown.ClearOptions();
			_actionsDropdown.AddOptions(options);
			_actionsDropdown.gameObject.SetActive(true);

			// Add a listener to the button so it will call the callback method with
			// the value selected in the dropdown
			_chooseActionButton.onClick.RemoveAllListeners();
			_chooseActionButton.onClick.AddListener(delegate { callback(this, _actionsDropdown.value); });
			_chooseActionButton.gameObject.SetActive(true);
		}
	}
}
