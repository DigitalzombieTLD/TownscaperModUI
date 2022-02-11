using MelonLoader;
using System.Collections;
using UnityEngine;

namespace ModUI
{
	public static class FileSystem
	{
		//public static string settingsPath = @"file:///" + Application.dataPath + @"/../Mods/_ModSettings";
		public static string settingsPath = Application.dataPath + "/../Mods/_ModSettings";
		public static bool savingInProgress;

		public static void Setup()
		{
			if (!System.IO.Directory.Exists(settingsPath))
			{
				System.IO.Directory.CreateDirectory(settingsPath);
			}
		}

		public static void SaveAllSettingsToFile()
		{
			if (!savingInProgress)
			{
				savingInProgress = true;
				MelonCoroutines.Start(StartSavingToFile());
			}
		}

		public static IEnumerator StartSavingToFile()
		{
			foreach (ModSettings singleModSetting in UIManager.activeModSettings.Values)
			{
				singleModSetting.SaveToFile();
			}

			yield return null;
			savingInProgress = false;
		}
	}
}
