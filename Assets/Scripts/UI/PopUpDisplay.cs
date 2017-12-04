using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopUpDisplay : Display
{
	[SerializeField] private Text _popUpTextField;

	private void Awake()
	{
		this.Display("Welcome, eat them ALL!!! AHAHAHAHA!");
	}

	public void Display(string message)
	{
		this.gameObject.SetActive(true);
		this._popUpTextField.text = message;

		Cursor.visible = true;

		this.Open();
	}

	public void OnOkClick()
	{
		Cursor.visible = false;

		this.Close();
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
	[CustomEditor(typeof(PopUpDisplay))]
	[CanEditMultipleObjects]
	public class PopUpDisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			PopUpDisplay sPopUpDisplay = target as PopUpDisplay;
#pragma warning restore 0219
		}
	}
#endif
}