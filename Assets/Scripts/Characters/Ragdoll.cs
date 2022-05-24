using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour {
	
	public GameObject characterBody;	// Character's animated body (Sprites)
	Transform[] jointsCharacter;		// characterBody's joints
	Transform[] jointsRagdoll;			// Ragdoll's joints

	JointAngleLimits2D setLimit;
	float limitMin, limitMax;
	bool isFacingRight = true;

	void Awake () {
		jointsRagdoll = GetComponentsInChildren<Transform> ();
		jointsCharacter = characterBody.GetComponentsInChildren<Transform> ();
	}

	// Die
	void OnEnable () {
		PoseSync ();

		transform.SetParent (null);
	}

	void PoseSync () {
		// Transform sync
		transform.position = characterBody.transform.position;
		transform.rotation = characterBody.transform.rotation;
		transform.localScale = characterBody.transform.localScale;

		bool needJointFlip = false;
		if ((transform.localScale.x < 0 && isFacingRight) || (transform.localScale.x > 0 && !isFacingRight)) {
			isFacingRight = !isFacingRight;
			needJointFlip = true;
		}

		foreach (Transform jointR in jointsRagdoll) {
			// Facing left -> Joint limit swap
			HingeJoint2D hinge = jointR.GetComponent<HingeJoint2D> ();
			if (hinge != null && needJointFlip) {
				limitMax = hinge.limits.max;
				limitMin = hinge.limits.min;
				setLimit.max = -limitMin;
				setLimit.min = -limitMax;
				hinge.limits = setLimit;
			}

			foreach (Transform jointC in jointsCharacter) {
				if (jointR.name == jointC.name) {
					// Part transform sync
					jointR.transform.localPosition = jointC.transform.localPosition;
					jointR.transform.localRotation = jointC.transform.localRotation;
					jointR.transform.localScale = jointC.transform.localScale;
					break;
				}
			}
		}
	}
}
