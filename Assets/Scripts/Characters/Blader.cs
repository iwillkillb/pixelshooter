using UnityEngine;
using System.Collections;

public class Blader : Humanoid {

	bool isTakeDamageAlready = false;

	// Animator hash
	static int animHash_Base_AttackIdle = Animator.StringToHash ("Base Layer.AttackIdle");
	static int animHash_Base_AttackCrouch = Animator.StringToHash ("Base Layer.AttackCrouch");
	static int animHash_Base_AttackAirial = Animator.StringToHash ("Base Layer.AttackAirial");



	override protected void FixedUpdate () {
		base.FixedUpdate ();


		if (hp > 0) {
			
			// Attack
			if (axisFire1 && curAttackDelay >= attackDelay) {
				curAttackDelay = 0;
				anim.SetTrigger ("attack");
			} else {
				anim.ResetTrigger ("attack");
			}

			if (baseBodyStateInfo.fullPathHash == animHash_Base_AttackIdle ||
			    baseBodyStateInfo.fullPathHash == animHash_Base_AttackCrouch ||
			    baseBodyStateInfo.fullPathHash == animHash_Base_AttackAirial) {

				// Ground attack -> Reduce speed
				// Airial attack -> Boost or Zero speed
				if (baseBodyStateInfo.fullPathHash == animHash_Base_AttackIdle ||
				    baseBodyStateInfo.fullPathHash == animHash_Base_AttackCrouch)
					rb.velocity = new Vector2 (rb.velocity.x * 0.5f, rb.velocity.y);
				else if (baseBodyStateInfo.fullPathHash == animHash_Base_AttackAirial) {
					if (isFacingRight == (axisHor > 0 ? true : false))
						rb.velocity = new Vector2 (rb.velocity.x * 2f, rb.velocity.y);
					else
						rb.velocity = new Vector2 (0f, rb.velocity.y);
				}

				if (baseBodyStateInfo.normalizedTime > 0.5f && !isTakeDamageAlready) {
					isTakeDamageAlready = true;
					Damage ();
				}
			} else {
				if (isTakeDamageAlready)
					isTakeDamageAlready = false;
			}
		}
	}

	void Damage () {
		Collider2D colInAttack;
		Vector3 curAngle = vectorToTarget;

		colInAttack = Physics2D.OverlapCircle (transform.position + (curAngle * 0.5f), 0.5f, layerHostile);
		if (colInAttack != null) {
			// Don't attack myself.
			if (colInAttack.gameObject == gameObject)
				return;

			// Physics force
			if (colInAttack.attachedRigidbody != null) {
				colInAttack.attachedRigidbody.AddForce (curAngle * 1000f);
			}

			// Damaged charcters list
			Character colChar = colInAttack.GetComponent<Character> ();
			if (colChar != null) {
				colChar.SetHp (-damage);
			}
		}
	}
}
