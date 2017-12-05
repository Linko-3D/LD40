using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuDisplay : Display
{
    [SerializeField] private Display _controlsDisplay;

    [SerializeField] private Text _playButtonTextField;

    protected override void Start() {
        base.Start();

        Game.Instance.Logger.Assert(
            _controlsDisplay != null, "MainMenuDisplay",
            "controlsDisplay not found. Drag and drop it to game object."
        );

        this.Open();
    }

    public override void Open()
    {
        base.Open();

        Game.Instance.Pause();
        Cursor.visible = true;
    }

    public override void Close() {
        if (_controlsDisplay.IsOpen) {
            _controlsDisplay.Close();
        } else {
            base.Close();

            Game.Instance.Resume();
            Cursor.visible = false;
        }
    }

    public void OnPlayClick()
	{
		this._playButtonTextField.text = "Resume";
		this.Close();
        
        UserInterfaceController.Instance_._PopUpDisplay.TryWelcomeDisplay();
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