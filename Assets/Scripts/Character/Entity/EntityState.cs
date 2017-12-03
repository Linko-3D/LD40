using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class EntityState
{
	protected EntityController entityController;

	public EntityState(EntityController entityConctoller)
	{
		Debug.Log(this.ToString());

		this.entityController = entityConctoller;
	}

	public abstract bool HandleInput();
	public abstract void Update();

#if UNITY_EDITOR
#endif
}
