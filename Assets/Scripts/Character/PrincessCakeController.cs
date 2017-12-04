using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrincessCakeController : MonoBehaviour, IWeightableController {

    public event Action OnResetToCheckpoint;
    public event Action OnCheckpoitAcquired;

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

    [SerializeField]
    public PrincessCakeModel Model { get; private set; }

    public string Name { get { return name; } }

    IWeightableModel IWeightableController.Model() {
        return Model;
    }

    [SerializeField]
    private Vector3 _lastCheckpoint;
    [SerializeField]
    private PrincessCakeModel _lastCheckpointState;

    private AudioSource _audio;

    // the radius is at 0.5 at 1 weight

    protected void Start() {
        Model = new PrincessCakeModel(name, Settings);

        _audio = this.GetOrAddComponent<AudioSource>();

        Model.OnConsumeCake += () => _audio.TryPlaySFX(_onConsumeCake);
        Model.OnConsumeTea += () => _audio.TryPlaySFX(_onConsumeTea);

        _audio.TryPlayTheme(_theme);

        _lastCheckpoint = transform.position;
        _lastCheckpointState = new PrincessCakeModel(Model);
    }

    protected void Update() {
        if (Input.GetKeyUp(KeyCode.R)) {
            OnResetEvent();
        }
    }

    public void SetCheckpoint(Vector3 pos) {
        _lastCheckpoint = pos;

        // uncomment to keep last stats as previous checkpoint
        //_lastCheckpointState.CopyStats(Model);

        _audio.TryPlaySFX(_onCheckpointAcquired);

        if (OnCheckpoitAcquired != null) {
            OnCheckpoitAcquired();
        }
    }
    
    public void OnResetEvent() {
        transform.position = _lastCheckpoint;
        Model.CopyStats(_lastCheckpointState);

        _audio.TryPlaySFX(_onResetToCheckpoint);

        if (OnResetToCheckpoint != null) {
            OnResetToCheckpoint();
        }
    }

    public void OnDisableEvent() {
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected void OnDrawGizmosSelected() {
        if (Model == null) return;

        Handles.Label(transform.position, Name);
        Handles.Label(transform.position + Vector3.down, "Weight: " + Model.Weight);
        Handles.Label(transform.position + Vector3.down * 2, "CakesEaten: " + Model.CakesEaten);
        Handles.Label(transform.position + Vector3.down * 3, "TeasDrunk: " + Model.TeasDrunk);
    }

#endif
}
