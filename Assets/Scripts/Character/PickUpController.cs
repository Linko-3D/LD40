using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PickUpController : MonoBehaviour {
    private BoxController _pickedUpBox;

    [SerializeField] private float _initialDelayBeforeAllowDrop = 2f;

    [SerializeField] private AudioClip _onPickUp;
    [SerializeField] private AudioClip _onPut;

    private AudioSource _audio;

    private void Awake() {
        _audio = this.GetOrAddComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.E) && _pickedUpBox == null) {
            _pickedUpBox = other.GetComponent<BoxController>();

            if (_pickedUpBox != null && _pickedUpBox.Model.CanMoveBy(Game.Instance.PrincessCake.Model)) {

                Game.Instance.Logger.Info("Picked Up box: " +_pickedUpBox);

                _pickedUpBox.GetComponent<Rigidbody>().isKinematic = true;
                _pickedUpBox.transform.position = transform.position;
                _pickedUpBox.transform.SetParent(transform);

                StopAllCoroutines();
                StartCoroutine(HandleInput());

                _audio.TryPlaySFX(_onPickUp);
            } else {
                StopAllCoroutines();
                _pickedUpBox = null;
            }
        }
    }

    private IEnumerator HandleInput() {
        yield return new WaitForSeconds(_initialDelayBeforeAllowDrop);

        while (_pickedUpBox != null) {
            if (Input.GetKeyDown(KeyCode.E)) {
                _pickedUpBox.GetComponent<Rigidbody>().isKinematic = false;
                _pickedUpBox.transform.SetParent(null);
                _pickedUpBox = null;

                _audio.TryPlaySFX(_onPut);
            }

            yield return null;
        }
    }
}
