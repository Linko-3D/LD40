using UnityEngine;

public class BoxController : MonoBehaviour {

    public BoxModel.Settings Settings;
    
    public BoxModel Model { get; private set; }

    protected virtual void Start() {
        Model = new BoxModel(Settings, Game.Instance.PrincessCake.Settings);
    }
}
