using DigitalRuby.Tween;
using MelonLoader;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace ModUI
{
    public class ModUIMain : MelonMod
    {
		public static Il2CppAssetBundle moduiBundle;
		
		public override void OnApplicationStart()
		{
			FileSystem.Setup();
			DZInput.GetAllKeycodes();

			ClassInjector.RegisterTypeInIl2Cpp<TweenFactory>();
			ClassInjector.RegisterTypeInIl2Cpp<ButtonBig>();
			ClassInjector.RegisterTypeInIl2Cpp<ButtonSmall>();
			ClassInjector.RegisterTypeInIl2Cpp<DZSlider>();
			ClassInjector.RegisterTypeInIl2Cpp<InputField>();
			ClassInjector.RegisterTypeInIl2Cpp<DZToggle>();
			ClassInjector.RegisterTypeInIl2Cpp<Keybind>();

			moduiBundle = Il2CppAssetBundleManager.LoadFromFile("Mods\\modui");			
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{			
			if (!UIManager.isInitialized)
			{
				TweenFactory.SceneManagerSceneLoaded();
				UIManager.InitializeManager();
			}
		}
		
		public override void OnUpdate()
		{
			DZInput.GetInput();			
		}
	}
}
