using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopUpDisplay : Display {

    private class DisplayData {
        public string Text;
        public Action OnHide;
    }

    [SerializeField] private string _welcomeText = "Welcome !!! Eat them all Ms Nom !";
    [SerializeField] private string _useCheckpointResetText = "Hit 'R' to reset to the last checkpoint !";
    private bool _welcomeDisplayed;

    [SerializeField] private string _firstCakeConsumedNomNomText = "Nom nom nom, YUMMY !!!";
    [SerializeField] private string _firstCakeConsumedMaximizeWeightText = "Eat more cakes to maximize your weight.";
    private bool _firstCakeConsumedDisplayed = false;

    [SerializeField] private string _reachedMaxWeightPressButtonsText = "You've put on some pounds, You are able to press buttons now !";
    [SerializeField] private string _reachedMaxWeightPickupItemText = "Try to pick up an item with 'E'.";
    private bool _reachedMaxWeightDisplayed = false;

    [SerializeField] private string _firstTeaConsumedResetWeightText = "Wow ! Looking gooood. The tea resets your weight !";
    [SerializeField] private string _firstTeaConsumeFitCorridorsText = "You can fit through narrow corridors now.";
    [SerializeField] private string _firstTeaConsumeJumpHighText = "And jump high again.";
    private bool _firstTeaConsumedDisplayed = false;

    [SerializeField] private string _hopedOnButtonNotEnoughWeightText = "You need to to maximize your weight to press buttons.";
    private bool _hopedOnButtonNotEnoughWeightDisplayed = false;
    
    [SerializeField] private string _hopedOnButtonEnoughWeightText = "Good job. The button was triggered !";
    [SerializeField] private string _hopedOnButtonEnoughWeightNoItemText = "Did you know that you can trigger buttons with items ?";
    private bool _hopedOnButtonEnoughWeightDisplayed = false;

    [SerializeField] private float _OpenMinIntervalInSeconds = 4f;

    [SerializeField] private Text _popUpTextField;

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

            Display(_welcomeText, () => {
                Display(_useCheckpointResetText);
            });

            _welcomeDisplayed = true;
        }
    }

    public void TryDisplayCakeConsumedTip() {
        if (!_firstCakeConsumedDisplayed) {
            Game.Instance.UI.Popup.Display(_firstCakeConsumedNomNomText, () => {
                Game.Instance.UI.Popup.Display(_firstCakeConsumedMaximizeWeightText);
            });

            _firstCakeConsumedDisplayed = true;
        }
    }

    public void TryDisplayReachedMaxWeightTip() {
        if (!_reachedMaxWeightDisplayed) {

            Game.Instance.UI.Popup.Display(_reachedMaxWeightPressButtonsText, () => {
                Game.Instance.UI.Popup.Display(_reachedMaxWeightPickupItemText);
            });

            _reachedMaxWeightDisplayed = true;
        }
    }

    public void TryDisplayTeaConsumedTip() {
        if (!_firstTeaConsumedDisplayed) {
            Game.Instance.UI.Popup.Display(_firstTeaConsumedResetWeightText, () => {

                Game.Instance.UI.Popup.Display(_firstTeaConsumeFitCorridorsText, () => {

                    Game.Instance.UI.Popup.Display(_firstTeaConsumeJumpHighText);
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
                    Display(_hopedOnButtonEnoughWeightNoItemText);
                };
            }

            Display(_hopedOnButtonEnoughWeightText, onHide);

            _hopedOnButtonEnoughWeightDisplayed = true;
        }
    }

    public void TryDisplayHopedOnButtonNotEnoughWeightTip(bool isPrincessCake) {
        if (!_hopedOnButtonEnoughWeightDisplayed) {

            if (isPrincessCake) {
                Game.Instance.UI.Popup.Display(_hopedOnButtonNotEnoughWeightText);
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

    private void OnDisplay(DisplayData data) {

        _popUpTextField.text = data.Text;
        _lastOnHide = data.OnHide;

        Open();

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
