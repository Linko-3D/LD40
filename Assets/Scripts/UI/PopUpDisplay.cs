using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopUpDisplay : Display
{
    [SerializeField] private string _welcomMessage = "Welcome, eat them ALL!!! AHAHAHAHA!";
    [SerializeField] private float _autoCloseTimer = 3f;

    [SerializeField] private Text _popUpTextField;

	private void Start()
	{
		this.Display(_welcomMessage);
	}

	public void Display(string message)
	{
		this._popUpTextField.text = message;

		Cursor.visible = true;

		this.Open();

		this.StartCoroutine (this.CloseTimer ());
	}

	public void OnOkClick()
	{
		Cursor.visible = false;

		this.Close();
	}

	private IEnumerator CloseTimer()
	{
		yield return new WaitForSecondsRealtime (_autoCloseTimer);

		this.Close ();
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