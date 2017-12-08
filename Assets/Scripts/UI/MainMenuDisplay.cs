using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuDisplay : Display {
    [SerializeField] private Display _controlsDisplay;

    [SerializeField] private TextDisplay _playButtonText;
    private bool _playButtonUpdated = false;

    protected override void Start() {
        base.Start();

        Game.Instance.Logger.Assert(
            _controlsDisplay != null, "MainMenuDisplay",
            "controlsDisplay not found. Drag and drop it to game object."
        );

        _playButtonText.Set(Game.Instance.Locale.Text.MenuStart);

        _controlsDisplay.Close();
    }

    public override void Open() {
        base.Open();

        Game.Instance.Pause();
        Cursor.visible = true;

        Game.Instance.UI.GamePlay.Close();
        Game.Instance.UI.Popup.Close();
    }

    public override void Close() {
        if (_controlsDisplay.IsOpen) {
            _controlsDisplay.Close();
        } else {
            base.Close();

            Game.Instance.UI.GamePlay.Open();
            Game.Instance.UI.Popup.Open();

            Game.Instance.Resume();
            Cursor.visible = false;
        }
    }

    public void OnPlayClick() {
        if (!_playButtonUpdated) {
            _playButtonText.Set(Game.Instance.Locale.Text.MenuResume);
            _playButtonUpdated = true;
        }

        Close();

        UI.Instance.Popup.TryDisplayWelcome();
    }

    public void OnLanguageClick() {
        Game.Instance.Locale.SetLanguage(Game.Instance.Locale.Language.NextEnumValue());
    }

    public void OnExitClick() {
        Application.Quit();
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
    }
#endif
}

namespace New.UTILITY {
#if UNITY_EDITOR
    [CustomEditor(typeof(MainMenuDisplay))]
    [CanEditMultipleObjects]
    public class MainMenuDisplayEditor : Editor {
        private void OnEnable() {

        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

#pragma warning disable 0219
            MainMenuDisplay sMainMenuDisplay = target as MainMenuDisplay;
#pragma warning restore 0219
        }
    }
#endif
}
