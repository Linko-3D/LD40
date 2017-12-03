using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntityStateIdle : EntityState
{
	public EntityStateIdle(EntityController entityConctoller) : base(entityConctoller)
	{
	}

	public override bool HandleInput()
	{
		return true;
	}

	public override void Update()
	{
		this.entityController.Move(Vector3.zero);
		
		if (Input.GetKey(KeyCode.Space))
		{
			this.entityController._EntityData.EntityState = new EntityStateJumping(this.entityController);
		}
		else if (
			Input.GetKey(KeyCode.W) ||
			Input.GetKey(KeyCode.S) ||
			Input.GetKey(KeyCode.A) || 
			Input.GetKey(KeyCode.D) ||
			Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) || 
			Input.GetKey(KeyCode.RightArrow))
		{
			this.entityController._EntityData.EntityState = new EntityStateWalking(this.entityController);
		}
	}

#if UNITY_EDITOR
#endif
}
