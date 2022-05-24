using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class ActionAiming : Action {

	[Header ("Aiming")]
	public Transform aimPivot;
	public Transform trnAnglePlus;
	[HideInInspector]public float degreeToTarget;
	[HideInInspector]public Vector3 vectorToTarget;

	Animator anim;



	protected override void Init ()
	{
		anim = GetComponent<Animator> ();
	}

	// This is used by Input script.
	public void SetAngle (Vector3 targetPos) {	// This codes find Mouse cursor. Player uses this, but Other use new codes.
		if (!enabled)
			return;

		float plusAngle = trnAnglePlus.eulerAngles.z;				// Take pelvis's world angle.
		if (plusAngle > 180)										// -180 ~ 180
			plusAngle -= 360;

		float dx, dy;

		// XY Length to target -> Degree to target
		dx = targetPos.x - aimPivot.position.x;
		dy = targetPos.y - aimPivot.position.y;
		degreeToTarget = Mathf.Atan2 (dy, dx) * Mathf.Rad2Deg - plusAngle;		// Angle from chracter's arm to mouse cursor.
		vectorToTarget = new Vector3 (dx, dy, 0f).normalized;

		// Not Equal character's direction and target postion -> FLIP
		if (character.IsFacingRight () && Mathf.Abs (degreeToTarget) > 95f)
			character.Flip ();	// Facing right And Target is left
		else if (!character.IsFacingRight () && Mathf.Abs (degreeToTarget) < 85f)
			character.Flip ();	// Facing left And Target is right

		if (anim != null) {
			float degreeParameter; // -1 ~ 1
			// Aiming parameter
			if (character.IsFacingRight ())	// degreeToTarget : -90 ~ 90, degreeParameter : -1 ~ 1
				degreeParameter = degreeToTarget / 90f;
			else {
				if (degreeToTarget > 0)		// degreeToTarget : 180 ~ 90 -> 0 ~ 1
					degreeParameter = (degreeToTarget - 180f) / -90f;
				else						// degreeToTarget : -90 ~ -180 -> -1 ~ 0
					degreeParameter = (degreeToTarget + 180f) / -90f;
			}
			anim.SetFloat ("aimAngle", Mathf.Lerp (0f, degreeParameter, activation));
		}
	}
}
