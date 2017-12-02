using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntityData : MonoBehaviour
{
	private EntityController _entityController;

	[Header("Movement")]
	[SerializeField]
	private float _movementSpeedWalkingState = 5f;
	public float _MovementSpeedWalkingState { get { return this._movementSpeedWalkingState; } }

	[SerializeField] private float _movementSpeedJumpingState = 3f;
	public float _MovementSpeedJumpingState { get { return this._movementSpeedJumpingState; } }


	[Header("Grounding")]
	[SerializeField] private Transform _groundCheckPoint;
	public Transform _GroundCheckPoint { get { return this._groundCheckPoint; } }

	[SerializeField] private float _groundCheckRadius = 0.4f;
	public float _GroundCheckRadius { get { return this._groundCheckRadius; } }

	private bool _isGrounded;
	public bool IsGrounded { get { return this._isGrounded; } set { this._isGrounded = value; } }

	public const float GROUND_CHECK_DELAY = 0.4f;

	private float _groundCheckCooldown = 0;
	public float _GroundCheckCooldown
	{
		get { return this._groundCheckCooldown; }
		set { this._groundCheckCooldown = value; if (this._groundCheckCooldown < GROUND_CHECK_DELAY) { this._isGrounded = false; } }
	}

	[Header("Jumping")]
	[SerializeField]
	private float _jumpForce = 500f;
	public float _JumpForce { get { return this._jumpForce; } }

	private EntityState _entityState;
	public EntityState EntityState { get { return this._entityState; } set { this._entityState = value; } }

	private Rigidbody _rigidbody;
	public Rigidbody _RigidBody { get { return this._rigidbody; } }

	public void Initialize(EntityController entityController)
	{
		this._entityController = entityController;

		this._entityState = new EntityStateIdle(this._entityController);
		this._rigidbody = this._entityController.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (this._groundCheckCooldown > GROUND_CHECK_DELAY)
		{
			this._isGrounded = Physics.CheckSphere(
				this._groundCheckPoint.transform.position,
				this._groundCheckRadius,
				GameController.Instance_._GameData._GroundLayerMask);
		}
		this._groundCheckCooldown += Time.deltaTime;
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(this._groundCheckPoint.position, this._groundCheckRadius);
	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
    [CustomEditor(typeof(EntityData))]
    [CanEditMultipleObjects]
    public class EntityDataEditor : Editor
    {
        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

#pragma warning disable 0219
            EntityData sEntityData = target as EntityData;
#pragma warning restore 0219
        }
    }
#endif
}