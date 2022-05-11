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
	public class DZColorSlider : MonoBehaviour
	{
		public Slider thisSliderR;
		public Slider thisSliderG;
		public Slider thisSliderB;

		public Image handleImageR;
		public Image handleImageG;
		public Image handleImageB;

		public Image backgroundImage;

		public float colorValueR;
		public float colorValueG;
		public float colorValueB;

		public Color combinedColor;

		public TextMeshProUGUI textLabel;
		public RectTransform thisRect;
		public Action<float> thisAction;
		public ModSettings parentModSetting;
		public string thisSection;
		public bool awoken;

		public DZColorSlider(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color color, float minValue, float maxValue, Action<float> newAction, ModSettings thisModSetting)
		{
			thisAction = newAction;
			parentModSetting = thisModSetting;
			thisSection = section;
			
			ManualAwake();

			textLabel.text = label;
			handleImageR.color = new Color(color.r, 0f, 0f,1f);
			handleImageG.color = new Color(0f, color.g, 0f, 1f);
			handleImageB.color = new Color(0f, 0f, color.b, 1f);

			//float minValue = 0.2f;
			//float maxValue = 1f;

			thisSliderR.minValue = minValue;
			thisSliderR.value = color.r;
			thisSliderR.maxValue = maxValue;
			thisSliderR.wholeNumbers = false;
				thisSliderG.minValue = minValue;
				thisSliderG.value = color.g;
				thisSliderG.maxValue = maxValue;
				thisSliderG.wholeNumbers = false;
			thisSliderB.minValue = minValue;
			thisSliderB.value = color.b;
			thisSliderB.maxValue = maxValue;
			thisSliderB.wholeNumbers = false;

			backgroundImage = this.gameObject.transform.Find("BackgroundBorder").GetComponent<Image>();
			backgroundImage.color = color;

			combinedColor = color;

		}

		[method: HideFromIl2Cpp]
		public void ManualAwake()
		{
			thisSliderR = transform.FindChild("SubSliderRed").GetComponent<Slider>();
			thisSliderG = transform.FindChild("SubSliderGreen").GetComponent<Slider>();
			thisSliderB = transform.FindChild("SubSliderBlue").GetComponent<Slider>();

			textLabel = transform.FindChild("Label").GetComponent<TextMeshProUGUI>();

			handleImageR = thisSliderR.image;
			handleImageG = thisSliderG.image;
			handleImageB = thisSliderB.image;

			thisRect = GetComponent<RectTransform>();

			thisSliderR.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(new Action<float>(delegate (float value) { UpdateSettingsValue(); })));
			thisSliderR.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(thisAction));
									
			thisSliderG.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(new Action<float>(delegate (float value) { UpdateSettingsValue(); })));
			thisSliderG.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(thisAction));
			
			thisSliderB.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(new Action<float>(delegate (float value) { UpdateSettingsValue(); })));
			thisSliderB.onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(thisAction));
		}

		[HideFromIl2Cpp]
		public void UpdateSettingsValue()
		{
			if(awoken)
			{
				colorValueR = thisSliderR.value;
				colorValueG = thisSliderG.value;
				colorValueB = thisSliderB.value;
							
				handleImageR.color = new Color(colorValueR, 0f, 0f, 1f);
				handleImageG.color = new Color(0f, colorValueG, 0f, 1f);
				handleImageB.color = new Color(0f, 0f, colorValueB, 1f);

				combinedColor = new Color(colorValueR, colorValueG, colorValueB);
				backgroundImage.color = combinedColor;

				parentModSetting.SetValueColor(textLabel.text, combinedColor);
			}						
		}
	}
}
