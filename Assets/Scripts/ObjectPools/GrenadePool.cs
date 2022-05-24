using UnityEngine;
using System.Collections;

public class GrenadePool : ObjectPool {

	public void CallObj (GameObject thrower, Vector3 throwDir, Transform trn) {
		// FILO
		for (int a = 0; a < objList.Count; a++) {
			if (!objList [a].activeInHierarchy) {
				// throw setting
				objList [a].GetComponent<Grenade> ().thrower = thrower;
				objList [a].GetComponent<Grenade> ().throwDir = throwDir;

				// Transform reset
				objList [a].transform.position = trn.position;
				objList [a].transform.rotation = trn.rotation;
				objList [a].transform.localScale = trn.localScale;

				// On
				objList [a].SetActive (true);
				break;
			}
		}
	}
}
