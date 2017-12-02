using System;

public class DoorModel: IModel {

    public enum State {
        Opened,
        Closed
    }

    [Serializable]
    public class Settings {
        public State StartState = State.Opened;
        public int AutoCloseAfterSeconds = 5;
    }

    private Logger _logger;
    private Settings _settings;
    private State _state;
    private float _openedAt;

    public bool IsOpen { get { return _state == State.Opened; } }

    public DoorModel(Settings settings, PrincessCakeModel.Settings princessCakeSettings) {
        _logger = Game.Instance.LoggerFactory("Door");
        _settings = settings;

        _state = _settings.StartState;

        _logger.Info("Starting at state: " + _state);
    }

    public void Open(float timeInSeconds) {
        _state = State.Opened;
        _openedAt = timeInSeconds;
    }

    public void Close() {
        _state = State.Closed;
    }
    
    public float AutoClosesInSeconds(float timeInSeconds) {
        if (_state != State.Opened) {
            return 0;
        }

        return _settings.AutoCloseAfterSeconds - (timeInSeconds - _openedAt);
    }

    public bool ShouldAutoClose(float timeInSeconds) {
        return AutoClosesInSeconds(timeInSeconds) <= 0;
    }
}
