using System.Linq;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class SceneInitializer : SingletonMonobehavior<SceneInitializer> {

#if UNITY_EDITOR
    [SerializeField]
    private List<SceneAsset> m_SubSceneAssets;
#endif

    [SerializeField]
    private List<string> m_SubSceneNames;
    
    protected void Awake() {

#if UNITY_EDITOR

        if (!UnityEditor.BuildPipeline.isBuildingPlayer) {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) {
                if (!EditorUtility.IsPersistent(this)) {
                    LoadSetup_Editor();
                }
            }
        }

#else

			LoadSetup_Runtime();

#endif
    }

    public void LoadSetup_Runtime() {
        Debug.Log("SceneSetupManager:LoadSetup_Runtime");

        if (m_SubSceneNames != null && m_SubSceneNames.Count > 0) {
            foreach (string sceneName in m_SubSceneNames) {
                Debug.Log("Loading Scene: " + sceneName);
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
    }

#if UNITY_EDITOR

    public void LoadSetup_Editor() {
        Debug.Log("SceneSetupManager:LoadSetup_Editor");

        if (EditorApplication.isPlayingOrWillChangePlaymode) { return; }
        if (m_SubSceneAssets != null && m_SubSceneAssets.Count > 0) {
            foreach (SceneAsset scene in m_SubSceneAssets) {
                string scenePath = AssetDatabase.GetAssetPath(scene);
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
            if (gameObject.scene.IsValid()) {
                EditorApplication.delayCall += () => {
                    EditorSceneManager.SetActiveScene(gameObject.scene);
                };
            }
        }
    }

    protected void OnValidate() {
        m_SubSceneNames = m_SubSceneAssets.Select(s => s.name).ToList();
    }

#endif

}
