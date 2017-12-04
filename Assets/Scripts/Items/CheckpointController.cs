using UnityEngine;

public class CheckpointController : ItemController {

    [SerializeField]
    private bool _triggerOnce = true;

    public override void OnConsumedBy(PrincessCakeController controller) {
        controller.SetCheckpoint(transform.position);

        _collider.enabled = false;
    }

    public override void OnResetEvent() {
        
    }

    public override void OnDisableEvent() {

    }
}
