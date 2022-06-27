using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ModUI
{
	public static class UIManager
	{
		public static bool isInitialized;
		public static bool isOpen;
		public static GameObject currentUI;

		public static Dictionary<string, GameObject> uiPrefabs = new Dictionary<string, GameObject>();
		public static Dictionary<MelonMod, GameObject> registeredMods = new Dictionary<MelonMod, GameObject>();
		public static Dictionary<MelonMod, ModSettings> activeModSettings = new Dictionary<MelonMod, ModSettings>();
		
		public static RectTransform menuRect;
		public static GameObject menuGameObject;

		public static GameObject mainPanel;
		public static RectTransform mainRect;
		public static GameObject viewport;

		public static ScrollRect scrollViewRect;

		public static GameObject activePanel;		

		public static Button buttonClose;
		public static Button buttonOpen;
	
		public static TextMeshProUGUI titleField;

		public static Vector3 mainPanelPositionOpen = new Vector3(0, 0, 0);
		public static Vector3 mainPanelPositionClosed = new Vector3(-180, 0, 0);

		public static Vector2 sidePanelPositionOpen = new Vector2(5, 0);
		public static Vector2 sidePanelPositionClosed = new Vector2(-180, 0);

		public static void InitializeManager()
		{			
			LoadPrefabs();
			InitializeBaseUI();
			isInitialized = true;
		}

		public static ModSettings Register(MelonMod thisMod, Color32 buttonColor)
		{			
			registeredMods.Add(thisMod, UnityEngine.Object.Instantiate(uiPrefabs["ButtonBig"]));
			registeredMods[thisMod].transform.parent = mainPanel.transform;
			ButtonBig newButton = registeredMods[thisMod].AddComponent<ButtonBig>();
				//newButton.Setup(thisMod.Info.Name, thisMod.Info.Author, buttonColor);
			newButton.Setup(thisMod.Info.Name, buttonColor, new Action(delegate { activeModSettings[thisMod].Toggle(); }));
			activeModSettings.Add(thisMod, new ModSettings(thisMod));
			
				//newButton.thisButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { activeModSettings[thisMod].Toggle(); })));

			MelonLogger.Msg("[" + thisMod.Info.Name + "] Settings initialized!");

			return activeModSettings[thisMod];
		}

		static void LoadPrefabs()
		{
			uiPrefabs.Add("ButtonBig", ModUIMain.moduiBundle.LoadAsset<GameObject>("ButtonBig"));
			uiPrefabs.Add("ButtonKeybind", ModUIMain.moduiBundle.LoadAsset<GameObject>("ButtonKeybind"));
			uiPrefabs.Add("ButtonSmall", ModUIMain.moduiBundle.LoadAsset<GameObject>("ButtonSmall"));
			uiPrefabs.Add("Input", ModUIMain.moduiBundle.LoadAsset<GameObject>("Input"));
			uiPrefabs.Add("Slider", ModUIMain.moduiBundle.LoadAsset<GameObject>("Slider"));
			uiPrefabs.Add("Toggle", ModUIMain.moduiBundle.LoadAsset<GameObject>("Toggle"));
			uiPrefabs.Add("SubPanel", ModUIMain.moduiBundle.LoadAsset<GameObject>("SubPanel"));
			uiPrefabs.Add("ButtonBack", ModUIMain.moduiBundle.LoadAsset<GameObject>("ButtonBack"));
			uiPrefabs.Add("NetworkStatus", ModUIMain.moduiBundle.LoadAsset<GameObject>("NetworkStatus"));
			uiPrefabs.Add("ColorSlider", ModUIMain.moduiBundle.LoadAsset<GameObject>("ColorSlider"));
			uiPrefabs.Add("SettingsGroup", ModUIMain.moduiBundle.LoadAsset<GameObject>("SettingsGroup"));
			uiPrefabs.Add("GroupTitle", ModUIMain.moduiBundle.LoadAsset<GameObject>("GroupTitle"));			
			uiPrefabs.Add("SelectionButton", ModUIMain.moduiBundle.LoadAsset<GameObject>("SelectionButton"));
		}

		static void InitializeBaseUI()
		{
			currentUI = UnityEngine.Object.Instantiate(ModUIMain.moduiBundle.LoadAsset<GameObject>("ModUI"));
			UnityEngine.Object.DontDestroyOnLoad(currentUI);
			//SceneManager.MoveGameObjectToScene(currentUI, SceneManager.GetSceneByName("FlatscreenUi"));						

			menuGameObject = currentUI.transform.Find("Sidepanel").gameObject;
			menuRect = menuGameObject.GetComponent<RectTransform>();

			titleField = menuGameObject.transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>();

			scrollViewRect = menuGameObject.transform.Find("ScrollView").gameObject.GetComponent<ScrollRect>();

			mainPanel = menuGameObject.transform.Find("ScrollView/Viewport/MainPanel").gameObject;
			mainRect = mainPanel.GetComponent<RectTransform>();
			
			viewport = menuGameObject.transform.Find("ScrollView/Viewport").gameObject;

			buttonClose = menuGameObject.transform.Find("ButtonClose").gameObject.GetComponent<Button>();
			buttonClose.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { ToggleUI(); })));
			//buttonClose.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { FileSystem.SaveAllSettingsToFile(); })));

			buttonOpen = menuGameObject.transform.Find("ButtonOpen").gameObject.GetComponent<Button>();
			buttonOpen.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { ToggleUI(); })));

			buttonClose.gameObject.SetActive(false);
			SetPinned(false);
			isOpen = false;

			UIManager.titleField.text = "ModUI";

			MelonLogger.Msg("ModUI initialized");			
		}

		public static void ToggleUI()
		{
			if (!UIAnimation.isAnimatingMain)
			{
				if (!isOpen)
				{
					MelonCoroutines.Start(UIAnimation.MainFadeIn());
				}
				else
				{
					FileSystem.SaveAllSettingsToFile();					
					MelonCoroutines.Start(UIAnimation.MainFadeOut());
				}
			}
		}

		public static void SetPinned(bool pinned)
		{
			if (pinned)
			{
				ColorBlock colors = buttonOpen.colors;
				colors.normalColor = new Color(buttonOpen.image.color.r, buttonOpen.image.color.g, buttonOpen.image.color.b, 1);
				buttonOpen.colors = colors;
			}
			else
			{
				ColorBlock colors = buttonOpen.colors;
				colors.normalColor = new Color(buttonOpen.image.color.r, buttonOpen.image.color.g, buttonOpen.image.color.b, 0);
				buttonOpen.colors = colors;
			}
		}
	}
}
