using UnityEngine;

public class Game : SingletonMonobehavior<Game> {

    [SerializeField]
    private PrincessCakeModel.Settings _princessSettings;

    public PrincessCakeModel PrincessCake { get; private set; }

    protected void Awake() {
        PrincessCake = new PrincessCakeModel(_princessSettings);
    }
}
