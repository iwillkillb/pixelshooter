using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : ObjectPool {

	public void CallObj (Transform trn, BulletData bulletData) {
		// FILO
		for (int a = 0; a < objList.Count; a++) {
			if (!objList [a].activeInHierarchy) {
				// Transform reset
				objList [a].GetComponent<Bullet> ().bData = bulletData;
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
