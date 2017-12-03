using UnityEngine;

public class ItemController : MonoBehaviour {

    private Logger _logger;

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
        gameObject.SetActive(false);
    }
}
