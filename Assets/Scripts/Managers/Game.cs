using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(GameData))]
public class Game : SingletonMonobehaviour<Game> {

    [SerializeField]
    private PrincessCakeController _princessCake;

    [SerializeField]
    private Logger.Level _logLevel = Logger.Level.Error;
    
    public Logger Logger { get; private set; }
    public GameData Data { get { return GameData.Instance; } }
    public Localization Locale { get { return Localization.Instance; } }
    public UI UI { get { return UI.Instance; } }
    public PrincessCakeController PrincessCake { get { return _princessCake; } }
    
    private HashSet<IController> _disabledControllers = new HashSet<IController>();
    private HashSet<IController> _resetOnCheckpoint = new HashSet<IController>();

    private int _isPausedMutex = 0;

    protected void Awake() {
        Logger = new Logger("Game", _logLevel);

        Data.Initialize();
        Locale.Initialize();

        if (_princessCake == null) {
            _princessCake = transform.GetOrAddComponent<PrincessCakeController>();

            Logger.Error("PrincessCakeController reference not found." +
                " Drag and Drop it in the Game gameobject for the game to work.");
        }

        _princessCake.OnResetToCheckpoint += ResetAllDisabled;
        _princessCake.OnCheckpointAcquired += _disabledControllers.Clear;
    }

    protected void Start() {
        Logger.Assert(UI != null, "UI not found, make sure to added to the scene.");

        UI.Initialize();
    }

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UI.MainMenu.Toggle();
        }
    }

    public bool IsPaused {
        get {
            return _isPausedMutex > 0;
        }
    }

    public void Pause() {
        ++_isPausedMutex;

        if (!IsPaused) {
            Time.timeScale = 0;
        }
    }

    public void Resume() {
        --_isPausedMutex;

        if (_isPausedMutex < 0) {

            Logger.Warn("Pause Error", "Resumed more times than paused. isPausedMutex: " + _isPausedMutex);

            _isPausedMutex = 0;
        }

        if (!IsPaused) {
            Time.timeScale = 1;
        }
    }

    public Logger LoggerFactory(string context) {
        return new Logger(context, _logLevel);
    }

    public void RegisterOnResetEvent(IController controller) {
        _resetOnCheckpoint.Add(controller);
    }

    public void Disable(IController controller) {
        _disabledControllers.Add(controller);

        controller.OnDisableEvent();
    }

    public void Reset(IController controller) {
        _disabledControllers.Remove(controller);

        controller.OnResetEvent();
    }

    private void ResetAllDisabled() {
        foreach(IController ctrl in _disabledControllers) {
            ctrl.OnResetEvent();
        }
        _disabledControllers.Clear();

        foreach(IController ctrl in _resetOnCheckpoint) {
            ctrl.OnResetEvent();
        }
    }
}
