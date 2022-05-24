using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class ActionJump : Action {

	[Header ("Jump")]
	public float jumpPower = 10f;
	public int jumpVariable = 1;
	public int jumpCount = 0;

	Rigidbody2D rb;

	protected override void Init ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	protected override void Effect ()
	{
		/*
		if (character.ctBottom.IsTerrain () && jumpCount != 0)
			jumpCount = 0;	//Grounded, Not jump -> 0
		if (jumpCount == 0 && !character.ctBottom.IsTerrain () && !character.ctTop.IsTerrain ())	//End of First Input -> You can Second Input
			jumpCount = 1;	//Grounded -> jump -> 1
		if (Input.GetButtonDown ("Vertical") && Input.GetAxisRaw ("Vertical") > 0) {	//Double jump
			if (jumpCount < 2) {
				if (character.ctBottom.IsTerrain ()) {	//First Input, Key:Up
					rb.velocity = new Vector2 (rb.velocity.x, jumpPower * activation);
				}
				if (jumpCount == 1) {	//Second Input, KeyDown:Up, Need SP
					rb.velocity = new Vector2 (rb.velocity.x, jumpPower);
					jumpCount = 2;
				}
			}
		}*/

		// Activated One frame
		if (character.axisVerDown) {

			if (jumpCount == 0) {	// Grounded
				if (character.ctBottom.IsTerrain () && !character.ctTop.IsTerrain ()) {
					rb.velocity = new Vector2 (rb.velocity.x, jumpPower * activation);
					jumpCount++;
				}
			} else {	// Airial
				if (jumpCount < jumpVariable) {
					rb.velocity = new Vector2 (rb.velocity.x, jumpPower * activation);
					jumpCount++;
				}
			}
		}
		// Reset jumpCount
		if (character.ctBottom.IsTerrain () || character.ctTop.IsTerrain ()) {
			if (jumpCount != 0)
				jumpCount = 0;
		}


		/*
		if ((character.axisVer > 0f) && character.ctBottom.IsTerrain () && !character.ctTop.IsTerrain ())
			rb.velocity = new Vector2 (rb.velocity.x, jumpPower * activation);*/
	}
}
