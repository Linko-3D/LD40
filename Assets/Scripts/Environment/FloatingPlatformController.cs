public class FloatingPlatformController : TweenController {

    protected override void Update() {
        base.Update();

        // continues to tween back and forth.
        TryToggle();
    }
}
