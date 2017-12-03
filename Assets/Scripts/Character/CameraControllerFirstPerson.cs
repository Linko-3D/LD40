using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraControllerFirstPerson : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _smoothingFactor = 1.5f;

	private void LateUpdate()
	{
		this.transform.position = Vector3.Lerp(this.transform.position, this._target.position, Time.deltaTime * this._smoothingFactor);
		this.transform.rotation = this._target.transform.rotation;
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{

	}
#endif
}

namespace Kimo.New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(CameraControllerFirstPerson))]
	[CanEditMultipleObjects]
	public class CameraControllerFirstPersonEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			CameraControllerFirstPerson sCameraControllerFirstPerson = target as CameraControllerFirstPerson;
#pragma warning restore 0219
		}
	}
#endif
}