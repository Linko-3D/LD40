using UnityEngine;

public class RotationController : MonoBehaviour {

    [SerializeField]
    private float _speed = 200;
    
    private readonly Vector3 _rotationToApply = Vector3.up;

    protected void FixedUpdate() {
        transform.Rotate(Vector3.up * _speed * Time.fixedDeltaTime);
    }
}
