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
	public class SelectionButton : MonoBehaviour
	{
		public Button leftArrow;
		public Button rightArrow;
		public Image buttonImage;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public Action leftAction;
		public Action rightAction;
		public string thisSection;
		public string selectValue;
		public string thisLabel;
		public ModSettings parentModSetting;

		public SelectionButton(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, Action newLeftAction, Action newRightAction, string defaultValue, ModSettings thisModSetting)
		{
			leftAction = newLeftAction;
			rightAction = newRightAction;
			thisSection = section;
			thisLabel = label;
			selectValue = defaultValue;
			parentModSetting = thisModSetting;
			ManualAwake();

			UpdateSettingsValue();
			buttonImage.color = color;
		}

		public void ManualAwake()
		{
			leftArrow = this.gameObject.transform.FindChild("Background").FindChild("Left").GetComponent<Button>();
			rightArrow = this.gameObject.transform.FindChild("Background").FindChild("Right").GetComponent<Button>();
			textField = GetComponentInChildren<TextMeshProUGUI>();
			buttonImage = GetComponentInChildren<Image>();
			thisRect = transform.FindChild("Background").GetComponent<RectTransform>();

			leftArrow.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(leftAction));
			rightArrow.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(rightAction));

			leftArrow.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { UpdateSettingsValue(); })));
			rightArrow.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { UpdateSettingsValue(); })));
		}

        public void UpdateSettingsValue()
		{
            textField.text = selectValue;
			parentModSetting.SetValueString(thisLabel, thisSection, selectValue);
		}
    }
}
