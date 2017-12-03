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
		this.HandleInput();
	}

	public override bool HandleInput()
	{
		bool isInputDetected = false;
		Vector3 movement = Vector3.zero;

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			movement += this.entityController.transform.forward;
			movement.y = 0;
			
			isInputDetected = true;
		}
		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			movement -= this.entityController.transform.forward;
			movement.y = 0;
			
			isInputDetected = true;
		}

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			movement -= this.entityController.transform.right;
			
			isInputDetected = true;
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			movement += this.entityController.transform.right;

			isInputDetected = true;
		}

		this.entityController.Move(movement.normalized * this.entityController._EntityData._MovementSpeedWalkingState);

		return isInputDetected;
	}

	public override void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			this.entityController._EntityData.EntityState = new EntityStateJumping(this.entityController);
		}
		else if (!this.HandleInput())
		{
			this.entityController._EntityData.EntityState = new EntityStateIdle(this.entityController);
		}
	}

#if UNITY_EDITOR
#endif
}
