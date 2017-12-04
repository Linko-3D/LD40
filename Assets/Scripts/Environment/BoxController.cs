using UnityEngine;

//[RequireComponent(typeof(BoxCollider))] // 
public class BoxController : MonoBehaviour, IWeightableController {

    public BoxModel.Settings Settings;

    public BoxModel Model { get; private set; }

    IWeightableModel IWeightableController.Model() { return Model; }

    public string Name { get { return name; } }

    private Logger _logger;

    protected virtual void Start() {
        Model = new BoxModel(Name, Settings, Game.Instance.PrincessCake.Settings);

        _logger = Game.Instance.LoggerFactory(name + "::BoxController");

        Collider collider = GetComponent<Collider>();
        if (collider == null) {
            collider = GetComponentInChildren<Collider>();
        }

        _logger.Assert(collider != null, "No collider was found at object. Make sure to assign one (.eg BoxCollider).");
    }

    public void OnResetEvent() {
        gameObject.SetActive(true);
    }

    public void OnDisableEvent() {
        gameObject.SetActive(false);
    }
}
