using System;
using System.Collections;
using UnityEngine;

public class TerrainButtonModel : IModel {

    [Serializable]
    public class Settings {
        public int WeightRequiredToPress = 5;
        public int PressDurationInSeconds = 5;
    }

    public enum State {
        Depressed,
        PressedHopedOn,
        PressedHopedOff
    }

    private Logger _logger;
    private Settings _settings;
    private State _state;
    private float _pressDecayStartedAt;

	private Coroutine _timerCoroutine;

    public string Name { get; private set; }

    public TerrainButtonModel(string name, Settings settings, PrincessCakeModel.Settings princessCakeSettings) {
        Name = name;
        _settings = settings;
        _state = State.Depressed;

        _logger = Game.Instance.LoggerFactory(Name + "::TerrainButtonModel");

        _logger.Assert(_settings.WeightRequiredToPress <= princessCakeSettings.MaxWeight, "WeightRequiredToPress should be less than or equal to pricessCake.MaxWeight");
        _logger.Assert(_settings.WeightRequiredToPress >= princessCakeSettings.MinWeight, "WeightRequiredToPress should be greater than or equal to pricessCake.MinWeight");

        _logger.Info("State: " + _state, "Initialized.");
    }

    public int WeightRequiredToPress { get { return _settings.WeightRequiredToPress; } }

    public int PressDurationInSeconds { get { return _settings.PressDurationInSeconds; } }

    public bool IsPressed { get { return _state != State.Depressed; } }

    /// <returns>whether state changed to PressedHopedOn</returns>
    public bool HopedOn(IWeightableModel model) {
        if (_state != State.PressedHopedOn) {
            if (CanBePressedBy(model)) {
                _state = State.PressedHopedOn;
                _logger.Info("State: " + _state, " state updated");
                return true;
            }
        }

        return false;
    }

    /// <returns>whether state changed to PressedHopedOff</returns>
    public bool HopedOff(IWeightableModel model, float timeInSeconds) {
        if (_state == State.PressedHopedOn) {
            _state = State.PressedHopedOff;
            _pressDecayStartedAt = timeInSeconds;
            _logger.Info("State: " + _state, " state updated at time: " + timeInSeconds);

			this.StartCountdown();

			return true;
        }
        return false;
    }

    /// <returns>whether state changed to Depressed</returns>
    public bool Depressed(float timeInSeconds) {
        if (_state == State.PressedHopedOff) {
            if (PressEffectIsDecayed(timeInSeconds)) {
                _state = State.Depressed;
                _logger.Info("State: " + _state, " state updated at time: " + timeInSeconds);
                return true;
            }
        }

        return false;
    }

    public bool CanBePressedBy(IWeightableModel model) {
        return model.Weight >= _settings.WeightRequiredToPress;
    }

    public float PressEffectDecaysInSeconds(float timeInSeconds) {
        if (!IsPressed) {
            return 0;
        }

        return _settings.PressDurationInSeconds - (timeInSeconds - _pressDecayStartedAt);
    }

    protected bool PressEffectIsDecayed(float timeInSeconds) {
        return PressEffectDecaysInSeconds(timeInSeconds) <= 0;
    }

	private void StartCountdown()
	{
		if (this._timerCoroutine != null)
		{
			UserInterfaceController.Instance_._GameplayDisplay.StopCoroutine(this._timerCoroutine);
		}

		this._timerCoroutine = UserInterfaceController.Instance_._GameplayDisplay.StartCoroutine(this.UpdateCountdownGrpahics());
	}

	private void StopCountdown()
	{
		UserInterfaceController.Instance_._GameplayDisplay.DeactivateTimer();

		if (this._timerCoroutine != null)
		{
			UserInterfaceController.Instance_._GameplayDisplay.StopCoroutine(this._timerCoroutine);
		}
	}

	private IEnumerator UpdateCountdownGrpahics()
	{
		UserInterfaceController.Instance_._GameplayDisplay.ActivateTimer();

		while (!PressEffectIsDecayed(Time.time))
		{
			UserInterfaceController.Instance_._GameplayDisplay.DisplayTimer(this.PressEffectDecaysInSeconds(Time.time));

			yield return new WaitForSecondsRealtime(0.1f);
		}

		UserInterfaceController.Instance_._GameplayDisplay.DeactivateTimer();
	}
}
