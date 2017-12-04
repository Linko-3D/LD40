using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Display : MonoBehaviour
{
	public void Open()
	{
		this.gameObject.SetActive(true);
	}

	public void Close()
	{
		this.gameObject.SetActive(false);
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{

	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(Display))]
	[CanEditMultipleObjects]
	public class DisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			Display sDisplay = target as Display;
#pragma warning restore 0219
		}
	}
#endif
}