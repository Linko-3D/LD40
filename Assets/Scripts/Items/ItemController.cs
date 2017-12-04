using UnityEngine;

public class ItemController : MonoBehaviour, IController {

    private Logger _logger;

    public string Name { get { return name; } }

    protected virtual void Start() {
        _logger = Game.Instance.LoggerFactory(name + "::ItemController");
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
