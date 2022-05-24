using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Humanoid humanoid;
	Camera chasingAngleCam;



	void Awake () {
		humanoid = GetComponent<Humanoid> ();
		chasingAngleCam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
	}

	void FixedUpdate () {
		// Take Input
		humanoid.SetInputMove (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		humanoid.SetInputFire1 (Input.GetButton ("Fire1"));
		humanoid.SetInputFire2 (Input.GetButton ("Fire2"));

		// Aim : Find mouse position.
		Vector3 mPos = Input.mousePosition;
		mPos.z = humanoid.transform.position.z - chasingAngleCam.transform.position.z;
		humanoid.SetInputAngle (chasingAngleCam.ScreenToWorldPoint (mPos));
	}
}
