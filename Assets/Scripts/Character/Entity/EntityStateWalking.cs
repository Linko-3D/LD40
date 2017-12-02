using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntityStateWalking : EntityState
{
	public EntityStateWalking(EntityController entityConctoller) : base(entityConctoller)
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			this.entityController.Move(Vector3.left * this.entityController._EntityData._MovementSpeedWalkingState);
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			this.entityController.Move(Vector3.right * this.entityController._EntityData._MovementSpeedWalkingState);
		}
	}

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.entityController._EntityData.EntityState = new EntityStateJumping(this.entityController);
		}
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			this.entityController.Move(Vector3.left * this.entityController._EntityData._MovementSpeedWalkingState);
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			this.entityController.Move(Vector3.right * this.entityController._EntityData._MovementSpeedWalkingState);
		}
		else
		{
			this.entityController._EntityData.EntityState = new EntityStateIdle(this.entityController);
		}
	}

#if UNITY_EDITOR
#endif
}
