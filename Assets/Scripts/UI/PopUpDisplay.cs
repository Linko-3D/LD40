using System;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopUpDisplay : Display {

    private class DisplayData {
        public string Text;
        public Action OnHide;
    }

    private bool _welcomeDisplayed = false;
    private bool _firstCakeConsumedDisplayed = false;
    private bool _reachedMaxWeightDisplayed = false;
    private bool _firstTeaConsumedDisplayed = false;
    private bool _hopedOnButtonNotEnoughWeightDisplayed = false;
    private bool _hopedOnButtonEnoughWeightDisplayed = false;

    [SerializeField] private float _OpenMinIntervalInSeconds = 4f;

    [SerializeField] private TextDisplay _popUpText;

    private List<DisplayData> _displayQueue = new List<DisplayData>();
    private float _lastTimeOpened = 0;
    private Action _lastOnHide = null;

    public override void Open() {
        base.Open();

        _lastTimeOpened = Time.time;
    }

    public override void Close() {
        base.Close();

        if (_lastOnHide != null) {
            _lastOnHide();
        }
    }

    #region Tips
    public void TryDisplayWelcome() {
        if (!_welcomeDisplayed) {

            Display(Game.Instance.Locale.Text.TipWelcome, () => {
                Display(Game.Instance.Locale.Text.TipUseCheckpointReset);
            });

            _welcomeDisplayed = true;
        }
    }

    public void TryDisplayCakeConsumedTip() {
        if (!_firstCakeConsumedDisplayed) {
            Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipFirstCakeConsumedNomNom, () => {
                Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipFirstCakeConsumedMaximizeWeight);
            });

            _firstCakeConsumedDisplayed = true;
        }
    }

    public void TryDisplayReachedMaxWeightTip() {
        if (!_reachedMaxWeightDisplayed) {

            Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipReachedMaxWeightPressButtons, () => {
                Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipReachedMaxWeightPickupItem);
            });

            _reachedMaxWeightDisplayed = true;
        }
    }

    public void TryDisplayTeaConsumedTip() {
        if (!_firstTeaConsumedDisplayed) {
            Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipFirstTeaConsumedResetWeight, () => {

                Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipFirstTeaConsumeFitCorridors, () => {

                    Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipFirstTeaConsumeJumpHigh);
                });

            });

            _firstTeaConsumedDisplayed = true;
        }
    }

    public void TryDisplayHopedOnButtonEnoughWeightTip(bool isPrincessCake) {
        if (!_hopedOnButtonEnoughWeightDisplayed) {

            Action onHide = null;
            if (isPrincessCake) {
                onHide = () => {
                    Display(Game.Instance.Locale.Text.TipHopedOnButtonEnoughWeightNoItem);
                };
            }

            Display(Game.Instance.Locale.Text.TipHopedOnButtonEnoughWeight, onHide);

            _hopedOnButtonEnoughWeightDisplayed = true;
        }
    }

    public void TryDisplayHopedOnButtonNotEnoughWeightTip(bool isPrincessCake) {
        if (!_hopedOnButtonEnoughWeightDisplayed) {

            if (isPrincessCake) {
                Game.Instance.UI.Popup.Display(Game.Instance.Locale.Text.TipHopedOnButtonNotEnoughWeight);
            }

            _hopedOnButtonEnoughWeightDisplayed = true;
        }
    }
    #endregion

    protected void Display(string message, Action onHide = null) {
        _displayQueue.Add(new DisplayData {
            Text = message,
            OnHide = onHide
        });
    }

    public void OnOkClick() {
        Close();

        Cursor.visible = false;
    }

    private void OnDisplay() {
        DisplayData data = _displayQueue[0];

        _popUpText.Set(data.Text);

        if (!Game.Instance.UI.MainMenu.IsOpen) {
            _displayQueue.RemoveAt(0);

            _lastOnHide = data.OnHide;

            Open();

            Cursor.visible = true;
        }
    }

    protected void Update() {
        if (IsOpen) {
            if (Time.time - _lastTimeOpened > _OpenMinIntervalInSeconds) {
                OnOkClick();
            }
        } else if (_displayQueue.Count != 0) {
            OnDisplay();
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
    }
#endif
}

namespace New.UTILITY {
#if UNITY_EDITOR
    [CustomEditor(typeof(PopUpDisplay))]
    [CanEditMultipleObjects]
    public class PopUpDisplayEditor : Editor {
        private void OnEnable() {

        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

#pragma warning disable 0219
            PopUpDisplay sPopUpDisplay = target as PopUpDisplay;
#pragma warning restore 0219
        }
    }
#endif
}
