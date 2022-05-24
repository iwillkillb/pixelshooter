using UnityEngine;
using System.Collections;

public class BulletData {
	public GameObject shooter;		// Who shot this bullet?
	public LayerMask targetLayer;	// targets
	public float damage = 1f;
	public float speed = 1f;
}

public class Bullet : MonoBehaviour {

	float curLifeTime = 0f;
	bool isCollide = false;
	Sprite sprInit;

	[Header ("Status")]
	public float lifeTime = 5f;
	public float force = 1000f;
	public Sprite sprAfterCollision;
	public LayerMask blockLayer; // The layers of objects this bullet can't pierce.

	// These variables are specified externally.
	[HideInInspector]public BulletData bData;

	[Header ("Sounds")]
	public AudioClip soundCollision;

	SpriteRenderer sr;
	AudioSource audioS;



	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
		audioS = GetComponent<AudioSource> ();

		bData = new BulletData ();
		sprInit = sr.sprite;
	}

	void FixedUpdate () {
		// Lifetime
		if (curLifeTime >= lifeTime) {
			ActiveOff ();
		} else
			curLifeTime += Time.deltaTime;

		// Move
		if (!isCollide)
			transform.Translate (Vector3.right * bData.speed * transform.localScale.x * Time.deltaTime);
	}

	// Bullet initialization.
	void OnEnable () {
		isCollide = false;
		sr.sprite = sprInit;
	}

	// Collision
	void OnTriggerEnter2D (Collider2D col) {
		// Don't shoot itself.
		if (col.gameObject == bData.shooter)
			return;

		if ((((1 << col.gameObject.layer) & blockLayer) != 0) || (((1 << col.gameObject.layer) & bData.targetLayer) != 0)) {
			isCollide = true;
			sr.sprite = sprAfterCollision;

			// Damage
			Character colChar = col.GetComponent<Character> ();
			if (colChar != null) {
				colChar.SetHp (-bData.damage);
			}

			// Force angle : Set by Bullet's rotation and Facing direction
			if (col.attachedRigidbody != null)
				col.attachedRigidbody.AddForce (transform.rotation * Vector2.right * transform.localScale.x * force, ForceMode2D.Impulse);
			
			audioS.PlayOneShot (soundCollision);
			Invoke ("ActiveOff", 0.1f);
		}
	}

	void ActiveOff () {
		// Off -> Lifetime reset.
		curLifeTime = 0;
		gameObject.SetActive (false);
	}
}
