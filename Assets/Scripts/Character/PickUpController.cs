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

            Game.Instance.Logger.Info(_pickedUpBox);

            if (_pickedUpBox != null && _pickedUpBox.Model.CanMoveBy(Game.Instance.PrincessCake.Model)) {
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

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos() {

    }
#endif
}

namespace New.UTILITY {
#if UNITY_EDITOR
    [CustomEditor(typeof(PickUpController))]
    [CanEditMultipleObjects]
    public class PickUpControllerEditor : Editor {
        private void OnEnable() {

        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

#pragma warning disable 0219
            PickUpController sPickUpController = target as PickUpController;
#pragma warning restore 0219
        }
    }
#endif
}
