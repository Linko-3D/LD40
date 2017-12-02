using UnityEngine;

public class TweenController : MonoBehaviour {

    public enum State {
        Off,
        On,
        MovingToOn,
        MovingToOff
    }

    [SerializeField]
    private Transform _targetOn;
    [SerializeField]
    private Transform _targetOff;
    [SerializeField]
    private float _movementSpeed = 1f;
    [SerializeField]
    private State _startState = State.Off;

    private Logger _logger;
    private State _state;

    public bool IsOn { get { return _state == State.On; } }

    public bool IsOff { get { return _state == State.Off; } }

    public bool IsMoving {
        get {
            return _state == State.MovingToOn
                || _state == State.MovingToOff;
        }
    }

    public bool TryToggle(bool force = false) {
        switch (_state) {
            case State.Off:
                _state = State.MovingToOn;
                return true;
            case State.On:
                _state = State.MovingToOff;
                return true;
            case State.MovingToOff:
                if (force) {
                    _state = State.MovingToOn;
                    return true;
                }
                break;
            case State.MovingToOn:
                if (force) {
                    _state = State.MovingToOff;
                    return true;
                }
                break;
        }

        return false;
    }

    protected virtual void Start() {
        _logger = Game.Instance.LoggerFactory("TweenController::" + name);
        _state = _startState;

        _logger.Assert(_targetOn != null, "TargetOn not found. Drag and drop a game object.");
        _logger.Assert(_targetOff != null, "TargetOff not found. Drag and drop a game object.");
    }

    protected virtual void Update() {
        switch (_state) {
            case State.MovingToOn:
                if (MoveStepToTarget(_targetOff)) {
                    _state = State.Off;
                }
                break;
            case State.MovingToOff:
                if (MoveStepToTarget(_targetOn)) {
                    _state = State.On;
                }
                break;
        }
    }

    private bool MoveStepToTarget(Transform target) {
        transform.position = transform.position = Vector3.Lerp(transform.position, target.position, _movementSpeed);

        return transform.position == target.position;
    }
}
