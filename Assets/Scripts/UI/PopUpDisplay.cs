using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopUpDisplay : Display
{
    [SerializeField] private string _welcomeMessage = "Welcome, eat them ALL!!! AHAHAHAHA!";
    [SerializeField] private string _useCheckpointReset = "Hit R to reset to the last checkpoint !";
    [SerializeField] private float _autoCloseTimer = 4f;

    [SerializeField] private Text _popUpTextField;

    private Queue<Action> onHideQueue = new Queue<Action>();
    protected bool _welcomeDisplayed;
    

    public void TryWelcomeDisplay() {
        if (!_welcomeDisplayed) {

            this.Display(_welcomeMessage, () => {
                this.Display(_useCheckpointReset);
            });

            _welcomeDisplayed = true;
        }
    }

	public void Display(string message, Action onHide = null)
	{
		this._popUpTextField.text = message;

		Cursor.visible = true;

		this.Open();

        if (onHide != null) {
            onHideQueue.Enqueue(onHide);
        }

		this.StartCoroutine (this.CloseTimer ());
	}

	public void OnOkClick()
	{
		Cursor.visible = false;

		this.Close();

        StartCoroutine(TryFireOnHideAfterSeconds());
    }

	private IEnumerator CloseTimer()
	{
		yield return new WaitForSecondsRealtime (_autoCloseTimer);

		this.Close ();

        StartCoroutine(TryFireOnHideAfterSeconds());
    }

    private IEnumerator TryFireOnHideAfterSeconds(float delayInSeconds = 1f) {
        yield return new WaitForSeconds(delayInSeconds);

        if (onHideQueue.Count != 0) {
            onHideQueue.Dequeue()();
        }
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