using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CoupGame.UI
{
	public class CoinsHolderUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text _coinsNumber;

		public void Awake()
		{
			_coinsNumber.text = "0";
		}

		public void SetCoins(int coins)
		{
			_coinsNumber.text = coins.ToString();
		}
	}
}
