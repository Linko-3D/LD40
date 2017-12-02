using UnityEngine;

[RequireComponent(typeof(TweenController))]
public class ElevatorController : GroundButtonController {

    [SerializeField]
    private bool _onDepressedMoveToOff;

    public TweenController Tween { get; private set; }

    protected override void Start() {
        base.Start();

        Tween = GetComponent<TweenController>();
    }

    protected override void OnHopedOnBy(IWeightableController controller) {
        base.OnHopedOnBy(controller);

        if (Game.Instance.PrincessCake == controller) {
            Tween.TryToggle();
        }
    }

    protected override void OnDepressed() {
        base.OnDepressed();
        
        if (_onDepressedMoveToOff) {
            Tween.TryToggle();
        }
    }

}
