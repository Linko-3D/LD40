public class CheckpointController : ItemController {

    public override void OnConsumedBy(PrincessCakeController controller) {
        controller.SetCheckpoint(transform.position);
    }
}
