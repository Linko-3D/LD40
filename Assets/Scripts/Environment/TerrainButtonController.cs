using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerrainButtonController : MonoBehaviour, IController {

    public List<TweenController> TweensOnAtPress = new List<TweenController>();
    public List<TweenController> TweensOffAtDepress = new List<TweenController>();
    public TerrainButtonModel.Settings Settings;
    
    public TerrainButtonModel Model { get; private set; }
    public string Name { get { return name; } }

    private Logger _logger;

    protected virtual void Start () {
        _logger = Game.Instance.LoggerFactory(name + "::TerrainButtonController");

        Model = new TerrainButtonModel(name, Settings, Game.Instance.PrincessCake.Settings);
    }

    protected virtual void OnTriggerEnter(Collider collider) {
        IWeightableController controller = collider.gameObject.GetComponent<IWeightableController>();

        _logger.Info("OnTriggerEnter", collider.gameObject.name + " entered");

        if (controller != null) {
            HopedOn(controller);
        }
    }

    protected virtual void OnTriggerExit(Collider collider) {
        IWeightableController controller = collider.gameObject.GetComponent<IWeightableController>();

        _logger.Info("OnTriggerExit", collider.gameObject.name + " left");

        if (controller != null) {
            HopedOff(controller);
        }
    }

    protected virtual void Update() {
        TryDepress();

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.E)) {
            HopedOn(Game.Instance.PrincessCake);
        }

        if (Input.GetKeyUp(KeyCode.L)) {
            HopedOff(Game.Instance.PrincessCake);
        }
#endif
    }

    // To be overriden by elevator.
    protected virtual void OnHopedOnBy(IWeightableController controller) {
        _logger.Info("OnHopedOnBy", controller.Name + " hoped on");

        foreach (TweenController tweensToOn in TweensOnAtPress) {
            tweensToOn.TryTweenToOn(true);
        }
    }

    protected virtual void OnHopedOffBy(IWeightableController controller) {
        _logger.Info("OnHopedOnBy", controller.Name + " hoped off");
    }

    protected virtual void OnDepressed() {
        _logger.Info("OnDepressed");

        foreach (TweenController tweensToOff in TweensOffAtDepress) {
            tweensToOff.TryTweenToOff(true);
        }
    }

    private void HopedOn(IWeightableController controller) {
        if (Model.HopedOn(controller.Model())) {
            OnHopedOnBy(controller);
        }
    }

    private void HopedOff(IWeightableController controller) {
        if (Model.HopedOff(controller.Model(), Time.time)) {
            OnHopedOffBy(controller);
        }
    }

    private void TryDepress() {
        if (Model.Depressed(Time.time)) {
            OnDepressed();
        }
    }
}
