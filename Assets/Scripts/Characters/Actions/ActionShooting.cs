using UnityEngine;
using System.Collections;

public class ActionShooting : ActionAttack {

	[Header ("Shooting")]
	public float bulletSpeed = 20f;
	public Transform bulletSpawnTrn;
	public BulletPool bulletPool;



	protected override void Attack ()
	{
		if (anim != null)
			anim.Play ("Shot");

		// Spawn bullet : Animation "Shot" -> Call bullet object to Object pool.
		Transform trnBullet = bulletSpawnTrn;
		if (character.IsFacingRight ())
			trnBullet.localScale = new Vector3 (1, 1, 1);
		else
			trnBullet.localScale = new Vector3 (-1, 1, 1);

		BulletData bData = new BulletData ();
		bData.shooter = gameObject;
		bData.targetLayer = layerHostile;
		bData.damage = damage;
		bData.speed = bulletSpeed;

		bulletPool.CallObj (trnBullet, bData);
	}
}
