using UnityEngine;

public class CheckpointController : ItemController {

    [SerializeField]
    private Cloth _flagCloth;

    [SerializeField]
    private Vector3 _externalAccelerationOnConsume = Vector3.zero;
    [SerializeField]
    private Vector3 _randomAccelerationOnConsume = Vector3.zero;

    [SerializeField]
    private Vector3 _externalAccelerationOnStart = new Vector3(-50f, 0f, 0f);
    [SerializeField]
    private Vector3 _randomAccelerationOnStart = new Vector3(10, 0, 10);


    protected override void Start() {
        base.Start();

        if (_flagCloth == null) {
            _flagCloth = GetComponentInChildren<Cloth>();
        }

        _logger.Assert(
            _flagCloth != null, 
            "flag cloth is neither set to script nor found on children."
          + " Drag and drop it to work."
        );

        _flagCloth.externalAcceleration = _externalAccelerationOnStart;
        _flagCloth.randomAcceleration = _randomAccelerationOnStart;
    }

    public override void OnConsumedBy(PrincessCakeController controller) {
        controller.SetCheckpoint(transform.position);

        _collider.enabled = false;

        if (_flagCloth != null) {
            _flagCloth.externalAcceleration = _externalAccelerationOnConsume;
            _flagCloth.randomAcceleration = _randomAccelerationOnConsume;
        }
    }

    public override void OnResetEvent() {
        
    }

    public override void OnDisableEvent() {

    }
}
