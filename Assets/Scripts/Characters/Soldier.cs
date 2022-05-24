using UnityEngine;
using System.Collections;

public class Soldier : Humanoid {

	[Header ("Shooting")]
	public float throwPower = 7f;
	public Transform bulletSpawnTrn;
	public Sprite bulletSprite;
	public BulletPool bulletPool;
	public GrenadePool grenadePool;

	// Input
	bool isThrowedGrenadeAlready = false;

	// Animator hash
	AnimatorStateInfo upperBodyStateInfo;
	static int animHash_Upper_Aim = Animator.StringToHash ("Upper.Aim");
	static int animHash_Upper_Shot = Animator.StringToHash ("Upper.Shot");
	static int animHash_Upper_Grenade = Animator.StringToHash ("Upper.Grenade");



	override protected void FixedUpdate () {
		base.FixedUpdate ();

		if (hp > 0) {
			upperBodyStateInfo = anim.GetCurrentAnimatorStateInfo (anim.GetLayerIndex ("Upper"));

			// Shot
			if ((upperBodyStateInfo.fullPathHash == animHash_Upper_Aim || upperBodyStateInfo.fullPathHash == animHash_Upper_Shot) &&
				axisFire1 &&
			    curAttackDelay >= attackDelay)
				Shot ();
			if (axisFire2)
				anim.Play ("Grenade");

			// Grenade is called one time.
			if (upperBodyStateInfo.fullPathHash == animHash_Upper_Grenade && upperBodyStateInfo.normalizedTime >= 0.66f) {
				if (!isThrowedGrenadeAlready) {
					isThrowedGrenadeAlready = true;
					Vector3 grenadeThrowForce = vectorToTarget * throwPower;
					Transform trnGrenade = aimPivot;
					trnGrenade.rotation = Quaternion.identity;
					trnGrenade.localScale = flipTrn.localScale;
					grenadePool.CallObj (gameObject, grenadeThrowForce, trnGrenade);
				}
			} else if (upperBodyStateInfo.fullPathHash != animHash_Upper_Grenade) {
				if (isThrowedGrenadeAlready)
					isThrowedGrenadeAlready = false;
			}
		}
	}




	void Shot () {
		curAttackDelay = 0;
		anim.Play ("Shot");
		// Spawn bullet : Animation "Shot" -> Call bullet object to Object pool.
		Transform trnBullet = bulletSpawnTrn;
		trnBullet.localScale = flipTrn.localScale;
		//bulletPool.CallObj (trnBullet, gameObject, layerHostile, damage, bulletSprite);
		audioS.PlayOneShot (audioAttack);
	}
}
