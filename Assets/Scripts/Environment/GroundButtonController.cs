using UnityEngine;

public class GroundButtonController : MonoBehaviour {

    public float PositionOffsetYOnPressed = -.06f;
    public GroundButtonModel.Settings Settings;

    protected GroundButtonModel _model;

    protected virtual void Start () {
        _model = new GroundButtonModel(Settings, Game.Instance.PrincessCakeSettings);
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        IWeightableController controller = collision.gameObject.GetComponent<IWeightableController>();

        if (controller != null) {
            HopedOn(controller.Model);
        }
    }

    protected virtual void OnCollisionExit(Collision collision) {
        IWeightableController controller = collision.gameObject.GetComponent<IWeightableController>();

        if (controller != null) {
            HopedOff(controller.Model);
        }
    }

    protected virtual void Update() {
        TryDepress();

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.E)) {
            HopedOn(Game.Instance.PrincessCake);
        }

        if (Input.GetKeyUp(KeyCode.L)) {
            HopedOff(Game.Instance.PrincessCake);
        }
#endif
    }

    // To be overriden by elevator.
    protected virtual void OnHopedOnBy(IWeightableModel model) {
        transform.SetPosY(transform.position.y + PositionOffsetYOnPressed);
    }

    protected virtual void OnHopedOffBy(IWeightableModel model) {

    }

    protected virtual void OnDepressed() {
        transform.SetPosY(transform.position.y - PositionOffsetYOnPressed);
    }


    private void HopedOn(IWeightableModel model) {
        if (_model.HopedOn(model)) {
            OnHopedOnBy(model);
        }
    }

    private void HopedOff(IWeightableModel model) {
        if (_model.HopedOff(model, Time.time)) {
            OnHopedOffBy(model);
        }
    }

    private void TryDepress() {
        if (_model.Depressed(Time.time)) {
            OnDepressed();
        }
    }
}
