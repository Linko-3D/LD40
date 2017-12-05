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

    private class DisplayData {
        public string Text;
        public Action OnHide;
    }

    [SerializeField] private string _welcomeMessage = "Welcome, eat them ALL!!! AHAHAHAHA!";
    [SerializeField] private string _useCheckpointReset = "Hit R to reset to the last checkpoint !";
    [SerializeField] private float _OpenMinIntervalInSeconds = 3f;

    [SerializeField] private Text _popUpTextField;

    private List<DisplayData> _displayQueue = new List<DisplayData>();
    private float _lastTimeOpened = 0;
    private Action _lastOnHide = null;

    private bool _welcomeDisplayed;

    public override void Open() {
        base.Open();

        _lastTimeOpened = Time.time;
    }

    public override void Close() {
        base.Close();

        if (this._lastOnHide != null) {
            this._lastOnHide();
        }
    }

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
        _displayQueue.Add(new DisplayData {
            Text = message,
            OnHide = onHide
        });
    }
    
	public void OnOkClick()
	{
		this.Close();

        Cursor.visible = false;
    }

    private void OnDisplay(DisplayData data) {

        this._popUpTextField.text = data.Text;
        this._lastOnHide = data.OnHide;

        this.Open();

        Cursor.visible = true;
    }

    protected void Update() {
        if (IsOpen) {
            if (Time.time - _lastTimeOpened > _OpenMinIntervalInSeconds) {
                OnOkClick();
            }
        } else if (_displayQueue.Count != 0) {
            DisplayData data = _displayQueue[0];

            _displayQueue.RemoveAt(0);

            OnDisplay(data);
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