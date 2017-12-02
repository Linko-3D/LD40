using System;

public class GroundButtonModel : IModel {

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

    public bool IsPressed { get { return _state != State.Depressed; } }

    /// <returns>whether state changed to PressedHopedOn</returns>
    public bool HopedOn(IWeightableModel model) {
        if (_state != State.PressedHopedOn) {
            if (CanBePressedBy(model)) {
                _state = State.PressedHopedOn;
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
            return true;
        }
        return false;
    }

    /// <returns>whether state changed to Depressed</returns>
    public bool Depressed(float timeInSeconds) {
        if (_state == State.PressedHopedOff) {
            if (PressEffectIsDecayed(timeInSeconds)) {
                _state = State.Depressed;
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
}
