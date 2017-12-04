#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TweenController : MonoBehaviour, IController {

    public enum State {
        Off,
        On,
        MovingToOn,
        MovingToOff
    }

    public enum TweenType {
        Lerp,
        SmoothDump
    }
    
    [SerializeField]
    private TweenType _tweenType = TweenType.Lerp;
    [SerializeField]
    private Transform _targetOn;
    [SerializeField]
    private Transform _targetOff;
    [SerializeField]
    private float _movementSpeed = 1f;
    [SerializeField]
    private bool _isFloatingPlatform = false;
    [SerializeField]
    private bool _startAutoFloat = true;
    [SerializeField]
    private bool _isRotationTween = false;
    [SerializeField]
    private AudioClip _onTweenOn;
    [SerializeField]
    private AudioClip _onTweenOff;

    private Logger _logger;
    private State _state;
    private bool _isAutoFloating;
    private Vector3 _velocity;
    private AudioSource _audio;

    public string Name { get { return name; } }

    public bool IsOn { get { return _state == State.On; } }

    public bool IsOff { get { return _state == State.Off; } }

    public bool IsMoving {
        get {
            return _state == State.MovingToOn
                || _state == State.MovingToOff;
        }
    }

    protected virtual void Start() {
        _logger = Game.Instance.LoggerFactory("TweenController::" + name);
        
        _logger.Assert(_targetOn != null, "TargetOn not found. Drag and drop a game object.");
        _logger.Assert(_targetOff != null, "TargetOff not found. Drag and drop a game object.");

        if (!_isRotationTween) {
            // reparent transform & target transforms under the same transform.
            // Leaving the target transforms as childs of this transform will
            // cause the tween to move forever as the target transforms will move along
            // with the tweened transform.
            if (_targetOn.IsChildOf(transform) || _targetOff.IsChildOf(transform)) {
                GameObject tweenGroupParent = new GameObject(Name + "TweenGroup");
                tweenGroupParent.transform.parent = transform.parent;

                transform.parent = tweenGroupParent.transform;
                _targetOff.parent = tweenGroupParent.transform;
                _targetOn.parent = tweenGroupParent.transform;
            }

            _logger.Assert(
                _targetOn.parent != transform,
                "TargetOn Transform should not be a child of a TweenController. "
                + "Group up TweenController, targetOn & TargetOff gameobjects "
                + "under the same parent object."
            );

            _logger.Assert(
                _targetOff.parent != transform,
                "TargetOff Transform should not be a child of a TweenController. "
                + "Group up TweenController, targetOn & TargetOff gameobjects "
                + "under the same parent object."
            );

        } else {
            // rotation is applied at the local axis.
            // the targets should be set as children
            // of the controller.
            _targetOff.transform.parent = transform;
            _targetOn.transform.parent = transform;

            _targetOff.transform.position = transform.position;
            _targetOn.transform.position = transform.position;
            
            _startAutoFloat = false;
            _isFloatingPlatform = false;
        }

        _logger.Info("State " + _state.ToString(), " Tween Initialized.");
        
        _audio = this.GetOrAddComponent<AudioSource>();

        OnReset();

        Game.Instance.RegisterOnResetEvent(this);
    }

    protected virtual void Update() {
        switch (_state) {
            case State.MovingToOff:
                if (MoveStepToTarget(_targetOff)) {
                    _state = State.Off;
                    if (_isRotationTween) {
                        transform.localRotation = _targetOff.localRotation;
                    }
                    _logger.Info("State " + _state.ToString(), "Tween Finished.");
                }
                break;
            case State.MovingToOn:
                if (MoveStepToTarget(_targetOn)) {
                    _state = State.On;
                    if (_isRotationTween) {
                        transform.localRotation = _targetOn.localRotation;
                    }
                    _logger.Info("State " + _state.ToString(), "Tween Finished.");
                }
                break;
        }

        if (_isFloatingPlatform && _isAutoFloating) {
            // continues to tween back and forth.
            TryTweenToOn();
            TryTweenToOff();
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;

        if (_targetOn != null && _targetOff != null) {
            Handles.Label(_targetOn.position, name + " On");
            Handles.Label(_targetOff.position, name + " Off");

            Gizmos.DrawLine(_targetOff.position, _targetOn.position);
        }
    }
#endif

    private void OnReset() {
        _state = State.Off;
        _isAutoFloating = false;
        _velocity = Vector3.zero;

        transform.position = _targetOff.position;
        if (_isRotationTween) {
            transform.localRotation = _targetOff.localRotation;
        }

        if (_isFloatingPlatform && _startAutoFloat) {
            TryTweenToOn(true);
        }
    }

    private bool MoveStepToTarget(Transform target) {
        switch(_tweenType) {
            case TweenType.Lerp:
                transform.position = Vector3.Lerp(transform.position, target.position, _movementSpeed);
                break;
            case TweenType.SmoothDump:
                transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, _movementSpeed);
                break;
        }

        return transform.position == target.position;
    }

    public virtual bool TryTweenToOn(bool force = false) {
        bool tweenStarted = false;

        switch (_state) {
            case State.Off:
                _state = State.MovingToOn;
                _logger.Info("State " + _state.ToString(), "Tween Starting.");
                tweenStarted = true;
                break;
            case State.MovingToOff:
                if (force) {
                    _state = State.MovingToOn;
                    _logger.Info("State " + _state.ToString(), "Forced Tween Starting.");
                    tweenStarted = true;
                }
                break;
        }
        
        if (tweenStarted) {
            _audio.TryPlaySFX(_onTweenOn);

            if (_isFloatingPlatform && force) {
                _isAutoFloating = true;
            }
        }

        return tweenStarted;
    }

    public virtual bool TryTweenToOff(bool force = false) {
        bool tweenStarted = false;

        switch (_state) {
            case State.On:
                _state = State.MovingToOff;
                _logger.Info("State " + _state.ToString(), "Tween Starting.");
                tweenStarted = true;
                break;
            case State.MovingToOn:
                if (force) {
                    _state = State.MovingToOff;
                    _logger.Info("State " + _state.ToString(), "Forced Tween Starting.");
                    tweenStarted = true;
                }
                break;
        }

        if (tweenStarted) {
            _audio.TryPlaySFX(_onTweenOff);

            if (_isFloatingPlatform && force) {
                _isAutoFloating = false;
            }
        }

        
        return tweenStarted;
    }

    public void OnResetEvent() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }

        OnReset();
    }

    public void OnDisableEvent() {
        gameObject.SetActive(false);
    }
}
