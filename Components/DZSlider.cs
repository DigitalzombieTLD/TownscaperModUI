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

		public DZSlider(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, float minValue, float maxValue, bool wholeNumbers, Action<float> newAction, ModSettings thisModSetting)
		{
			thisAction = newAction;
			parentModSetting = thisModSetting;
			thisSection = section;

			ManualAwake();

			textField.text = label;
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

			Quaternion randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
			thisRect.rotation = randomRotation;

			thisSlider.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(thisAction));
			thisSlider.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(new Action<float>(delegate (float value) { UpdateSettingsValue(); })));						
		}

		[HideFromIl2Cpp]
		public void UpdateSettingsValue()
		{
			if(awoken)
			{				
				parentModSetting.SetValueFloat(textField.text, thisSection, thisSlider.value);
			}						
		}
	}
}
