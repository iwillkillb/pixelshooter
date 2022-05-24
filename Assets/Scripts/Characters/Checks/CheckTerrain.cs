using UnityEngine;
using System.Collections;

public class CheckTerrain : MonoBehaviour {
	
	public LayerMask layerOfTerrains;
	public float range = 0.1f;

	bool isTerrain;



	void FixedUpdate () {
		isTerrain = Physics2D.OverlapCircle (transform.position, range, layerOfTerrains);
	}

	public bool IsTerrain () {
		return isTerrain;
	}
}
