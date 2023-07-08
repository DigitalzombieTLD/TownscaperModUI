using MelonLoader;
using System;
using TMPro;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModUI
{
    [RegisterTypeInIl2Cpp]
    public class PlayerButton : MonoBehaviour
	{
		public Button thisButton;
		public Image buttonImage;
		public uint playerID;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public Action thisAction;

		public PlayerButton(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(uint newplayerID, string label, string section, Color32 color, Action newAction)
		{
			thisAction = newAction;
			ManualAwake();

			playerID = newplayerID;
			textField.text = label;
			buttonImage.color = color;
		}
		
		public void ManualAwake()
		{
			thisButton = GetComponent<Button>();
			textField = GetComponentInChildren<TextMeshProUGUI>();
			buttonImage = thisButton.image;
			thisRect = GetComponent<RectTransform>();
					
			thisButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(thisAction));
		}
	}
}
