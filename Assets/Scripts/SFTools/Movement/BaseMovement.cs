using UnityEngine;
using System.Collections;
using AssemblyCSharp;

[HideInInspector]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseMovement : MonoBehaviour {

	#region Editor Properties

	public float MoveSpeed = 50;
    public float RotOffset = 0;

	#endregion

	#region Private Members

	protected Rigidbody2D rigidBody;

	#endregion

    #region Public Properties

    public Vector2 Direction
    {
        get { return rigidBody.velocity.normalized; }
    }

    #endregion

    #region Public Interface

    public void StartMove(Vector3 moveDir, float speed)
    {
        rigidBody.velocity = Vector2.zero;
        MoveToward(moveDir, speed);
    }

	public abstract void StartMove();

    public abstract void PauseMove();

    public virtual void StartMove(Animator animator)
    {
        StartMove();
    }

	public virtual void PauseMove(Animator animator)
    {
        PauseMove();
    }

	public virtual void ResetSpeed()
	{
		PauseMove();
		StartMove();
	}

	public void ReverseDirection()
	{
		Vector2 velocity = rigidBody.velocity.normalized;
		
		rigidBody.velocity = Vector2.zero;
		rigidBody.AddForce(-velocity * MoveSpeed);
	}

	public virtual void StopMove()
	{
		PauseMove();
	}

	#endregion

	#region Privare Routines

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	protected virtual void AdjustSpeed(float factor)
	{
		rigidBody.velocity *= factor;
	}

    protected void MoveToward(Vector3 direction)
    {
        MoveToward(direction, MoveSpeed);
    }

    protected void MoveToward(Vector3 direction, float speed)
    {
        transform.localRotation = Quaternion.AngleAxis(MathUtil.VectorToAngle(direction) + RotOffset, -Vector3.back);
        rigidBody.AddForce(direction * speed);
    }

	#endregion	
}
