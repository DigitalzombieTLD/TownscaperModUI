using MelonLoader;
using System;
using System.Collections.Generic;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System.IO;

namespace ModUI
{
	public class ModSettings : MonoBehaviour
	{
		public MelonMod parentMod;
		public GameObject subPanel;
		public RectTransform subRect;

		public GameObject groupContainer;

		public GameObject backButtonGameObject;
		public Button backButton;
		public FileIniDataParser thisIniParser;
		public string settingsFile;
		public IniData iniData;

		public Dictionary<String, GameObject> smallButtons = new Dictionary<String, GameObject>();
		public Dictionary<String, GameObject> bigButtons = new Dictionary<String, GameObject>();

		public GameObject playerPlacementSFXprefab;

		public Dictionary<String, GameObject> controlSliders = new Dictionary<String, GameObject>();
		public Dictionary<String, GameObject> controlColorSliders = new Dictionary<String, GameObject>();
		public Dictionary<String, GameObject> controlInputFields = new Dictionary<String, GameObject>();
		public Dictionary<String, GameObject> controlToggle = new Dictionary<String, GameObject>();
		public Dictionary<String, GameObject> controlKeybind = new Dictionary<String, GameObject>();
		public Dictionary<String, GameObject> networkStatus = new Dictionary<String, GameObject>();
		public Dictionary<uint, GameObject> playerButtons = new Dictionary<uint, GameObject>();

		public Dictionary<String, GameObject> settingGroups = new Dictionary<String, GameObject>();

		bool isOpen = false;

		public ModSettings(MelonMod thisMod)
		{
			parentMod = thisMod;			
			Setup();
		}

