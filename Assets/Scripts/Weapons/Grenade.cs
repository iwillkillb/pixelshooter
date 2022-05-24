using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour {
	
	public float delay = 5f;

	float curDelay = 0f;

	// These variables are specified externally.
	[HideInInspector]public GameObject thrower; // Who shot this bullet?
	[HideInInspector]public Vector3 throwDir;

	Rigidbody2D rb;
	ObjectPool boomEffectPool;



	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		boomEffectPool = GameObject.Find ("Boom Effects").GetComponent<ObjectPool> ();
	}

	// Add Physic moving
	void OnEnable () {
		rb.velocity = throwDir;
		rb.AddTorque (throwDir.magnitude);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Lifetime
		if (curDelay >= delay) {
			Boom ();
		} else
			curDelay += Time.deltaTime;
	}

	void Boom () {
		// Reset LifeTime
		curDelay = 0f;

		// Effect
		Transform trnBoomEffect = transform;
		trnBoomEffect.rotation = Quaternion.identity;
		trnBoomEffect.localScale = Vector3.one;
		boomEffectPool.CallObj (trnBoomEffect);

		gameObject.SetActive (false);
	}
}
