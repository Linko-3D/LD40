using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuDisplay : Display
{
	[SerializeField] private Text _playButtonTextField;
	
	public void OnPlayClick()
	{
		this._playButtonTextField.text = "Resume";
		this.Close();

		Cursor.visible = false;
	}

	public void OnExitClick()
	{
		Application.Quit();
	}

#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(MainMenuDisplay))]
	[CanEditMultipleObjects]
	public class MainMenuDisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			MainMenuDisplay sMainMenuDisplay = target as MainMenuDisplay;
#pragma warning restore 0219
		}
	}
#endif
}