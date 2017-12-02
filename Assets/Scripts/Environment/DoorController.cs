using UnityEngine;

public class DoorController : MonoBehaviour {

    public DoorModel.Settings Settings;

    public DoorModel Model { get; private set; }

    protected virtual void Start() {
        Model = new DoorModel(Settings, Game.Instance.PrincessCake.Settings);
    }
}
