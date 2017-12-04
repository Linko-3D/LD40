using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrincessCakeController : MonoBehaviour, IWeightableController {

    public event Action OnResetToCheckpoint;
    public event Action OnCheckpointAcquired;

    [SerializeField]
    private float _weightRadiusModifier = .1f;

    [SerializeField]
    private AudioClip _theme;
    [SerializeField]
    private AudioClip _onConsumeCake;
    [SerializeField]
    private AudioClip _onConsumeTea;
    [SerializeField]
    private AudioClip _onCheckpointAcquired;
    [SerializeField]
    private AudioClip _onResetToCheckpoint;

    public PrincessCakeModel.Settings Settings = new PrincessCakeModel.Settings();

    public PrincessCakeModel Model { get; private set; }

    public string Name { get { return name; } }

    IWeightableModel IWeightableController.Model() {
        return Model;
    }

    private Vector3 _lastCheckpoint;
    private PrincessCakeModel _lastCheckpointState;

    private CharacterController _characterCtrl;
    private float _characterCtrlDefaultRadius;
    private AudioSource _audio;
    
    private void Awake() {
        Model = new PrincessCakeModel(name, Settings);

        _characterCtrl = this.GetOrAddComponent<CharacterController>();
        _characterCtrlDefaultRadius = _characterCtrl.radius;
        _audio = this.GetOrAddComponent<AudioSource>();

        Model.OnConsumeCake += () => {
            _audio.TryPlaySFX(_onConsumeCake);
            UpdateCharacterCtrlRadius();
        };
        Model.OnConsumeTea += () => {
            _audio.TryPlaySFX(_onConsumeTea);
            UpdateCharacterCtrlRadius();
        };

        _audio.TryPlayTheme(_theme);

        _lastCheckpoint = transform.position;
        _lastCheckpointState = new PrincessCakeModel(Model);
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.R)) {
            OnResetEvent();
        }
    }

    private void UpdateCharacterCtrlRadius() {
        _characterCtrl.radius = _characterCtrlDefaultRadius + Model.Weight * _weightRadiusModifier;
    }

    public void SetCheckpoint(Vector3 pos) {
        _lastCheckpoint = pos;

        // uncomment to keep last stats as previous checkpoint
        //_lastCheckpointState.CopyStats(Model);

        _audio.TryPlaySFX(_onCheckpointAcquired);

        if (OnCheckpointAcquired != null) {
            OnCheckpointAcquired();
        }
    }
    
    public void OnResetEvent() {
        transform.position = _lastCheckpoint;
        Model.CopyStats(_lastCheckpointState);

        UpdateCharacterCtrlRadius();

        _audio.TryPlaySFX(_onResetToCheckpoint);

        if (OnResetToCheckpoint != null) {
            OnResetToCheckpoint();
        }
    }

    public void OnDisableEvent() {
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (Model == null) return;

        Handles.Label(transform.position, Name);
        Handles.Label(transform.position + Vector3.down, "Weight: " + Model.Weight);
        Handles.Label(transform.position + Vector3.down * 2, "CakesEaten: " + Model.CakesEaten);
        Handles.Label(transform.position + Vector3.down * 3, "TeasDrunk: " + Model.TeasDrunk);
    }

#endif
}
