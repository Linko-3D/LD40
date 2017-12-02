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
			this.entityController._EntityData._RigidBody.AddForce(Vector3.up * this.entityController._EntityData._JumpForce);

			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				this.entityController.Move(Vector3.left * this.entityController._EntityData._MovementSpeedJumpingState);
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				this.entityController.Move(Vector3.right * this.entityController._EntityData._MovementSpeedJumpingState);
			}
		}

		this.entityController._EntityData._GroundCheckCooldown = 0;
	}

	public override void Update()
	{
		if (this.entityController._EntityData.IsGrounded)
		{
			this.entityController._EntityData.EntityState = new EntityStateIdle(this.entityController);
		}
		else
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				this.entityController.Move(Vector3.left * this.entityController._EntityData._MovementSpeedJumpingState);
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				this.entityController.Move(Vector3.right * this.entityController._EntityData._MovementSpeedJumpingState);
			}
		}
	}


#if UNITY_EDITOR
#endif
}
