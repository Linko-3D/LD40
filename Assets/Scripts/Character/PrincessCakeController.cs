using UnityEngine;

public class PrincessCakeController : MonoBehaviour, IWeightableController {

    public PrincessCakeModel.Settings Settings = new PrincessCakeModel.Settings();
    
    public PrincessCakeModel Model { get; private set; }

    public string Name { get { return name; } }

    IWeightableModel IWeightableController.Model() {
        return Model;
    }

    protected virtual void Start() {
        Model = new PrincessCakeModel(name, Settings);
    }

}
