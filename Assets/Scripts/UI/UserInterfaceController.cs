using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UserInterfaceController : MonoBehaviour
{
	[Header("Displays")]
	[SerializeField] private GameplayDisplay _gameplayDisplay;
	[SerializeField] private MainMenuDisplay _mainMenuDisplay;

	[Header("Fonts")]
	[SerializeField] private Font _globalFont;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this._mainMenuDisplay.Open();
			Cursor.visible = true;
		}
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{

	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(UserInterfaceController))]
	[CanEditMultipleObjects]
	public class UserInterfaceControllerEditor : Editor
	{
		private SerializedProperty _globalFontProperty;

		private void OnEnable()
		{
			this._globalFontProperty = serializedObject.FindProperty("_globalFont");
		}

		private void ApplyFont(Font font)
		{
			Text[] textFields = FindObjectsOfType<Text>();

			for (int i = 0; i < textFields.Length; i++)
			{
				textFields[i].font = font;
			}
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			if (GUILayout.Button("Apply Font"))
			{
				this.ApplyFont(this._globalFontProperty.objectReferenceValue as Font);

				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}

#pragma warning disable 0219
			UserInterfaceController sUserInterfaceController = target as UserInterfaceController;
#pragma warning restore 0219
		}
	}
#endif
}