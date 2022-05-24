using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	
	Camera _cam;

	public GameObject target;

	bool isShaking = false;
	public bool fixX, fixY, fixZ;
	public float xLimitMin = -Mathf.Infinity, xLimitMax = Mathf.Infinity;
	public float yLimitMin = -Mathf.Infinity, yLimitMax = Mathf.Infinity;
	public float zLimitMin = -Mathf.Infinity, zLimitMax = Mathf.Infinity;
	public float shakeTime = 0.5f, shakePower = 1;

	Vector3 chasedPosition;
	float drag;
	float initX, initY;

	void Awake () {
		_cam = GetComponent<Camera> ();

		chasedPosition.z = transform.position.z;

		initX = transform.position.x;
		initY = transform.position.y;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (target != null) {

			Vector3 mPosition = Input.mousePosition;
			mPosition.z = target.transform.position.z - _cam.transform.position.z;

			if (!fixX) {
				chasedPosition.x = target.transform.position.x;
			}

			if (!fixY) {
				chasedPosition.y = target.transform.position.y;
			}

			if (!fixZ) {
				drag += Input.GetAxis ("Mouse ScrollWheel");
				if (drag > 0.95f)
					drag = 0.95f;
				else if (drag < -0.95f)
					drag = -0.95f;
			}

			//Camera Position Setting.
			if (isShaking) {	//CAMERA SHAKE !!
				transform.localPosition += Random.insideUnitSphere * shakePower;
			}

			Vector3 finalPosition = Vector3.Lerp (chasedPosition, _cam.ScreenToWorldPoint (mPosition), drag);

			//XY Limit
			if (finalPosition.x < xLimitMin)
				finalPosition.x = xLimitMin;
			if (finalPosition.x > xLimitMax)
				finalPosition.x = xLimitMax;

			if (finalPosition.y < yLimitMin)
				finalPosition.y = yLimitMin;
			if (finalPosition.y > yLimitMax)
				finalPosition.y = yLimitMax;

			if (finalPosition.z < zLimitMin)
				finalPosition.z = zLimitMin;
			if (finalPosition.z > zLimitMax)
				finalPosition.z = zLimitMax;

			if (fixX && !fixY)
				transform.position = new Vector3 (initX, finalPosition.y, finalPosition.z);
			else if (!fixX && fixY)
				transform.position = new Vector3 (finalPosition.x, initY, finalPosition.z);
			else if (!fixX && !fixY)
				transform.position = finalPosition;
		}
	}



	public void CameraShake() {
		StartCoroutine (Shake ());
	}
	IEnumerator Shake () {	//This just controls bool value "isShaking".
		float shakingTime = 0;

		while (shakingTime < shakeTime) {
			if(isShaking == false)
				isShaking = true;
			shakingTime += Time.deltaTime;
			yield return null;
		}
		if(isShaking == true)
			isShaking = false;
	}
}
