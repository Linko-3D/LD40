using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ControlsDisplay : Display
{

#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(ControlsDisplay))]
	[CanEditMultipleObjects]
	public class ControlsDisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			ControlsDisplay sControlsDisplay = target as ControlsDisplay;
#pragma warning restore 0219
		}
	}
#endif
}