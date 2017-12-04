using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckpointActivationController : ItemController {

    public override void OnConsumedBy(PrincessCakeController controller) {
        controller.ResetToLastCheckpoint();
    }
}
