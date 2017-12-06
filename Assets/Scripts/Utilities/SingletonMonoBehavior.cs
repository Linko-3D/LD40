using UnityEngine;

/// <summary>
/// This Singleton generic should be used when in need of a singleton class
/// that utilizes the Monobehavior API.
///
/// More info: http://wiki.unity3d.com/index.php/Singleton
/// </summary>
public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    private static string NAME = "Singleton<" + typeof(T).Name + ">";

    private static object _lock = new object();
    private static bool _applicationIsQuitting = false;
    private static T _instance;

    protected SingletonMonobehaviour() { }

    public static T Instance {
        get {
            lock (_lock) {
                if (_instance == null) {
                    _applicationIsQuitting = false;
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1) {
                        Debug.Log(NAME + ": Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                    }
                } else if (_applicationIsQuitting) {
                    Debug.Log(NAME + ": Instance '" +
                        "' already destroyed on application quit." +
                        " Won't create again - returning null.");
                    return null;
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnApplicationQuit() {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy() {
        _applicationIsQuitting = true;
    }
}
