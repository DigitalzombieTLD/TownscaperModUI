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
	public class InputField : MonoBehaviour
	{
		public TMP_InputField thisInputField;
		public Image inputfieldImage;
		public TextMeshProUGUI textField;
		public RectTransform thisRect;
		public Action<string> thisAction;
		public TMP_InputField.ContentType thisContentType;
		public ModSettings parentModSetting;
		public string thisSection;

		public InputField(IntPtr intPtr) : base(intPtr) { }

		[HideFromIl2Cpp]
		public void Setup(string label, string section, Color32 color, TMP_InputField.ContentType contentType, Action<string> newAction, ModSettings thisModSetting)
		{
			thisAction = newAction;
			thisContentType = contentType;
			parentModSetting = thisModSetting;
			thisSection = section;

			ManualAwake();
			inputfieldImage.color = color;
			textField.text = label;			
		}

		[method: HideFromIl2Cpp]
		public void ManualAwake()
		{
			thisInputField = GetComponent<TMP_InputField>();
			textField = GetComponentInChildren<TextMeshProUGUI>();
			inputfieldImage = thisInputField.image;
			thisRect = GetComponent<RectTransform>();
			thisInputField.contentType = thisContentType;

		    Quaternion randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
			thisRect.rotation = randomRotation;

			thisInputField.onEndEdit.AddListener(DelegateSupport.ConvertDelegate<UnityAction<string>>(thisAction));
			thisInputField.onEndEdit.AddListener(DelegateSupport.ConvertDelegate<UnityAction<string>>(new Action<string>(delegate (string value) { UpdateSettingsValue(); })));
		}

		[HideFromIl2Cpp]
		public void UpdateSettingsValue()
		{
			parentModSetting.SetValueString(textField.text, thisSection, thisInputField.text);
		}
	}
}
