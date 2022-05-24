using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Soldier))]
public class SoldierNPC : MonoBehaviour {
	
	public enum actionState {Idle, Chase, Battle, Fallback};
	[HideInInspector]public actionState curState = actionState.Idle;

	[Header ("Reaction to Player")]
	public float rangeDetection = 10f;
	public float rangeBattle = 3f;
	public float rangeTooNearInBattle = 1f;
	public float hpRatioLimitToBattle = 0.3f;	// This character runs away if the hp is lower than this ratio.
	bool isInDetection, isInBattle, isInTooNearInBattle;
	Collider2D colDetection;		// Collider within detection range.
	Collider2D colBattle;			// Collider within battle range. It takes precedence over colDetection.
	Collider2D colTooNearInBattle;	// Collider within fallback range. It takes precedence over colDetection.

	[Header ("Reaction velocity")]
	public float rvMove = 1f;
	public float rvAim = 1f;

	// Blocking terrain check -> Input vertical axis.
	bool isBlockTop, isBlockBottom;

	// Input axis
	float curAxisHor, goalAxisHor;
	float curAxisVer;
	Vector3 curTargetPos, goalTargetPos;

	// Components
	Soldier soldier;
	BoxCollider2D colBox;
	Transform aimPivot;



	void Awake () {
		soldier = GetComponent<Soldier> ();
		colBox = GetComponent<BoxCollider2D> ();
		aimPivot = transform.FindChild ("Aim Pivot");
	}

	void FixedUpdate () {
		// Detect player
		colDetection = Physics2D.OverlapCircle (transform.position, rangeDetection, soldier.layerHostile);
		if (colDetection != null)
			isInDetection = true;
		else
			isInDetection = false;
		
		colBattle = Physics2D.OverlapCircle (transform.position, rangeBattle, soldier.layerHostile);
		if (colBattle != null)
			isInBattle = true;
		else
			isInBattle = false;
		
		colTooNearInBattle = Physics2D.OverlapCircle (transform.position, rangeTooNearInBattle, soldier.layerHostile);
		if (colTooNearInBattle != null)
			isInTooNearInBattle = true;
		else
			isInTooNearInBattle = false;



		// Select actionState
		if (isInDetection) {
			if (isInBattle) {
				if (isInTooNearInBattle) {
					if(curState != actionState.Fallback)
						curState = actionState.Fallback;
				} else {
					if (curState != actionState.Battle)
						curState = actionState.Battle;
				}
			} else {
				if (soldier.GetHp () < soldier.GetHp () * hpRatioLimitToBattle) {
					if(curState != actionState.Fallback)
						curState = actionState.Fallback;
				} else {
					if(curState != actionState.Chase)
						curState = actionState.Chase;
				}
			}
		} else {
			if(curState != actionState.Idle)
				curState = actionState.Idle;
		}



		// React to actionState
		switch (curState) {
		case actionState.Idle:
			goalAxisHor = 0f;
			break;

		case actionState.Chase:
			goalTargetPos = colDetection.transform.position;
			if (transform.position.x > colDetection.transform.position.x)
				goalAxisHor = -1f;
			else
				goalAxisHor = 1f;
			break;

		case actionState.Battle:
			goalTargetPos = colBattle.transform.position;
			goalAxisHor = 0f;
			break;

		case actionState.Fallback:
			goalTargetPos = colDetection.transform.position;
			if (transform.position.x > colDetection.transform.position.x)
				goalAxisHor = 1f;
			else
				goalAxisHor = -1f;
			break;
		}



		// Detect blocking terrain
		if (curAxisHor > 0) {
			isBlockTop = Physics2D.OverlapCircle (transform.TransformPoint (soldier.roofCheckOffset + Vector2.right * colBox.size.x * 0.5f), 0.1f, soldier.layerGround);
			isBlockBottom = Physics2D.OverlapCircle (transform.TransformPoint (soldier.roofCheckOffset + Vector2.down * colBox.size.y + Vector2.right * colBox.size.x * 0.5f), 0.1f, soldier.layerGround);
		} else if (curAxisHor < 0) {
			isBlockTop = Physics2D.OverlapCircle (transform.TransformPoint (soldier.roofCheckOffset + Vector2.left * colBox.size.x * 0.5f), 0.1f, soldier.layerGround);
			isBlockBottom = Physics2D.OverlapCircle (transform.TransformPoint (soldier.roofCheckOffset + Vector2.down * colBox.size.y + Vector2.left * colBox.size.x * 0.5f), 0.1f, soldier.layerGround);
		}



		// Input axis
		if (isBlockTop && !isBlockBottom)
			curAxisVer = -1f;
		else if (!isBlockTop && isBlockBottom)
			curAxisVer = 1f;
		else if (isBlockTop == isBlockBottom)
			curAxisVer = 0f;
		curAxisHor = Mathf.Lerp (curAxisHor, goalAxisHor, Time.deltaTime * rvMove);
		soldier.SetInputMove (curAxisHor, curAxisVer);

		curTargetPos = Vector3.Lerp (curTargetPos, goalTargetPos, Time.deltaTime * rvAim);
		soldier.SetInputAngle (curTargetPos);

		bool isAimTargetButBlocked = Physics2D.Linecast (aimPivot.position, curTargetPos, soldier.layerGround);
		if (curState == actionState.Battle || curState == actionState.Fallback)
			soldier.SetInputFire1 (!isAimTargetButBlocked);
		else
			soldier.SetInputFire1 (false);
	}
}
