using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextDisplay : MonoBehaviour {

    [SerializeField] private string _textId;

    protected Text _text;

    protected virtual void Awake () {
        _text = GetComponent<Text>();

        Game.Instance.Locale.OnLanguageUpdated += OnLanguageUpdated;
    }

    protected virtual void OnLanguageUpdated() {
        if (!string.IsNullOrEmpty(_textId)) {
            SetId(_textId);
        }
    }

    public void SetIdFormat(string textId, params object[] args) {
        _textId = textId;

        SetFormat(Game.Instance.Locale.GetTextById(_textId), args);
    }

    public void SetId(string textId) {
        _textId = textId;

        Set(Game.Instance.Locale.GetTextById(_textId));
    }

    public void Set(string text) {
        _text.text = text;
    }

    public void SetFormat(string text, params object[] args) {
        _text.text = string.Format(text, args);
    }
}
