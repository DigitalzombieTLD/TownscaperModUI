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
	public class ButtonSmall : MonoBehaviour
	{
		public Button thisButton;
		public Image buttonImage;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public Action thisAction;

		public ButtonSmall(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, Action newAction)
		{
			thisAction = newAction;
			ManualAwake();

			textField.text = label;
			buttonImage.color = color;
		}

		[HideFromIl2Cpp]
		public void Setup(string label, Color32 color, Action newAction)
		{			
			Setup(label, label, color, newAction);
		}

		[HideFromIl2Cpp]
		public void Setup(string label, Action newAction)
		{
			Color32 randomColor = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
			Setup(label, randomColor, newAction);
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
