using UnityEngine;
using System.Collections;

public class SetActiveAfter : MonoBehaviour {

	public float lifeTime = 5f;
	float curLifeTime = 0f;

	void OnEnable () {
		curLifeTime = 0;
	}
	
	void FixedUpdate () {
		// Lifetime
		if (curLifeTime >= lifeTime) {
			ActiveOff ();
		} else
			curLifeTime += Time.deltaTime;
	}

	void ActiveOff () {
		// Off -> Lifetime reset.
		curLifeTime = 0;
		gameObject.SetActive (false);
	}
}
