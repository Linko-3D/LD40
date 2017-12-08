using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// GameData Layer abstraction to hide the .json nature of the data in the codebase.
/// </summary>
public class GameData : SingletonMonobehaviour<GameData> {

    private const string JSON_EXT = ".json";

    [SerializeField] private LayerMask _groundLayerMask;

    public LayerMask _GroundLayerMask { get { return _groundLayerMask; } }

    private Logger _logger;

    protected void Awake() {
        _logger = Game.Instance.LoggerFactory("GameData");
    }

    public T LoadReadonly<T>(string path) {
        return Load<T>(Path.Combine(Application.streamingAssetsPath, path + JSON_EXT));
    }

    public void SaveReadonly(string path, object data) {
        Save(Path.Combine(Application.streamingAssetsPath, path), data + JSON_EXT);
    }

    public T LoadState<T>(string path) {
        return Load<T>(Path.Combine(Application.persistentDataPath, path + JSON_EXT));
    }

    public void SaveState(string path, object data) {
        Save(Path.Combine(Application.persistentDataPath, path), data + JSON_EXT);
    }

    protected T Load<T>(string filePath) {
        _logger.Assert(File.Exists(filePath), "file: " + filePath + " does not exist.");

        string dataAsJson = File.ReadAllText(filePath);

        return JsonUtility.FromJson<T>(dataAsJson);
    }

    protected void Save(string filePath, object data) {
        string dataAsJson = JsonUtility.ToJson(data);
        
        File.WriteAllText(filePath, dataAsJson);
    }
}

namespace New.UTILITY {
#if UNITY_EDITOR
    [CustomEditor(typeof(GameData))]
    [CanEditMultipleObjects]
    public class GameDataEditor : Editor {
        private void OnEnable() {

        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

#pragma warning disable 0219
            GameData sGameData = target as GameData;
#pragma warning restore 0219
        }
    }
#endif
}