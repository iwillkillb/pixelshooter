using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class InputPlayer : MonoBehaviour {

	Character character;

	// Aiming Only
	ActionAiming aa;
	Camera chasingAngleCam;



	void Awake () {
		character = GetComponent<Character> ();

		aa = GetComponent<ActionAiming> ();
		chasingAngleCam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
	}

	void FixedUpdate () {
		// Take Input
		character.axisHor = Input.GetAxis ("Horizontal");
		character.axisVer = Input.GetAxis ("Vertical");
		character.axisHorDown = Input.GetButtonDown ("Horizontal");
		character.axisVerDown = Input.GetButtonDown ("Vertical");
		character.axisFire0 = Input.GetButton ("Fire1");
		character.axisFire1 = Input.GetButton ("Fire2");

		if (aa != null) {
			Vector3 mPos = Input.mousePosition;
			mPos.z = transform.position.z - chasingAngleCam.transform.position.z;
			aa.SetAngle (chasingAngleCam.ScreenToWorldPoint (mPos));
		}
	}
}
