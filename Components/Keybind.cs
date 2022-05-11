using MelonLoader;
using System;
using System.Collections.Generic;
using TMPro;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModUI
{
	public class Keybind : MonoBehaviour
	{
		public Button thisButton;
		public Image buttonImage;
		public TextMeshProUGUI textField;
		public TextMeshProUGUI contentField;
		public RectTransform thisRect;
		public Action thisAction;
		public KeyCode thisKeyCode;
		public bool waitingForInput;
		public ModSettings parentModSetting;
		public string thisSection;

		public Keybind(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, ModSettings thisModSetting)
		{
			parentModSetting = thisModSetting;
			thisSection = section;
			ManualAwake();

			textField.text = label;
			buttonImage.color = color;
		}

		[HideFromIl2Cpp]
		public void ManualAwake()
		{
			thisButton = GetComponent<Button>();

			textField = this.transform.Find("Label").GetComponent<TextMeshProUGUI>();
			contentField = this.transform.Find("Content").GetComponent<TextMeshProUGUI>();

			buttonImage = thisButton.image;
			thisRect = GetComponent<RectTransform>();

			thisAction = new Action(delegate { GetPressedKey(); });
			thisButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(thisAction));
		}

		[HideFromIl2Cpp]
		public void UpdateSettingsValue()
		{
			parentModSetting.SetValueKeyCode(textField.text, thisSection, parentModSetting.ConvertToKeyCode(contentField.text));
		}

		[HideFromIl2Cpp]
		public void GetPressedKey()
		{
			waitingForInput = true;
		}

		public void Update()
		{
			if(waitingForInput)
			{
				contentField.text = "Press key ...";

				if (Input.anyKeyDown)
				{
					if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
					{
						waitingForInput = false;
						contentField.text = thisKeyCode.ToString();
					}
					else if (Input.GetKeyDown(KeyCode.Delete))
					{
						waitingForInput = false;
						thisKeyCode = KeyCode.None;
						contentField.text = thisKeyCode.ToString();
						UpdateSettingsValue();
					}
					else
					{
						for (int i = 0; i < DZInput.keyCodes.Length; i++)
						{
							if (Input.GetKey(DZInput.keyCodes[i]))
							{
								waitingForInput = false;
								thisKeyCode = DZInput.keyCodes[i];
								contentField.text = thisKeyCode.ToString();
								UpdateSettingsValue();
							}								
						}							
					}
				}
			}
		}
	}
}
