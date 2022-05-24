using UnityEngine;
using System.Collections;

public class ActionMeeleAttack : ActionAttack {

	[Header ("Meele Attack")]
	public BoxCollider2D boxColAttack;



	protected override void Attack ()
	{
		if (anim != null) {
			if (anim.GetBool ("isCrouch")) {
				anim.Play ("Sting");
			} else {
				anim.Play ("Slash");
			}
		}

		Collider2D colInAttack;
		colInAttack = Physics2D.OverlapBox (transform.TransformPoint (boxColAttack.offset), boxColAttack.size, 0f, layerHostile); 

		if (colInAttack != null) {
			// Don't attack myself.
			if (colInAttack.gameObject == gameObject)
				return;
			
			// Damaged charcters list
			Character colChar = colInAttack.GetComponent<Character> ();
			if (colChar != null) {
				colChar.SetHp (-damage);
			}
		}
	}
}
