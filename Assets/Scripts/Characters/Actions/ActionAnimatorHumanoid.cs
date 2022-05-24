using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class ActionAnimatorHumanoid : Action {

	Animator anim;
	Rigidbody2D rb;



	protected override void Init ()
	{
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	protected override void Effect ()
	{
		if (
			(character.axisHor > 0 && !character.IsFacingRight ()) ||
			(character.axisHor < 0 && character.IsFacingRight ())
		) {
			// Move input != Character direction
			anim.SetFloat ("move", -Mathf.Abs (character.axisHor) * activation);
		} else if (
			(character.axisHor > 0 && character.IsFacingRight ()) ||
			(character.axisHor < 0 && !character.IsFacingRight ()) ||
			character.axisHor == 0
		) {
			// Move input == Character direction
			anim.SetFloat ("move", Mathf.Abs (character.axisHor) * activation);
		}

		if (character.ctTop.IsTerrain ()) {
			anim.SetBool ("isCrouch", true);
		} else {
			anim.SetBool ("isCrouch", character.axisVer < 0f);
			anim.SetFloat ("fly", rb.velocity.y * 0.5f);
		}

		anim.SetBool ("isGround", character.ctBottom.IsTerrain ());
	}
}
