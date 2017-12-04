using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckpointActivationController : ItemController, IController {

    public override void OnConsumedBy(PrincessCakeController controller) {
        controller.OnResetEvent();
    }

    public override void OnResetEvent() {
        gameObject.SetActive(true);
    }

    public override void OnDisableEvent() {
        gameObject.SetActive(false);
    }
}
