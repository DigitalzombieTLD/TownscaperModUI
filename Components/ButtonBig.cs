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
	public class ButtonBig : MonoBehaviour
	{
		public Button thisButton;
		public Image buttonImage;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public string thissection = "";


		public ButtonBig(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color)
		{
			ManualAwake();
			textField.text = label;
			buttonImage.color = color;
			
			if(section!="")
			{
				thissection = section;
			}			
		}

		[HideFromIl2Cpp]
		public void Setup(string label, Color32 color)
		{			
			Setup(label, label, color);
		}

		[HideFromIl2Cpp]
		public void Setup(string label)
		{
			Color32 randomColor = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
			Setup(label, randomColor);
		}

		public void ManualAwake()
		{
			thisButton = GetComponent<Button>();
			textField = GetComponentInChildren<TextMeshProUGUI>();
			buttonImage = thisButton.image;
			thisRect = GetComponent<RectTransform>();			

			Quaternion randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
			thisRect.rotation = randomRotation;			
		}	
	}
}
