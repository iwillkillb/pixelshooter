using UnityEngine;
using System.Collections;

public class ActionMove : Action {

	[Header ("Option")]
	public bool uerMovingFlip = true;
	public bool useRigidBody2d = false;

	[Header ("Move")]
	float axisMove;
	public float fastSpeed = 10f;
	public float slowSpeed = 5f;
	public float climbSlopeLimitAngle = 45f;
	float curSlopeAngle;

	Rigidbody2D rb;



	protected override void Init ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	protected override void Effect ()
	{
		// Flip
		if (uerMovingFlip) {
			if ((character.axisHor > 0f && !character.IsFacingRight ()) || (character.axisHor < 0f && character.IsFacingRight ()))
				character.Flip ();
		}

		// Down key -> Slow speed
		float speedFastTemp = 1f;
		if (character.axisVer < 0f)
			speedFastTemp = character.axisVer + 1f;

		float moveSpeed = Mathf.Lerp (slowSpeed, fastSpeed, speedFastTemp);

		// On ground -> Save slope angle and move input.
		if (character.ctBottom.IsTerrain ()) {
			curSlopeAngle = GetSlopeAngle ();
		}
		axisMove = character.axisHor;
		if (rb != null && useRigidBody2d)
			rb.velocity = new Vector2 (axisMove * moveSpeed * activation, rb.velocity.y);
		else
			transform.Translate (Vector3.right * axisMove * moveSpeed * Time.deltaTime * activation);
		/*
		// Actual moving : Limit by slope angle
		if ((curSlopeAngle < climbSlopeLimitAngle && curSlopeAngle > -climbSlopeLimitAngle) ||
			((curSlopeAngle > climbSlopeLimitAngle && axisMove < 0f) || (curSlopeAngle < -climbSlopeLimitAngle && axisMove > 0f))
		) {
			if (rb != null && useRigidBody2d)
				rb.velocity = new Vector2 (axisMove * moveSpeed * activation, rb.velocity.y);
			else
				transform.Translate (Vector3.right * axisMove * moveSpeed * Time.deltaTime * activation);
		}*/
	}

	float GetSlopeAngle () {
		RaycastHit2D hit = Physics2D.Raycast (character.ctBottom.transform.position, Vector3.down, character.ctBottom.range, character.ctBottom.layerOfTerrains);
		return Mathf.Atan2 (hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
	}
}
