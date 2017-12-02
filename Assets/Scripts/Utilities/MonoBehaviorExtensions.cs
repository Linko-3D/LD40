using UnityEngine;

/// <summary>
/// Monobehavior extensions to prevent boilerplate code.
/// 
/// More info: https://unity3d.com/learn/tutorials/topics/scripting/extension-methods
/// </summary>
public static class MonoBehaviourExtensions {

    /// <summary>
    /// Gets or adds a component of the specified type.
    /// 
    /// Usage:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// 
    /// More info: http://wiki.unity3d.com/index.php/GetOrAddComponent
    /// </summary>
    public static T GetOrAddComponent<T>(this Transform transform) where T : Component {
        T result = transform.GetComponent<T>();
        if (result == null) {
            result = transform.gameObject.AddComponent<T>();
        }
        return result;
    }

    /// <summary>
    /// Alias of GetOrAddComponent<T> to be called from the component instead of transform.
    /// 
    /// Usage:
    /// BoxCollider boxCollider = this.GetOrAddComponent<BoxCollider>();
    /// </summary>
    public static T GetOrAddComponent<T>(this Component component) where T : Component {
        return GetOrAddComponent<T>(component.transform);
    }

    /// <summary>
    /// Checks whether the game object has a component of the specified type.
    /// 
    /// Usage from MonoBehaviour:
    /// if(!transform.HasComponent<BoxCollider>()) return;
    /// </summary>
    public static bool HasComponent<T>(this Transform transform) where T : Component {
        T result = transform.GetComponent<T>();
        return result != null;
    }

    /// <summary>
    /// Alias of HasComponent<T> to be called from the component instead of transform.
    /// 
    /// Usage from MonoBehaviour:
    /// if(!this.HasComponent<BoxCollider>()) return;
    /// </summary>
    public static bool HasComponent<T>(this Component component) where T : Component {
        return HasComponent<T>(component.transform);
    }

    /// <summary>
    /// Resets position, rotation & scale of the transform
    /// 
    /// Usage from MonoBehaviour:
    /// transform.Reset();
    /// </summary>
    public static void Reset(this Transform transform) {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Sets position X axis of the tranformation
    /// 
    /// Usage from MonoBehaviour:
    /// transform.SetPosX(0);
    /// </summary>
    public static void SetPosX(this Transform transform, float val) {
        transform.position = new Vector3(val, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Sets position Y axis of the tranformation
    /// 
    /// Usage from MonoBehaviour:
    /// transform.SetPosY(0);
    /// </summary>
    public static void SetPosY(this Transform transform, float val) {
        transform.position = new Vector3(transform.position.x, val, transform.position.z);
    }

    /// <summary>
    /// Sets position Z axis of the tranformation
    /// 
    /// Usage from MonoBehaviour:
    /// transform.SetPosZ(0);
    /// </summary>
    public static void SetPosZ(this Transform transform, float val) {
        transform.position = new Vector3(transform.position.x, transform.position.y, val);
    }
}
