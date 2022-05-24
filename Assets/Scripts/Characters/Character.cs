using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Character : MonoBehaviour {

	[Header("Status")]
	public float orgHp = 100f;
	protected float hp;
	public float damage = 10f;
	public float attackDelay = 0.1f;
	[HideInInspector]public float curAttackDelay = 0f;

	[Header("Spawned object after die")]
	public Transform trnAfterDie;

	[Header ("Terrain check")]
	public CheckTerrain ctTop;
	public CheckTerrain ctBottom;

	[Header ("Flip")]
	public Transform flipTransform;
	protected bool isFacingRight = true;

	// Input
	public float axisHor { get; set; }
	public float axisVer { get; set; }
	public bool axisHorDown { get; set; }
	public bool axisVerDown { get; set; }
	public bool axisFire0 { get; set; }
	public bool axisFire1 { get; set; }
	public bool axisFire2 { get; set; }
	public bool axisFire3 { get; set; }
	public bool axisFire4 { get; set; }
	public float axisAngle { get; set; }

	// Components
	CharacterUI characterUI;



	public UnityEvent myUnityEvent;

	// Use this for initialization
	virtual protected void Awake () {
		characterUI = GetComponentInChildren<CharacterUI> ();
		hp = orgHp;

		if (myUnityEvent == null)
			myUnityEvent = new UnityEvent ();
		myUnityEvent.AddListener (Ping);
	}

	void Ping () {
		Debug.Log ("Ping");
	}


	
	// Update is called once per frame
	virtual protected void FixedUpdate () {
		// UI HP
		if (characterUI != null)
			characterUI.ChangeText ("HP : " + Mathf.RoundToInt (hp));

		// Life Check
		if (hp <= 0)
			Die ();

		// Delay
		if (curAttackDelay < attackDelay)
			curAttackDelay += Time.deltaTime;

		// Debug
		if (Input.GetKeyDown (KeyCode.P)) {
			myUnityEvent.Invoke ();
			hp = 0;
		}
	}



	protected void OnEnable () {
		Revive ();
	}



	virtual protected void Die () {
		if (trnAfterDie != null) {
			trnAfterDie.gameObject.SetActive (true);
			trnAfterDie.parent = null;
		}
		
		gameObject.SetActive (false);
	}



	public void Revive () {
		// Status reset
		hp = orgHp;

		if (trnAfterDie != null) {
			// Sync character position and trnAfterDie position
			transform.position = trnAfterDie.position;

			// trnAfterDie reset : Parent, transform, active
			trnAfterDie.parent = transform;
			trnAfterDie.localPosition = Vector3.zero;
			trnAfterDie.localRotation = Quaternion.identity;
			trnAfterDie.localScale = Vector3.one;
			trnAfterDie.gameObject.SetActive (false);
		}
	}
	public float GetHp () {
		return hp;
	}
	public void SetHp (float changeValue) {
		hp += changeValue;
	}




	public void Flip () {
		if (flipTransform == null)
			return;

		Vector3 curLocalScale = flipTransform.localScale;
		curLocalScale.x *= -1;
		flipTransform.localScale = curLocalScale;

		if ((flipTransform.localScale.x > 0 && !isFacingRight) || (flipTransform.localScale.x < 0 && isFacingRight)) {
			isFacingRight = !isFacingRight;
		}
	}
	public bool IsFacingRight () {
		return isFacingRight;
	}
}
