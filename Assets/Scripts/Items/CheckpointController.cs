public class CheckpointController : ItemController {
    
    public override void OnConsumedBy(PrincessCakeController controller) {
        controller.SetCheckpoint(transform.position);

        _collider.enabled = false;
    }

    public override void OnResetEvent() {
        
    }

    public override void OnDisableEvent() {

    }
}
