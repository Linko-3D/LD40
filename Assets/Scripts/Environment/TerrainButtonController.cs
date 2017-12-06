using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerrainButtonController : MonoBehaviour, IController {
    
    [SerializeField]
    private bool _triggeredByPlayerOnly = false;

    [SerializeField]
    private AudioClip _onPressed;
    [SerializeField]
    private AudioClip _onDepressed;

    public List<TweenController> TweensOnAtPress = new List<TweenController>();
    public List<TweenController> TweensOffAtDepress = new List<TweenController>();
    public List<IController> DisableAtPress = new List<IController>();
    public List<IController> DisableAtDepress = new List<IController>();

    public TerrainButtonModel.Settings Settings;

    public TerrainButtonModel Model { get; private set; }
    public string Name { get { return name; } }
    public bool IsVisible { get { return _renderer != null; } }

    private Logger _logger;
    private AudioSource _audio;
    private MeshRenderer _renderer;

    private static IEnumerator _timerCoroutine;

    protected virtual void Start () {
        _logger = Game.Instance.LoggerFactory(name + "::TerrainButtonController");

        Model = new TerrainButtonModel(name, Settings, Game.Instance.PrincessCake.Settings);

        _audio = this.GetOrAddComponent<AudioSource>();
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    protected virtual void OnTriggerEnter(Collider collider) {
        IWeightableController controller = collider.gameObject.GetComponent<IWeightableController>();

        _logger.Info("OnTriggerEnter", collider.gameObject.name + " entered");

        if (controller != null) {
            if (!_triggeredByPlayerOnly || controller == Game.Instance.PrincessCake) {
                HopedOn(controller);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider collider) {
        IWeightableController controller = collider.gameObject.GetComponent<IWeightableController>();

        _logger.Info("OnTriggerExit", collider.gameObject.name + " left");

        if (controller != null) {
            HopedOff(controller);
        }
    }

    protected virtual void Update() {
        TryDepress();

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.I)) {
            HopedOn(Game.Instance.PrincessCake);
        }

        if (Input.GetKeyUp(KeyCode.J)) {
            HopedOff(Game.Instance.PrincessCake);
        }
#endif
    }

    // To be overriden by elevator.
    protected virtual void OnHopedOnBy(IWeightableController controller) {
        _logger.Info("OnHopedOnBy", controller.Name + " hoped on");

        foreach (TweenController tweensToOn in TweensOnAtPress) {
            tweensToOn.TryTweenToOn(true);
        }

        foreach (IController ctrl in DisableAtPress) {
            Game.Instance.Disable(ctrl);
        }

        _audio.TryPlaySFX(_onPressed);
    }

    protected virtual void OnHopedOffBy(IWeightableController controller) {
        _logger.Info("OnHopedOnBy", controller.Name + " hoped off");
        
        if (_timerCoroutine != null) {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = this.StartCoundownUI();

        StartCoroutine(_timerCoroutine);
    }

    protected virtual void OnDepressed() {
        _logger.Info("OnDepressed");

        foreach (TweenController tweensToOff in TweensOffAtDepress) {
            tweensToOff.TryTweenToOff(true);
        }

        foreach (IController ctrl in DisableAtDepress) {
            Game.Instance.Disable(ctrl);
        }

        _audio.TryPlaySFX(_onDepressed);
    }

    private void HopedOn(IWeightableController controller) {
        bool isPrincessCake = controller == Game.Instance.PrincessCake;

        if (Model.HopedOn(controller.Model())) {
            OnHopedOnBy(controller);
            if (IsVisible) {
                Game.Instance.UI.Popup.TryDisplayHopedOnButtonEnoughWeightTip(isPrincessCake);
            }
        } else {
            if (IsVisible) {
                Game.Instance.UI.Popup.TryDisplayHopedOnButtonNotEnoughWeightTip(isPrincessCake);
            }
        }
    }

    private void HopedOff(IWeightableController controller) {
        if (Model.HopedOff(controller.Model(), Time.time)) {
            OnHopedOffBy(controller);
        }
    }

    private void TryDepress() {
        if (Model.Depressed(Time.time)) {
            OnDepressed();
        }
    }

    public void OnResetEvent() {
        gameObject.SetActive(true);
    }

    public void OnDisableEvent() {
        gameObject.SetActive(false);
    }
}

public static class TerrainButtonControllerUIExtensions {

    public static IEnumerator StartCoundownUI(this TerrainButtonController ctrl) {
        Game.Instance.UI.GamePlay.ActivateTimer();

        float secondsLeft = 0.1f;
        while (secondsLeft > 0) {
            secondsLeft = ctrl.Model.PressEffectDecaysInSeconds(Time.time);

            Game.Instance.UI.GamePlay.DisplayTimer(secondsLeft);

            yield return new WaitForSeconds(0.1f);
        }

        Game.Instance.UI.GamePlay.DeactivateTimer();
    }
}
