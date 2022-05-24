using UnityEngine;
using System.Collections;

[System.Serializable]
public class Frame {
	public Sprite sprite;
	public Color color = Color.white;
}

public class FrameAnimation : MonoBehaviour {

	// Lifetime
	[Header ("Setting")]
	public bool isLoop = false;
	public float lifeTime = 1f;
	float curLifeTime = 0f;

	public Frame[] frames;

	// Initialization
	Sprite initSprite;
	Color initColor;

	// Components
	SpriteRenderer sr;



	// Use this for initialization
	void Awake () {
		sr = GetComponent<SpriteRenderer> ();

		initSprite = sr.sprite;
		initColor = sr.color;
	}

	// Reset
	void OnEnable () {
		curLifeTime = 0;

		sr.sprite = initSprite;
		sr.color = initColor;
	}

	void FixedUpdate () {
		// Lifetime
		if (curLifeTime >= lifeTime) {
			if (isLoop)
				curLifeTime = 0f;
			else
				gameObject.SetActive (false);	// Off
		} else
			curLifeTime += Time.deltaTime;

		float lifeTimeNormalized = curLifeTime / lifeTime;
		float lifeTimeStandard = 1f / (float)(frames.Length);
		int currentIndex = Mathf.FloorToInt (lifeTimeNormalized / lifeTimeStandard);
		// Example----------If you have 4 frames...-----------------
		// init : Object's Transform and Sprite Renderer data
		// Frame's index :		0			1			2			3		
		// lifeTimeNormalized :	0.00~0.25	0.25~0.50	0.50~0.75	0.75~1.00
		// lifeTimeStandard = 0.25 = 1f / (float)(frames.Length = 4)
		// ---------------------------------------------------------
		float tempColor = lifeTimeNormalized / lifeTimeStandard - (int)(lifeTimeNormalized / lifeTimeStandard);
		if (currentIndex < frames.Length - 1) {
			sr.sprite = frames [currentIndex].sprite;
			sr.color = Color.Lerp (frames [currentIndex].color, frames [currentIndex + 1].color, tempColor);
		} else if (currentIndex == frames.Length - 1) {
			sr.sprite = frames [currentIndex].sprite;
			if (isLoop)
				sr.color = Color.Lerp (frames [currentIndex].color, frames [0].color, tempColor);
			else
				sr.color = frames [currentIndex].color;
		} else {
			return;	// No Out of array index
		}
	}
}
