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
    public class DZSlider : MonoBehaviour
	{
		public Slider thisSlider;
		public Image handleImage;
		public Image backgroundImage;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public Action<float> thisAction;
		public ModSettings parentModSetting;
		public string thisSection;
		public bool awoken;
		public string title;

		public DZSlider(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, float minValue, float maxValue, bool wholeNumbers, Action<float> newAction, ModSettings thisModSetting)
		{
			thisAction = newAction;
			parentModSetting = thisModSetting;
			thisSection = section;

			ManualAwake();

			textField.text = label;
			title = label;

			handleImage.color = color;
			thisSlider.minValue = minValue;
			thisSlider.maxValue = maxValue;
			thisSlider.wholeNumbers = wholeNumbers;

			backgroundImage = this.gameObject.transform.Find("BackgroundBorder").GetComponent<Image>();
			backgroundImage.color = color;
		}

		[method: HideFromIl2Cpp]
		public void ManualAwake()
		{
			thisSlider = GetComponent<Slider>();
			textField = GetComponentInChildren<TextMeshProUGUI>();
			handleImage = thisSlider.image;
			thisRect = GetComponent<RectTransform>();

			thisSlider.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(thisAction));
			thisSlider.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(new Action<float>(delegate (float value) { UpdateSettingsValue(); })));						
		}

		[HideFromIl2Cpp]
		public void UpdateSettingsValue()
		{
			if(awoken)
			{				
				parentModSetting.SetValueFloat(title, thisSection, thisSlider.value);
			}						
		}
	}
}
