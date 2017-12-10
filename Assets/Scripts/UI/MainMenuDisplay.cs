using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuDisplay : Display {
    [SerializeField] private Display _controlsDisplay;

    [SerializeField] private TextDisplay _playButtonText;

    [SerializeField] private GameObject _exitButtonGameObject;

    private bool _playButtonUpdated = false;
    private Logger _logger;

    public override void Initialize() {
        _logger = Game.Instance.LoggerFactory("MainMenuDisplay");

        _logger.Assert( _controlsDisplay != null, "controlsDisplay not found. Drag and drop it to game object.");
        _logger.Assert(_playButtonText != null, "PlayButtonText not found. Drag and drop it to game object.");
        _logger.Assert(_exitButtonGameObject != null, "exitButtonGameObject not found. Drag and drop it to game object.");
        
        _playButtonText.Set(Game.Instance.Locale.Text.MenuStart);

        _controlsDisplay.Initialize();

#if UNITY_WEBGL || UNITY_EDITOR
        _exitButtonGameObject.SetActive(false);
#endif

        base.Initialize();
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
        Localization.LanguageId lang = Game.Instance.Locale.Language.NextEnumValue();

        // Dynamic font issue in webgl.
        // Bored to look into it, let's just skip chinese in webGL.
#if UNITY_WEBGL
        if (lang == Localization.LanguageId.Chinese) {
            lang = lang.NextEnumValue();
        }
#endif

        Game.Instance.Locale.SetLanguage(lang);
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
