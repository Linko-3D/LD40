using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class Game : SingletonMonobehavior<Game> {

    private GameData _gameData;
    
    [SerializeField]
    private PrincessCakeController _princessCake;

    [SerializeField]
    private Logger.Level _logLevel = Logger.Level.Error;
    
    public Logger Logger { get; private set; }
    public GameData _GameData { get { return _gameData; } }

    public PrincessCakeController PrincessCake { get { return _princessCake; } }
    
    private HashSet<IController> _disabledControllers = new HashSet<IController>();
    private HashSet<IController> _resetOnCheckpoint = new HashSet<IController>();

    protected void Awake() {
        Logger = new Logger("Game", _logLevel);

        _gameData = GetComponent<GameData>();

        if (_princessCake == null) {
            _princessCake = transform.GetOrAddComponent<PrincessCakeController>();

            Logger.Error("PrincessCakeController reference not found. Drag and Drop it and restart the game.");
        }

        _princessCake.OnResetToCheckpoint += ResetDisabled;
        _princessCake.OnCheckpoitAcquired += _disabledControllers.Clear;
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

    private void ResetDisabled() {
        foreach(IController ctrl in _disabledControllers) {
            ctrl.OnResetEvent();
        }
        _disabledControllers.Clear();

        foreach(IController ctrl in _resetOnCheckpoint) {
            ctrl.OnResetEvent();
        }
    }
}
