using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class Action : MonoBehaviour {
	
	[Range (0,1)]
	public float activation = 1f;

	protected Character character;



	void Awake () {
		character = GetComponent<Character> ();

		Init ();
	}

	void FixedUpdate () {
		Effect ();
	}

	protected virtual void Init () {

	}

	protected virtual void Effect () {

	}
}
