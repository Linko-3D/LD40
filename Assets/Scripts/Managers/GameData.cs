using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

    public void Initialize() {
        _logger = Game.Instance.LoggerFactory("GameData");
    }

    public T LoadReadonly<T>(string path) {
#if UNITY_WEBGL
        return LoadFromHolder<T>(path);
#else
        return LoadFromFile<T>(Path.Combine(Application.streamingAssetsPath, path + JSON_EXT));
#endif
    }

    public void SaveReadonly(string path, object data) {
        SaveToFile(Path.Combine(Application.streamingAssetsPath, path), data + JSON_EXT);
    }

    public T LoadState<T>(string path) {
        return LoadFromFile<T>(Path.Combine(Application.persistentDataPath, path + JSON_EXT));
    }

    public void SaveState(string path, object data) {
        SaveToFile(Path.Combine(Application.persistentDataPath, path), data + JSON_EXT);
    }

    protected T LoadFromFile<T>(string filePath) {
        _logger.Assert(File.Exists(filePath), "file: " + filePath + " does not exist.");

        string dataAsJson = File.ReadAllText(filePath);
        return JsonUtility.FromJson<T>(dataAsJson);
    }

    protected T LoadFromHolder<T>(string path)
    {
        _logger.Assert(GameDataHolder.Data.ContainsKey(path), "file: " + path + " does not exist.");

        string dataAsJson = GameDataHolder.Data[path];
        return JsonUtility.FromJson<T>(dataAsJson);
    }

    protected void SaveToFile(string filePath, object data) {
        string dataAsJson = JsonUtility.ToJson(data);
        
        File.WriteAllText(filePath, dataAsJson);
    }
}

namespace New.UTILITY {
#if UNITY_EDITOR
    [CustomEditor(typeof(GameData))]
    [CanEditMultipleObjects]
    public class GameDataEditor : Editor {

        private const string GAME_DATA_HOLDER_PATH = "Assets/Scripts/Managers/GameDataHolder.cs";
        private const string GAME_DATA_LOCALIZATION_DIR = "Localization";

        private GameData _gameData;

        private void OnEnable()
        {
            _gameData = target as GameData;

            _gameData.Initialize();
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Export Data Holder"))
            {
                ExportDataHolder();
            }
            GUILayout.Label("*Used for WebGL instead of retrieving the data via http.");
        }

        void ExportDataHolder()
        {
            Dictionary<string, string> allStreamingAssets = LoadStreamingAssets();
            string dataHolderContent = GenerateGameDataHolder(allStreamingAssets);
            File.WriteAllText(GAME_DATA_HOLDER_PATH, dataHolderContent);
        }

        Dictionary<string, string> LoadStreamingAssets()
        {
            Dictionary<string, string> allStreamingAssets = new Dictionary<string, string>();

            string localizationDirPath = Path.Combine(Application.streamingAssetsPath, GAME_DATA_LOCALIZATION_DIR);

            string[] locFilePaths = Directory.GetFiles(localizationDirPath);
            foreach (string locFilePath in locFilePaths)
            {
                string locFileNameNoExtension = Path.GetFileNameWithoutExtension(locFilePath);
                string locFileExtension = Path.GetExtension(locFilePath);
                string locFileRelativePath = Path.Combine(GAME_DATA_LOCALIZATION_DIR, locFileNameNoExtension).Replace("\\", "\\\\");

                if (locFileExtension == ".json")
                {
                    Debug.Log("Exporting streaming asset file: " + locFileRelativePath);

                    string jsonContent = File.ReadAllText(locFilePath);

                    string jsonStringified = jsonContent
                                                .Replace("\"", "\\\"")
                                                .Replace(System.Environment.NewLine, string.Empty);

                    allStreamingAssets[locFileRelativePath] = jsonStringified;
                }
            }

            return allStreamingAssets;
        }

        string GenerateGameDataHolder(Dictionary<string, string> streamingAssets)
        {
            string content = string.Empty;
            content += "using System.Collections.Generic;\n";
            content += "\n";
            content += "public static class GameDataHolder {\n";
            content += "\n";
            content += "    public static Dictionary<string, string> Data = new Dictionary<string, string>() {\n";

            int pathsLeft = streamingAssets.Count;
            foreach (string path in streamingAssets.Keys)
            {
                --pathsLeft;
                
                if (pathsLeft != 0)
                {
                    content += "        { \"" + path + "\", \"" + streamingAssets[path] + "\"},\n";
                }
                else
                {
                    content += "        { \"" + path + "\", \"" + streamingAssets[path] + "\"}\n";
                }
            }
            content += "    };\n";
            content += "\n";
            content += "}\n";
            return content;
        }
    }

#endif
}
