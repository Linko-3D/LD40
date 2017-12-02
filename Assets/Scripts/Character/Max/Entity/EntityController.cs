﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(EntityData), typeof(Rigidbody))]
public class EntityController : MonoBehaviour
{
	private EntityData _entityData;
	public EntityData _EntityData { get { return this._entityData; } }

	private void Awake()
	{
		this._entityData = this.GetComponent<EntityData>();

		this._entityData.Initialize(this);
	}

	private void Update()
	{
		this._entityData.EntityState.Update();
	}

	public void Move(Vector3 velocity)
	{
		velocity.y = this._entityData._RigidBody.velocity.y;

		this._entityData._RigidBody.velocity = velocity;
	}

#if UNITY_EDITOR
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(EntityController))]
	[CanEditMultipleObjects]
	public class EntityControllerEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			EntityController sEntityController = target as EntityController;
#pragma warning restore 0219
		}
	}
#endif
}