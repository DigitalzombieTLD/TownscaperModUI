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
    public class DZToggle : MonoBehaviour
	{
		public Toggle thisToggle;
		public Image toggleImage;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public Action<bool> thisAction;
		public ModSettings parentModSetting;
		public string thisSection;

		public DZToggle(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, Action<bool> newAction, ModSettings thisModSetting)
		{
			thisAction = newAction;
			parentModSetting = thisModSetting;
			thisSection = section;

			ManualAwake();
			toggleImage.color = color;
			textField.text = label;			
		}

		[method: HideFromIl2Cpp]
		public void ManualAwake()
		{
			thisToggle = GetComponent<Toggle>();
			textField = GetComponentInChildren<TextMeshProUGUI>();
			toggleImage = thisToggle.image;
			thisRect = GetComponent<RectTransform>();

			thisToggle.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(thisAction));
			thisToggle.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(new Action<bool>(delegate (bool value) { UpdateSettingsValue(); })));
		}

		[HideFromIl2Cpp]
		public void UpdateSettingsValue()
		{
			parentModSetting.SetValueBool(textField.text, thisSection, thisToggle.isOn);			
		}
	}
}
