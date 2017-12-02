using UnityEngine;

public class Game : SingletonMonobehavior<Game> {

    [SerializeField]
    private PrincessCakeController _princessCake;

    public Logger Logger { get; private set; }
    public PrincessCakeController PrincessCake { get { return _princessCake; } }

    protected void Awake() {
        Logger = new Logger("Game");

        if (_princessCake == null) {
            _princessCake = transform.GetOrAddComponent<PrincessCakeController>();

            Logger.Error("PrincessCakeController reference not found. Drag and Drop it and restart the game.");
        }
    }

    public Logger LoggerFactory(string context) {
        return new Logger(context);
    }
}
