using MelonLoader;
using System;
using TMPro;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModUI
{
	public class NetworkStatus : MonoBehaviour
	{
		public NetworkStatus thisNetworkStatus;
		public Image thisNetworkStatusBackground;
		public TextMeshProUGUI statusText;
		public TextMeshProUGUI playerCount;
		public RectTransform thisRect;
		public string thisSection;

		public NetworkStatus(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color)
		{			
			ManualAwake();

			thisNetworkStatusBackground.color = color;
			statusText.text = "<color=\"red\">Offline";
			playerCount.text = "0";
			thisSection = section;
		}
		
		public void ManualAwake()
		{
			thisNetworkStatus = GetComponent<NetworkStatus>();
			thisNetworkStatusBackground = GetComponent<Image>();

			statusText = this.gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
			playerCount = this.gameObject.transform.Find("PlayerCount").GetComponent<TextMeshProUGUI>();

			thisRect = GetComponent<RectTransform>();						
		}
	}
}
