using UnityEngine;

[RequireComponent(typeof(GameData))]
public class Game : SingletonMonobehavior<Game> {


    private GameData _gameData;
    
    [SerializeField]
    private PrincessCakeController _princessCake;

    [SerializeField]
    private Logger.Level _logLevel = Logger.Level.Error;

    [SerializeField]
    private AudioClip _theme;

    public Logger Logger { get; private set; }
    public GameData _GameData { get { return _gameData; } }
    public PrincessCakeController PrincessCake { get { return _princessCake; } }

    private AudioSource _audio;

    protected void Awake() {
        Logger = new Logger("Game", _logLevel);

        _gameData = GetComponent<GameData>();

        if (_princessCake == null) {
            _princessCake = transform.GetOrAddComponent<PrincessCakeController>();

            Logger.Error("PrincessCakeController reference not found. Drag and Drop it and restart the game.");
        }

        _audio = this.GetOrAddComponent<AudioSource>();
    }

    protected void Start() {
        _audio.TryPlayTheme(_theme);
    }

    public Logger LoggerFactory(string context) {
        return new Logger(context, _logLevel);
    }

}
