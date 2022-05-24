using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionAttack : Action {

	public int indexAttackButton = 0;

	[Header ("Attack")]
	public float damage = 10f;
	public float delay = 0.1f;
	public LayerMask layerHostile;
	public AudioClip audioAttack;

	float curDelay = 0f;

	protected Animator anim;
	protected AudioSource audioS;



	protected override void Init ()
	{
		anim = GetComponent<Animator> ();
		audioS = GetComponent<AudioSource> ();
	}

	protected override void Effect ()
	{
		// Delay fill
		if (curDelay < delay)
			curDelay += Time.deltaTime;

		bool axis;
		switch (indexAttackButton) {
		case 0:
			axis = character.axisFire0;
			break;

		case 1:
			axis = character.axisFire1;
			break;

		case 2:
			axis = character.axisFire2;
			break;

		case 3:
			axis = character.axisFire3;
			break;

		default:
			axis = character.axisFire4;
			break;
		}
		
		if (axis && curDelay >= delay) {
			curDelay = 0f;
			Attack ();

			if (audioS != null && audioAttack != null)
				audioS.PlayOneShot (audioAttack);
		}
	}

	protected virtual void Attack () {
		
	}
}
