﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrincessCakeController : MonoBehaviour, IWeightableController {

    [SerializeField]
    private AudioClip _onConsumeCake;
    [SerializeField]
    private AudioClip _onConsumeTea;

    public PrincessCakeModel.Settings Settings = new PrincessCakeModel.Settings();

    public PrincessCakeModel Model { get; private set; }

    public string Name { get { return name; } }

    IWeightableModel IWeightableController.Model() {
        return Model;
    }

    private AudioSource _audio;

    protected void Start() {
        Model = new PrincessCakeModel(name, Settings);

        _audio = this.GetOrAddComponent<AudioSource>();

        Model.OnConsumeCake += () => _audio.TryPlaySFX(_onConsumeCake);
        Model.OnConsumeTea += () => _audio.TryPlaySFX(_onConsumeTea);
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
