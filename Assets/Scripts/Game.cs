using UnityEngine;

public class Game : SingletonMonobehavior<Game> {

    [SerializeField]
    private PrincessCakeModel.Settings _princessSettings;

    public Logger Logger { get; private set; }

    public PrincessCakeModel PrincessCake { get; private set; }
    
    protected void Awake() {
        Logger = new Logger("Game");
        PrincessCake = new PrincessCakeModel(_princessSettings);
    }

    public Logger LoggerFactory(string context) {
        return new Logger(context);
    }
}
