using System;
using System.Collections;

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
    private AudioClip _onConsumeCake;
    [SerializeField]
    private AudioClip _onConsumeCakeChubby;
    [SerializeField]
    private AudioClip _onConsumeCakeFat;
    [SerializeField]
    private AudioClip _onGainWeight;
    [SerializeField]
    private float _gainWeightSFXDelayAfterEatInSeconds = .5f;
    [SerializeField]
    private AudioClip _onConsumeTea;
    [SerializeField]
    private AudioClip _onLoseWeight;
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
            if (Model.IsThin) {
                _audio.TryPlaySFX(_onConsumeCake);
            } else if (Model.IsChubby) {
                _audio.TryPlaySFX(_onConsumeCakeChubby);
            } else {
                _audio.TryPlaySFX(_onConsumeCakeFat);
            }

            StartCoroutine(
                TryPlaySFXAfterSeconds(_gainWeightSFXDelayAfterEatInSeconds, _onGainWeight)
            );

            UpdateCharacterCtrlRadius();
        };
        Model.OnConsumeTea += () => {
            _audio.TryPlaySFX(_onConsumeTea);

            StartCoroutine(
                TryPlaySFXAfterSeconds(_gainWeightSFXDelayAfterEatInSeconds, _onLoseWeight)
            );

            UpdateCharacterCtrlRadius();
        };
        
        _lastCheckpoint = transform.position;
        _lastCheckpointState = new PrincessCakeModel(Model);
    }

    private IEnumerator TryPlaySFXAfterSeconds(float sec, AudioClip clip) {
        yield return new WaitForSeconds(sec);

        _audio.TryPlaySFX(clip);
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
    private GUIStyle _gizmosStyle;

    private void OnDrawGizmosSelected() {
        if (Model == null) return;

        if (_gizmosStyle == null) {
            _gizmosStyle = new GUIStyle() { fontSize = 20 };
        }

        Gizmos.color = Color.red;
        Handles.color = Color.red;
        Handles.Label(transform.position, Name, _gizmosStyle);
        Handles.Label(transform.position + Vector3.back * 3, "Weight: " + Model.Weight, _gizmosStyle);
        Handles.Label(transform.position + Vector3.back * 6, "CakesEaten: " + Model.CakesEaten, _gizmosStyle);
        Handles.Label(transform.position + Vector3.back * 9, "TeasDrunk: " + Model.TeasDrunk, _gizmosStyle);
    }

#endif
}
