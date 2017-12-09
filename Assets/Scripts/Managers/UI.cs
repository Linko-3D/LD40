using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UI : SingletonMonobehaviour<UI>
{
	[Header("Displays")]
	[SerializeField] private GameplayDisplay _gameplayDisplay;
	[SerializeField] private MainMenuDisplay _mainMenuDisplay;
	[SerializeField] private PopUpDisplay _popUpDisplay;

	[Header("Fonts")]
	[SerializeField] private Font _globalFont;

    public GameplayDisplay GamePlay { get { return _gameplayDisplay; } }
    public MainMenuDisplay MainMenu { get { return _mainMenuDisplay; } }
    public PopUpDisplay Popup { get { return _popUpDisplay; } }

    protected UI() : base() { }

    public void Initialize() {
        GamePlay.Initialize();
        Popup.Initialize();
        MainMenu.Initialize();

        GamePlay.Open();
        MainMenu.Open();
    }
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(UI))]
	[CanEditMultipleObjects]
	public class UserInterfaceControllerEditor : Editor
	{
		private SerializedProperty _globalFontProperty;

		private void OnEnable()
		{
			_globalFontProperty = serializedObject.FindProperty("_globalFont");
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
				ApplyFont(_globalFontProperty.objectReferenceValue as Font);

				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}

            if (GUILayout.Button("Ensure TextDisplay")) {
                Text[] textFields = FindObjectsOfType<Text>();

                for (int i = 0; i < textFields.Length; i++) {
                    textFields[i].transform.GetOrAddComponent<TextDisplay>();
                }
            }

#pragma warning disable 0219
			UI sUserInterfaceController = target as UI;
#pragma warning restore 0219
		}
	}
#endif
}