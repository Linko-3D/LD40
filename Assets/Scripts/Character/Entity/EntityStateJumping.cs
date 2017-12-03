using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntityStateJumping : EntityState
{
	public EntityStateJumping(EntityController entityConctoller) : base(entityConctoller)
	{
		if (this.entityController._EntityData.IsGrounded)
		{
			this.entityController._EntityData._RigidBody.velocity = Vector3.zero;
			this.entityController._EntityData._RigidBody.AddForce(
				Vector3.up * 
				this.entityController._EntityData._JumpForce * 
				(1 - (Game.Instance.PrincessCake.Model.Weight / (float)Game.Instance.PrincessCake.Settings.MaxWeight)));

			this.HandleInput();
		}

		this.entityController._EntityData._GroundCheckCooldown = 0;
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

		this.entityController.Move(movement.normalized * this.entityController._EntityData._MovementSpeedJumpingState);

		return isInputDetected;
	}

	public override void Update()
	{
		if (this.entityController._EntityData.IsGrounded)
		{
			this.entityController._EntityData.EntityState = new EntityStateIdle(this.entityController);
		}
		else
		{
			this.HandleInput();
		}
	}


#if UNITY_EDITOR
#endif
}
