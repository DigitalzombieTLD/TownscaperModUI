using MelonLoader;
using System.Linq;
using UnityEngine;

namespace ModUI
{
	public static class DZInput
	{
		public static int[] keyValues;
		public static bool[] keyCount;

		public static KeyCode[] keyCodes = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => k < KeyCode.JoystickButton19).ToArray();

		public static void GetAllKeycodes()
		{
			keyValues = (int[])System.Enum.GetValues(typeof(KeyCode));
			keyCount = new bool[keyValues.Length];			
		}
	}
}
