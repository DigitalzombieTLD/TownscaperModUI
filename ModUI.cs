using DigitalRuby.Tween;
using MelonLoader;
using System;
using System.IO;
using UnhollowerRuntimeLib;
using UnityEngine;
using System.Reflection;

namespace ModUI
{
	public class ModUIMain : MelonMod
	{
		public static Il2CppAssetBundle moduiBundle;

		public override void OnInitializeMelon()
		{
			FileSystem.Setup();
			DZInput.GetAllKeycodes();

			LoadEmbeddedAssetBundle();
        }

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if (!UIManager.isInitialized)
			{
				TweenFactory.SceneManagerSceneLoaded();
				UIManager.InitializeManager();
			}
		}
        public static void LoadEmbeddedAssetBundle()
        {
            MemoryStream memoryStream;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ModUI.Resources.modui");
            memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            moduiBundle = Il2CppAssetBundleManager.LoadFromMemory(memoryStream.ToArray());			
        }
    }


}
