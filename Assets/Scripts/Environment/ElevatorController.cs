using UnityEngine;

public class ElevatorController : TweenController {

    [SerializeField]
    private AudioClip _elevatorInProgress;

    protected override void OnTweenOffStarted() {
        base.OnTweenOffStarted();
        _audio.TryPlaySFX(_elevatorInProgress, true);
    }

    protected override void OnTweenOnStarted() {
        base.OnTweenOnStarted();
        _audio.TryPlaySFX(_elevatorInProgress, true);
    }

    protected override void OnTweenOnFinished() {
        base.OnTweenOnFinished();
        _audio.Stop();
    }

    protected override void OnTweenOffFinished() {
        base.OnTweenOffFinished();
        _audio.Stop();
    }

}
