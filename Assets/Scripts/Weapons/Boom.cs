using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boom : MonoBehaviour {

	[Header ("Status")]
	public float damage = 50f;
	public float radius = 1f;
	public float force = 3000f;

	ParticleSystem ps;

	// Use this for initialization
	void Awake () {
		ps = GetComponent<ParticleSystem> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(ps.isStopped)
			gameObject.SetActive (false);
	}

	// Use this for initialization
	void OnEnable () {
		// Call colliders or characters in explosion range.
		Collider2D[] colsInExplosion = Physics2D.OverlapCircleAll (transform.position, radius);
		List<Character> charsInExplosion = new List<Character> ();

		foreach (Collider2D col in colsInExplosion) {
			// Physics force
			if (col.attachedRigidbody != null) {
				Vector3 colDir = (col.transform.position - transform.position).normalized;
				col.attachedRigidbody.AddForce (colDir * force);
			}

			// Damaged charcters list
			Character colChar = col.GetComponent<Character> ();
			if (colChar != null) {
				// No Repeat -> Add to list
				if (!charsInExplosion.Contains (colChar))
					charsInExplosion.Add (colChar);
			}
		}

		// Damage
		for (int i = 0; i < charsInExplosion.Count; i++) {
			charsInExplosion [i].SetHp (-damage);
		}
	}
}
