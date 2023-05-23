using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoupGame.UI
{
	public class StartPopup : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown _optionsDropdown;
		[SerializeField] private Button _startButton;

		public int Players { get; private set; }

		public System.Action<int> OnStart;

		// Start is called before the first frame update
		private void Start()
		{
			_startButton.onClick.AddListener(OnStartButton);
		}

		private void OnStartButton()
		{
			_startButton.interactable = false;
			_optionsDropdown.interactable = false;

			OnStart?.Invoke(_optionsDropdown.value);
			gameObject.SetActive(false);
		}
	}
}
