using UnityEngine;

public class ItemController : MonoBehaviour, IController {
    
    protected Logger _logger;
    protected Collider _collider;

    public string Name { get { return name; } }

    protected virtual void Start() {
        _logger = Game.Instance.LoggerFactory(name + "::ItemController");
        _collider = GetComponent<Collider>();

        _logger.Assert(
            _collider != null,
            "Controller requires a collider(.eg BoxCollider) attached to it."
        );
        //_logger.Assert(
        //    _collider.isTrigger,
        //    "Controller requires a collider(.eg BoxCollider) marked as trigger"
        //  + " attached to it. Make sure you configured the object's collider properly."
        //);
    }

    private void OnTriggerEnter(Collider collider) {
        PrincessCakeController controller = collider.gameObject.GetComponent<PrincessCakeController>();

        _logger.Info("OnTriggerEnter", collider.gameObject.name + " entered");

        if (controller != null) {
            OnConsumedBy(controller);
        }
    }

    public virtual void OnConsumedBy(PrincessCakeController controller) {
        Game.Instance.Disable(this);
    }

    public virtual void OnResetEvent() {
        gameObject.SetActive(true);
    }

    public virtual void OnDisableEvent() {
        gameObject.SetActive(false);
    }
}
