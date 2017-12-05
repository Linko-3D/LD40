using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PickUpController : MonoBehaviour {
    private BoxController _pickedUpBox;

    private float _initialDelayBeforeAllowDrop = 2f;

    [SerializeField] private AudioClip _onPickUp;
    [SerializeField] private AudioClip _onPut;

    private AudioSource _audio;

    private void Awake() {
        this._audio = this.GetOrAddComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.E) && this._pickedUpBox == null) {
            this._pickedUpBox = other.GetComponent<BoxController>();

            Game.Instance.Logger.Info(this._pickedUpBox);

            if (this._pickedUpBox != null && this._pickedUpBox.Model.CanMoveBy(Game.Instance.PrincessCake.Model)) {
                this._pickedUpBox.GetComponent<Rigidbody>().isKinematic = true;
                this._pickedUpBox.transform.position = this.transform.position;
                this._pickedUpBox.transform.SetParent(this.transform);

                this.StopAllCoroutines();
                this.StartCoroutine(this.HandleInput());

                this._audio.TryPlaySFX(this._onPickUp);
            } else {
                this.StopAllCoroutines();
                this._pickedUpBox = null;
            }
        }
    }

    private IEnumerator HandleInput() {
        yield return new WaitForSeconds(_initialDelayBeforeAllowDrop);

        while (this._pickedUpBox != null) {
            if (Input.GetKeyDown(KeyCode.E)) {
                this._pickedUpBox.GetComponent<Rigidbody>().isKinematic = false;
                this._pickedUpBox.transform.SetParent(null);
                this._pickedUpBox = null;

                this._audio.TryPlaySFX(this._onPut);
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
