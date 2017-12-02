using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameData : MonoBehaviour
{
	[SerializeField] private LayerMask _groundLayerMask;
	public LayerMask _GroundLayerMask { get { return this._groundLayerMask; } }
}

namespace New.UTILITY
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameData))]
    [CanEditMultipleObjects]
    public class GameDataEditor : Editor
    {
        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

#pragma warning disable 0219
            GameData sGameData = target as GameData;
#pragma warning restore 0219
        }
    }
#endif
}