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
    [RegisterTypeInIl2Cpp]
    public class ButtonVeryBig : MonoBehaviour
	{
		public Button thisButton;
		public Image buttonImage;
		public RawImage contentImage;

		public RectTransform thisRect;		
		public Action thisAction;

		public ButtonVeryBig(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public ButtonVeryBig Setup(string label, string section, Color32 color, Action newAction)
		{
			thisAction = newAction;
			ManualAwake();
			
			buttonImage.color = color;
			return this;
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
			buttonImage = thisButton.image;
			contentImage = GetComponentInChildren<RawImage>();

			thisRect = GetComponent<RectTransform>();

			thisButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(thisAction));
		}
	}
}