		public void Setup()
		{
			subPanel = UnityEngine.Object.Instantiate(UIManager.uiPrefabs["SubPanel"]);
			subRect = subPanel.GetComponent<RectTransform>();
			subPanel.name = parentMod.Info.Name;

			MelonLogger.Msg("[" + parentMod.Info.Name + "] Initializing settings UI ...");

			subPanel.transform.parent = UIManager.viewport.transform;
			subRect.position = UIManager.sidePanelPositionClosed;

			groupContainer = subPanel.transform.Find("GroupContainer").gameObject;

			settingGroups.Add("BACKBUTTONGROUP", UnityEngine.Object.Instantiate(UIManager.uiPrefabs["SettingsGroup"]));
			settingGroups.TryGetValue("BACKBUTTONGROUP", out GameObject backButtonGroup);					
			backButtonGroup.transform.parent = groupContainer.transform;

			backButtonGameObject = UnityEngine.Object.Instantiate(UIManager.uiPrefabs["ButtonBack"]);
			backButtonGameObject.transform.parent = backButtonGroup.transform;
			backButton = backButtonGameObject.GetComponent<Button>();
			backButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { this.Toggle(); })));

			
			subPanel.SetActive(false);

			OpenOrCreateSettingsFile();
		}
		
		public GameObject CheckForAndAddSection(string section)
		{
			//settingGroups.TryGetValue(section, out GameObject settingsGroup);
			
			if (settingGroups.ContainsKey(section))
			{
				return settingGroups[section];
			}
			else
			{
				settingGroups.Add(section, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["SettingsGroup"]));
				settingGroups.TryGetValue(section, out GameObject newGroup);

				GameObject newTitleObject = UnityEngine.Object.Instantiate(UIManager.uiPrefabs["GroupTitle"]);
				newTitleObject.transform.parent = newGroup.transform;
				newGroup.transform.parent = groupContainer.transform;

				TextMeshProUGUI titleText = newTitleObject.GetComponentInChildren<TextMeshProUGUI>();
				titleText.text = section;

				return newGroup;
			}
		}

		public void AddButton(string name, string section, Color32 buttonColor, Action newAction)
		{			
			smallButtons.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["ButtonSmall"]));
			smallButtons[name].transform.parent = CheckForAndAddSection(section).transform;

			ButtonSmall newButton = smallButtons[name].AddComponent<ButtonSmall>();
			newButton.Setup(name, section, buttonColor, newAction);
		}

		public void AddPlayerButton(uint playerID, string name, string section, Color32 buttonColor, Action newAction)
		{	
			if (!playerButtons.ContainsKey(playerID))
			{
				playerButtons.Add(playerID, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["ButtonSmall"]));
				playerButtons[playerID].transform.parent = CheckForAndAddSection(section).transform;

				PlayerButton newButton = playerButtons[playerID].AddComponent<PlayerButton>();
				newButton.Setup(playerID, name, section, buttonColor, newAction);
			}
		}

		public void RemovePlayerButton(uint playerID)
		{
			playerButtons.TryGetValue(playerID, out GameObject buttonObject);

			if (buttonObject != null)
			{
				UnityEngine.Object.Destroy(buttonObject);
				playerButtons.Remove(playerID);
			}			
		}

		public void ClearPlayerButton()
		{			
			foreach(KeyValuePair<uint, GameObject> singleButton in playerButtons)
			{
				UnityEngine.Object.Destroy(singleButton.Value);
			}
			playerButtons.Clear();
		}

		public NetworkStatus AddNetworkStatus(string name, string section, Color32 buttonColor)
		{
			networkStatus.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["NetworkStatus"]));
			networkStatus[name].transform.parent = CheckForAndAddSection(section).transform;

			NetworkStatus newStatus = networkStatus[name].AddComponent<NetworkStatus>();
			
			newStatus.Setup(name, section, buttonColor);

			return newStatus;
		}

		public void RemoveButton(string name, string section)
		{
			smallButtons.TryGetValue(name, out GameObject buttonObject);

			if (buttonObject!=null)
			{
				UnityEngine.Object.Destroy(buttonObject);
			}			
		}

		public void AddButtonBig(string name, string section, Color32 buttonColor, Action newAction)
		{
			bigButtons.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["ButtonBig"]));
			bigButtons[name].transform.parent = CheckForAndAddSection(section).gameObject.transform;

			ButtonBig newButton = bigButtons[name].AddComponent<ButtonBig>();
			newButton.Setup(name, section, buttonColor, newAction);
		}

		public void RemoveButtonBig(string name, string section)
		{
			bigButtons.TryGetValue(name, out GameObject buttonObject);

			if (buttonObject != null)
			{
				UnityEngine.Object.Destroy(buttonObject);
			}
		}
		
		public void AddSlider(string name, string section, Color32 sliderColor, float minValue, float maxValue, bool wholeNumbers, float defaultValue, Action<float> newAction)
		{
			controlSliders.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["Slider"]));
			controlSliders[name].transform.parent = CheckForAndAddSection(section).gameObject.transform;

			DZSlider newSlider = controlSliders[name].AddComponent<DZSlider>();
			newSlider.Setup(name, section, sliderColor, minValue, maxValue, wholeNumbers, newAction, this);

			bool valueExists = GetValueFloat(name, section, out float valueResult);

			if (valueExists)
			{
				newSlider.thisSlider.value = valueResult;
			}
			else
			{
				newSlider.thisSlider.value = defaultValue;
				SetValueFloat(name, section, defaultValue);
			}

			newSlider.awoken = true;
		}

		public void AddColorSlider(string name, string section, Color sliderColor, float minValue, float maxValue, Action<float> newAction)
		{
			controlColorSliders.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["ColorSlider"]));
			controlColorSliders[name].transform.parent = CheckForAndAddSection(section).gameObject.transform;
			
			DZColorSlider newColorSlider = controlColorSliders[name].AddComponent<DZColorSlider>();

			bool valueExists = GetValueColor(name, out Color valueResult);

			if(valueExists)
			{
				sliderColor = valueResult;
			}
			
			newColorSlider.Setup(name, section, sliderColor, minValue, maxValue, newAction, this);
			
			if (valueExists)
			{
				newColorSlider.thisSliderR.value = valueResult.r;
				newColorSlider.thisSliderG.value = valueResult.g;
				newColorSlider.thisSliderB.value = valueResult.b;								
			}
			else
			{
				newColorSlider.thisSliderR.value = 0.5f;
				newColorSlider.thisSliderG.value = 0.5f;
				newColorSlider.thisSliderB.value = 0.5f;
								
				SetValueColor(name, new Color(0.5f,0.5f,0.5f,0.7f));
			}
			newColorSlider.UpdateSettingsValue();
			newColorSlider.awoken = true;
		}

		public void AddInputField(string name, string section, Color32 fieldColor, TMP_InputField.ContentType contentType, string defaultValue, Action<string> newAction)
		{
			controlInputFields.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["Input"]));
			controlInputFields[name].transform.parent = CheckForAndAddSection(section).gameObject.transform;

			InputField newInputField = controlInputFields[name].AddComponent<InputField>();
			newInputField.Setup(name, section, fieldColor, contentType, newAction, this);

			bool valueExists = GetValueString(name, section, out string valueResult);

			if (valueExists)
			{
				newInputField.thisInputField.text = valueResult;
			}
			else
			{
				newInputField.thisInputField.text = defaultValue;
				SetValueString(name, section, defaultValue);
			}
		}

		public void AddToggle(string name, string section, Color32 toggleColor, bool defaultValue, Action<bool> newAction)
		{
			controlToggle.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["Toggle"]));
			controlToggle[name].transform.parent = CheckForAndAddSection(section).gameObject.transform;

			DZToggle newToggleField = controlToggle[name].AddComponent<DZToggle>();
			newToggleField.Setup(name, section, toggleColor, newAction, this);

			bool valueExists = GetValueBool(name, section, out bool valueResult);

			if (valueExists)
			{
				newToggleField.thisToggle.isOn = valueResult;
			}
			else
			{
				newToggleField.thisToggle.isOn = defaultValue;
				SetValueBool(name, section, defaultValue);
			}
		}

		public void AddKeybind(string name, string section, KeyCode defaultValue, Color32 keybindColor)
		{
			controlKeybind.Add(name, UnityEngine.Object.Instantiate(UIManager.uiPrefabs["ButtonKeybind"]));
			controlKeybind[name].transform.parent = CheckForAndAddSection(section).gameObject.transform;

			Keybind newKeybind = controlKeybind[name].AddComponent<Keybind>();
			newKeybind.Setup(name, section, keybindColor, this);

			bool valueExists = GetValueKeyCode(name, section, out KeyCode valueResult);

			if (valueExists)
			{
				newKeybind.thisKeyCode = valueResult;
				newKeybind.contentField.text = valueResult.ToString();
			}
			else
			{
				newKeybind.thisKeyCode = defaultValue;
				newKeybind.contentField.text = defaultValue.ToString();
				SetValueKeyCode(name, section, defaultValue);
			}
		}

		public void OpenOrCreateSettingsFile()
		{		
			thisIniParser = new FileIniDataParser();
			settingsFile = FileSystem.settingsPath + "/" + parentMod.Info.Name + ".ini";

			thisIniParser.Parser.Configuration.AllowCreateSectionsOnFly = true;
			//thisIniParser.Parser.Configuration.AssigmentSpacer = "&&";
			thisIniParser.Parser.Configuration.SkipInvalidLines = true;
			thisIniParser.Parser.Configuration.OverrideDuplicateKeys = true;
			thisIniParser.Parser.Configuration.AllowDuplicateKeys = true;

			if (!File.Exists(settingsFile))
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Creating new settings file ...");
				MelonLogger.Msg("[" + settingsFile + "]");

				iniData = new IniData();
				thisIniParser.WriteFile(settingsFile, iniData);
			}
			else
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Loading settings file ...");
				MelonLogger.Msg("[" + settingsFile + "]");

				iniData = thisIniParser.ReadFile(settingsFile);				
			}
		}

		public bool GetValueString(string name, string section, out string result)
		{
			string tempResult;
			iniData.TryGetKey(section + "|" + name, out tempResult);

			if (tempResult == "")
			{
				result = "";
				MelonLogger.Msg("[" + parentMod.Info.Name + "] String value [" + name + "] not found in section ["+ section +"]");

				return false;
			}
			else
			{
				result = tempResult;
				return true;
			}
		}

		public bool GetValueFloat(string name, string section, out float result)
		{
			string tempResult;
			iniData.TryGetKey(section + "|" + name, out tempResult);

			if (tempResult == "")
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Float value [" + name + "] not found in section [" + section + "]");
				result = 0f;
				return false;
			}
			else
			{
				result = float.Parse(tempResult);
				return true;
			}			
		}

		public bool GetValueColor32(string name, out Color32 result)
		{
			string section = name;

			iniData.TryGetKey(section + "|" + name + "_Red", out string tempResultR);
			iniData.TryGetKey(section + "|" + name + "_Green", out string tempResultG);
			iniData.TryGetKey(section + "|" + name + "_Blue", out string tempResultB);
			iniData.TryGetKey(section + "|" + name + "_Alpha", out string tempResultA);

			if (tempResultR == "" || tempResultG == "" || tempResultB == "" || tempResultA == "")
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Color value [" + name + "] not found or incomplete");
				result = new Color32(0, 0, 0, 1);
				return false;
			}
			else
			{
				result = new Color32(byte.Parse(tempResultR), byte.Parse(tempResultG), byte.Parse(tempResultB), byte.Parse(tempResultA));
				return true;
			}
		}

		public bool GetValueColor(string name, out Color result)
		{
			string section = name;

			iniData.TryGetKey(section + "|" + name + "_Red", out string tempResultR);
			iniData.TryGetKey(section + "|" + name + "_Green", out string tempResultG);
			iniData.TryGetKey(section + "|" + name + "_Blue", out string tempResultB);
			iniData.TryGetKey(section + "|" + name + "_Alpha", out string tempResultA);

			if (tempResultR == "" || tempResultG == "" || tempResultB == "" || tempResultA == "")
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Color value [" + name + "] not found or incomplete");
				result = new Color(0, 0, 0, 1);
				return false;
			}
			else
			{
				result = new Color(float.Parse(tempResultR), float.Parse(tempResultG), float.Parse(tempResultB), float.Parse(tempResultA));
				return true;
			}
		}

		public bool GetValueBool(string name, string section, out bool result)
		{
			string tempResult;
			iniData.TryGetKey(section + "|" + name, out tempResult);

			if (tempResult == "")
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Bool value [" + name + "] not found in section [" + section + "]");
				result = false;
				return false;
			}
			else
			{
				result = bool.Parse(tempResult);
				return true;
			}
		}

		public bool GetValueKeyCode(string name, string section, out KeyCode result)
		{
			string tempResult;
			iniData.TryGetKey(section + "|" + name, out tempResult);
				
			if (tempResult == "")
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] KeyCode value [" + name + "] not found in section [" + section + "]");
				result = KeyCode.None; 
				return false;
			}
			else
			{
				result = ConvertToKeyCode(tempResult);
				return true;
			}
		}

		public KeyCode ConvertToKeyCode(string keyCodeString)
		{
			return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodeString);
		}

		public bool GetValueInt(string name, string section, out int result)
		{
			string tempResult;
			iniData.TryGetKey(section + "|" + name, out tempResult);

			if (tempResult == "")
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Int value [" + name + "] not found in section [" + section + "]");
				result = 0;
				return false;
			}
			else
			{
				float floatResult = float.Parse(tempResult);
				int intResult = int.Parse(tempResult);

				result = intResult;
				return true;
			}
		}

		public void SetValueString(string name, string section, string value)
		{
			CheckIfSectionExist(section);

			if (!iniData[section].ContainsKey(name))
			{
				iniData[section].AddKey(name, value);
			}
			else
			{
				iniData[section][name] = value;
			}
		}

		public void SetValueFloat(string name, string section, float value)
		{
			SetValueString(name, section, value.ToString());
		}

		public void SetValueBool(string name, string section, bool value)
		{
			SetValueString(name, section, value.ToString());
		}

		public void SetValueInt(string name, string section, int value)
		{
			SetValueFloat(name, section, value);
		}

		public void SetValueKeyCode(string name, string section, KeyCode value)
		{
			SetValueString(name, section, value.ToString());
		}

		public void SetValueColor32(string name, Color32 color)
		{
			string section = name;
			int colorElementR = color.r;
			int colorElementG = color.g;
			int colorElementB = color.b;
			int colorElementA = color.a;

			SetValueInt(name + "_Red", section, color.r);
			SetValueInt(name + "_Green", section, color.g);
			SetValueInt(name + "_Blue", section, color.b);
			SetValueInt(name + "_Alpha", section, color.a);
		}

		public void SetValueColor(string name, Color color)
		{
			string section = name;
			float colorElementR = color.r;
			float colorElementG = color.g;
			float colorElementB = color.b;
			float colorElementA = color.a;

			SetValueFloat(name + "_Red", section, color.r);
			SetValueFloat(name + "_Green", section, color.g);
			SetValueFloat(name + "_Blue", section, color.b);
			SetValueFloat(name + "_Alpha", section, color.a);
		}

		public void SaveToFile()
		{
			MelonLogger.Msg("[" + parentMod.Info.Name + "] Writing settings to file ...");
			MelonLogger.Msg("[" + settingsFile + "]");

			thisIniParser.WriteFile(settingsFile, iniData);
		}


		public void CheckIfSectionExist(string section)
		{
			if (!iniData.Sections.ContainsSection(section))
			{
				MelonLogger.Msg("[" + parentMod.Info.Name + "] Section [" + section + "] not found in settings. Creating ...");
				iniData.Sections.AddSection(section);				
			}			
		}		

		public void Toggle()
		{
			if (!UIAnimation.isAnimatingSub)
			{
				if (!isOpen)
				{
					MelonCoroutines.Start(UIAnimation.PanelFadeIn(subRect, subPanel));
					isOpen = true;
					UIManager.titleField.text = parentMod.Info.Name;
					UIManager.scrollViewRect.content = subRect;
				}
				else
				{
					MelonCoroutines.Start(UIAnimation.PanelFadeOut(subRect, subPanel));
					isOpen = false;
					UIManager.titleField.text = "ModUI";
					UIManager.scrollViewRect.content = UIManager.mainRect;
				}
			}
		}
	}
}
