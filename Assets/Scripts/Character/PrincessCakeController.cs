using UnityEngine;

public class PrincessCakeController : MonoBehaviour, IWeightableController {

    public PrincessCakeModel.Settings Settings;

    protected PrincessCakeModel _model;

    public IWeightableModel Model { get { return _model; } }

    protected virtual void Start() {
        _model = new PrincessCakeModel(Settings);
    }

}
