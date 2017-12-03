public class ElevatorController : GroundButtonController {

    protected override void OnHopedOnBy(IWeightableController controller) {
        base.OnHopedOnBy(controller);

        if (Game.Instance.PrincessCake == controller) {
            // TODO:
        }
    }

    protected override void OnHopedOffBy(IWeightableController controller) {
        base.OnHopedOffBy(controller);

        if (Game.Instance.PrincessCake == controller) {
            // TODO:
        }
    }

    protected override void OnDepressed() {
        base.OnDepressed();
    }
}
