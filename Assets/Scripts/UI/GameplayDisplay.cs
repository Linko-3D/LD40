using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameplayDisplay : Display, IController {

    [SerializeField] private TextDisplay _weightText;
    [SerializeField] private TextDisplay _cakesText;
    [SerializeField] private TextDisplay _teasText;
    [SerializeField] private Image _timerBackground;
    [SerializeField] private TextDisplay _timerText;

    public string Name { get { return "Gameplay Display"; } }

    private PrincessCakeModel _model;
    private string _weightTextFormat;
    private string _cakesTextFormat;
    private string _teaTextFormat;
    
    protected override void Start() {
        base.Start();

        Game.Instance.RegisterOnResetEvent(this);

        Game.Instance.Locale.OnLanguageUpdated += UpdateTexts;

        _model = Game.Instance.PrincessCake.Model;
        _model.OnConsumeCake += UpdateWeightDisplay;
        _model.OnConsumeCake += UpdateCakesDisplay;
        _model.OnConsumeTea += UpdateWeightDisplay;
        _model.OnConsumeTea += UpdateTeaDisplay;

        DeactivateTimer();
        UpdateTexts();

        Close();
    }

    protected void UpdateWeightDisplay() {
        _weightText.SetFormat(_weightTextFormat, _model.Weight);
    }

    protected void UpdateCakesDisplay() {
        _cakesText.SetFormat(_cakesTextFormat, _model.Weight);
    }

    protected void UpdateTeaDisplay() {
        _teasText.SetFormat(_teaTextFormat, _model.Weight);
    }

    protected void UpdateTexts() {
        _weightTextFormat = Game.Instance.Locale.Text.StatWeight;
        _cakesTextFormat = Game.Instance.Locale.Text.StatCakes;
        _teaTextFormat = Game.Instance.Locale.Text.StatTeas;

        UpdateWeightDisplay();
        UpdateCakesDisplay();
        UpdateTeaDisplay();
    }

    public void ActivateTimer() {
        _timerBackground.enabled = true;
    }

    public void DisplayTimer(float time) {
        _timerText.Set(time.ToString("0.0"));
    }

    public void DeactivateTimer() {
        _timerText.Set("");
        _timerBackground.enabled = false;
    }

    public void OnResetEvent() {
        DeactivateTimer();

        UpdateWeightDisplay();
        UpdateCakesDisplay();
        UpdateTeaDisplay();
    }

    public void OnDisableEvent() {

    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
    }
#endif
}

namespace New.UTILITY {
#if UNITY_EDITOR
    [CustomEditor(typeof(GameplayDisplay))]
    [CanEditMultipleObjects]
    public class GameplayDisplayEditor : Editor {
        private void OnEnable() {

        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

#pragma warning disable 0219
            GameplayDisplay sGameplayDisplay = target as GameplayDisplay;
#pragma warning restore 0219
        }
    }
#endif
}
