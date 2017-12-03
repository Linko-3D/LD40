#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrincessCakeController : MonoBehaviour, IWeightableController {

    public PrincessCakeModel.Settings Settings = new PrincessCakeModel.Settings();

    public PrincessCakeModel Model { get; private set; }

    public string Name { get { return name; } }

    IWeightableModel IWeightableController.Model() {
        return Model;
    }

    protected void Start() {
        Model = new PrincessCakeModel(name, Settings);
    }

#if UNITY_EDITOR
    protected void OnDrawGizmosSelected() {
        if (Model == null) return;

        Handles.Label(transform.position, Name);
        Handles.Label(transform.position + Vector3.down, "Weight: " + Model.Weight);
        Handles.Label(transform.position + Vector3.down * 2, "CakesEaten: " + Model.CakesEaten);
        Handles.Label(transform.position + Vector3.down * 3, "TeasDrunk: " + Model.TeasDrunk);
    }
#endif
}
