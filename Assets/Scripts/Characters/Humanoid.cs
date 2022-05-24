using UnityEngine;
using System.Collections;

public class Humanoid : Character {

	[Header("Status")]
	public float runSpeed = 5f;
	public float crawlSpeed = 1f;
	public float backSpeed = 3f;
	public float jumpPower = 7f;

	// Targeting
	protected float degreeToTarget;
	protected Vector3 vectorToTarget;

	[Header ("Ground Check")]
	public LayerMask layerGround;
	[HideInInspector]public Vector2 roofCheckOffset;	// (Local space position) This is only set at this moment.
	protected bool isGround = true;		// Ground check
	protected bool isCeiling = false;		// Roof check

	[Header("Attack")]
	public LayerMask layerHostile;
	public AudioClip audioAttack;

	// Input
	protected bool isGettingInput = true;

	// Components
	protected Rigidbody2D rb;
	protected Animator anim;
	protected AudioSource audioS;
	protected BoxCollider2D colBox;
	protected CircleCollider2D colCir;
	protected Transform flipTrn;
	protected Transform aimPivot;

	// Animator hash
	protected AnimatorStateInfo baseBodyStateInfo;
	static protected int animHash_Base_Ground = Animator.StringToHash ("Base Layer.Ground");
	static protected int animHash_Base_Airial = Animator.StringToHash ("Base Layer.Airial");



	// Use this for initialization
	override protected void Awake () {
		base.Awake ();

		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		audioS = GetComponent<AudioSource> ();
		colBox = GetComponent<BoxCollider2D> ();
		colCir = GetComponent<CircleCollider2D> ();

		flipTrn = transform.FindChild ("Sprites");
		aimPivot = transform.FindChild ("Aim Pivot");

		roofCheckOffset = colBox.offset + Vector2.up * colBox.size.y * 0.5f;
	}



	// Update is called once per frame
	override protected void FixedUpdate () {
		base.FixedUpdate ();

		// Life Check
		if (hp > 0) {
			baseBodyStateInfo = anim.GetCurrentAnimatorStateInfo (anim.GetLayerIndex ("Base Layer"));

			// Ground Check
			isGround = Physics2D.OverlapCircle (transform.TransformPoint (colCir.offset) + Vector3.down * 0.01f, colCir.radius, layerGround);
			isCeiling = Physics2D.OverlapCircle (transform.TransformPoint (roofCheckOffset), colCir.radius, layerGround);

			// isGround parameter
			anim.SetBool ("isGround", isGround);

			// Move
			Move ();

			// Jump : There shall be ground below and no ceiling above for jumping.
			if (isGround && !isCeiling && axisVer > 0)
				Jump (jumpPower);

			// Sit : You can stand up without a ceiling.
			if (!isCeiling)
				anim.SetFloat ("crouch", -axisVer);
		}
	}



	override protected void Die () {
		anim.Stop ();
		if (trnAfterDie != null) {
			trnAfterDie.gameObject.SetActive (true);
			trnAfterDie.parent = null;
		}
		gameObject.SetActive (false);
	}



	protected void Move () {
		float moveSpeed = 0f;

		// Animation parameter
		if ((axisHor > 0 && !isFacingRight) || (axisHor < 0 && isFacingRight)) {
			// Move input != Character direction
			moveSpeed = Mathf.Lerp (backSpeed, crawlSpeed, -axisVer);
			anim.SetFloat ("move", -Mathf.Abs (axisHor));
		} else if ((axisHor > 0 && isFacingRight) || (axisHor < 0 && !isFacingRight) || axisHor == 0) {
			// Move input == Character direction
			moveSpeed = Mathf.Lerp (runSpeed, crawlSpeed, -axisVer);
			anim.SetFloat ("move", Mathf.Abs (axisHor));
		}
		// Actual moving
		rb.velocity = new Vector2 (axisHor * moveSpeed, rb.velocity.y);
	}



	protected void Jump (float jumpPower) {
		rb.velocity = new Vector2 (rb.velocity.x, jumpPower);
	}



	public new void Revive () {
		// Status reset
		hp = orgHp;
	}



	// Input axis
	public void SetInputMove (float horizontal, float vertical) {
		if (!isGettingInput)
			return;
		
		axisHor = horizontal;
		axisVer = vertical;
	}
	public void SetInputFire1 (bool fire) {
		if (!isGettingInput)
			return;
		
		axisFire1 = fire;
	}
	public void SetInputFire2 (bool fire) {
		if (!isGettingInput)
			return;
		
		axisFire2 = fire;
	}
	public void SetInputAngle (Vector3 targetPos) {	// This codes find Mouse cursor. Player uses this, but Other use new codes.
		if (!isGettingInput)
			return;

		float dx, dy;

		// XY Length to target -> Degree to target
		dx = targetPos.x - aimPivot.position.x;
		dy = targetPos.y - aimPivot.position.y;
		degreeToTarget = Mathf.Atan2 (dy, dx) * Mathf.Rad2Deg;		// Angle from chracter's arm to mouse cursor.
		vectorToTarget = new Vector3 (dx, dy, 0f).normalized;

		// Not Equal character's direction and target postion -> FLIP
		if (isFacingRight && Mathf.Abs (degreeToTarget) > 95f)
			Flip ();	// Facing right And Target is left
		else if (!isFacingRight && Mathf.Abs (degreeToTarget) < 85f)
			Flip ();	// Facing left And Target is right

		// Animation parameter
		if (isFacingRight) {	// degreeToTarget : -90 ~ 90, anim.SetFloat : 0 ~ 1
			anim.SetFloat ("aimAngle", (degreeToTarget + 90f) / 180f);
		} else {
			if (degreeToTarget > 0) {	// degreeToTarget : 90 ~ 180
				anim.SetFloat ("aimAngle", 90f / degreeToTarget);
			} else {					// degreeToTarget : -90 ~ -180
				anim.SetFloat ("aimAngle", -(degreeToTarget + 90f) / 180f);
			}
		}
	}
}
