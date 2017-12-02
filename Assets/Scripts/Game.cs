using UnityEngine;

public class Game : SingletonMonobehavior<Game> {

    [SerializeField]
    public PrincessCakeModel.Settings PrincessCakeSettings;

    public Logger Logger { get; private set; }

    public PrincessCakeModel PrincessCake { get; private set; }
    
    protected void Awake() {
        Logger = new Logger("Game");
        PrincessCake = new PrincessCakeModel(PrincessCakeSettings);
    }

    public Logger LoggerFactory(string context) {
        return new Logger(context);
    }
}
