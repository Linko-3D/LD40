using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UserInterfaceController : MonoBehaviour
{
	[SerializeField] private MainMenuDisplay _mainMenuDisplay;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this._mainMenuDisplay.Open();
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
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			UserInterfaceController sUserInterfaceController = target as UserInterfaceController;
#pragma warning restore 0219
		}
	}
#endif
}