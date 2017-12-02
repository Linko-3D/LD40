using System;

public class GroundButtonModel {

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

    public GroundButtonModel(Settings settings, PrincessCakeModel.Settings princessCakeSettings) {
        _logger = Game.Instance.LoggerFactory("Box");
        _settings = settings;
        _state = State.Depressed;

        _logger.Assert(_settings.WeightRequiredToPress <= princessCakeSettings.MaxWeight, "WeightRequiredToPress should be less than or equal to pricessCake.MaxWeight");
        _logger.Assert(_settings.WeightRequiredToPress >= princessCakeSettings.MinWeight, "WeightRequiredToPress should be greater than or equal to pricessCake.MinWeight");

        _logger.Info("Starting at state: " + _state);
    }

    public int WeightRequiredToPress { get { return _settings.WeightRequiredToPress; } }

    public int PressDurationInSeconds { get { return _settings.PressDurationInSeconds; } }

    public bool CanBePressedBy(IWeightable model) {
        return model.Weight >= _settings.WeightRequiredToPress;
    }

    /// <returns>whether state changed to PressedHopedOn</returns>
    public bool HopedOn(IWeightable model) {
        if (CanBePressedBy(model)) {
            _state = State.PressedHopedOn;
            return true;
        }
        return false;
    }

    /// <returns>whether state changed to PressedHopedOff</returns>
    public bool HopedOff(IWeightable model, float timeInSeconds) {
        if (_state == State.PressedHopedOn) {
            _state = State.PressedHopedOff;
            _pressDecayStartedAt = timeInSeconds;
            return true;
        }
        return false;
    }

    /// <returns>whether state changed to Depressed</returns>
    public void Depress() {
        _state = State.Depressed;
    }

    public float PressEffectDecaysInSeconds(float timeInSeconds) {
        if (_state != State.Depressed) {
            return 0;
        }

        return timeInSeconds - _pressDecayStartedAt;
    }

    public bool PressEffectHasDecayed(float timeInSeconds) {
        return PressEffectDecaysInSeconds(timeInSeconds) <= 0;
    }
}
