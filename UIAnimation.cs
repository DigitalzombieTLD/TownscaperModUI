using DigitalRuby.Tween;
using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModUI
{
	public class UIAnimation
	{
		public static bool isAnimatingMain;
		public static bool isAnimatingSub;

		public static IEnumerator MainFadeIn()
		{
			isAnimatingMain = true;
			yield return null;
			Vector3 currentPos = UIManager.mainPanelPositionClosed;
			Vector3 startPos = UIManager.mainPanelPositionClosed;
			Vector3 endPos = UIManager.mainPanelPositionOpen;

			System.Action<ITween<Vector3>> updateMovePos = (t) =>
			{
				UIManager.menuRect.anchoredPosition = t.CurrentValue;
			};

			System.Action<ITween<Vector3>> moveCompleted = (t) =>
			{
				UIManager.menuRect.anchoredPosition = endPos;
				UIManager.isOpen = true;
				isAnimatingMain = false;

				UIManager.buttonClose.gameObject.SetActive(true);
				UIManager.buttonOpen.gameObject.SetActive(false);
			};

			UIManager.menuGameObject.Tween("mainfadein", currentPos, endPos, 0.5f, TweenScaleFunctions.SineEaseInOut, updateMovePos, moveCompleted);

			yield return null;
		}

		public static IEnumerator MainFadeOut()
		{
			isAnimatingMain = true;
			Vector3 currentPos = UIManager.mainPanelPositionOpen;
			Vector3 startPos = UIManager.mainPanelPositionOpen;
			Vector3 endPos = UIManager.mainPanelPositionClosed;

			System.Action<ITween<Vector3>> updateMovePos = (t) =>
			{
				UIManager.menuRect.anchoredPosition = t.CurrentValue;
			};

			System.Action<ITween<Vector3>> moveCompleted = (t) =>
			{
				UIManager.menuRect.anchoredPosition = endPos;
				UIManager.isOpen = false;
				isAnimatingMain = false;

				UIManager.buttonClose.gameObject.SetActive(false);
				UIManager.buttonOpen.gameObject.SetActive(true);
			};

			UIManager.menuGameObject.Tween("mainfadeout", currentPos, endPos, 0.5f, TweenScaleFunctions.SineEaseInOut, updateMovePos, moveCompleted);

			yield return null;
		}

		public static IEnumerator PanelFadeIn(RectTransform panelRect, GameObject subPanel)
		{
			isAnimatingSub = true;
			yield return null;


			
			UIManager.mainPanel.SetActive(false);
			
			Vector2 currentPos = UIManager.sidePanelPositionClosed;
			Vector2 startPos = UIManager.sidePanelPositionClosed;
			Vector2 endPos = UIManager.sidePanelPositionOpen;

			System.Action<ITween<Vector2>> updateMovePos = (t) =>
			{
				panelRect.anchoredPosition = t.CurrentValue;
			};

			System.Action<ITween<Vector2>> moveCompleted = (t) =>
			{
				panelRect.anchoredPosition = endPos;
				isAnimatingSub = false;
				subPanel.SetActive(true);
			};

			panelRect.gameObject.Tween("panelfadein", currentPos, endPos, 0.2f, TweenScaleFunctions.SineEaseInOut, updateMovePos, moveCompleted);

			yield return null;
		}

		public static IEnumerator PanelFadeOut(RectTransform panelRect, GameObject subPanel)
		{
			isAnimatingSub = true;
			Vector2 currentPos = UIManager.sidePanelPositionOpen;
			Vector2 startPos = UIManager.sidePanelPositionOpen;
			Vector2 endPos = UIManager.sidePanelPositionClosed;

			System.Action<ITween<Vector2>> updateMovePos = (t) =>
			{
				panelRect.anchoredPosition = t.CurrentValue;
			};

			System.Action<ITween<Vector2>> moveCompleted = (t) =>
			{
				panelRect.anchoredPosition = endPos;
				isAnimatingSub = false;
				subPanel.SetActive(false);
				UIManager.mainPanel.SetActive(true);
			};

			panelRect.gameObject.Tween("panelfadeout", currentPos, endPos, 0.5f, TweenScaleFunctions.SineEaseInOut, updateMovePos, moveCompleted);

			yield return null;
		}
	}
}
