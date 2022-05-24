using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {

	[Header ("Map and Object")]
	public Texture2D map;
	public GameObject block;

	[Header ("Block Sprites")]
	public Sprite sprTopLeft;
	public Sprite sprTopCenter;
	public Sprite sprTopRight;

	public Sprite sprMiddleLeft;
	public Sprite sprMiddleCenter;
	public Sprite sprMiddleRight;

	public Sprite sprBottomLeft;
	public Sprite sprBottomCenter;
	public Sprite sprBottomRight;

	public Sprite sprAloneTop;
	public Sprite sprAloneMiddle;
	public Sprite sprAloneBottom;
	public Sprite sprAloneLeft;
	public Sprite sprAloneCenter;
	public Sprite sprAloneRight;
	public Sprite sprAloneMiddleCenter;

	Vector2 objSize = Vector2.one;




	// Use this for initialization
	void Start () {
		bool isThereBlockTopLeft = true;
		bool isThereBlockTopCenter = true;
		bool isThereBlockTopRight = true;

		bool isThereBlockBottomLeft = true;
		bool isThereBlockBottomCenter = true;
		bool isThereBlockBottomRight = true;

		bool isThereBlockMiddleLeft = true;
		bool isThereBlockMiddleRight = true;

		for (int i = 0; i < map.width; i++) {
			for (int j = 0; j < map.height; j++) {
				// Generating sequence :
				// 20 21 22    1. Bottom -> Top
				// 10 11 12    2. Left -> Right
				// 00 01 02

				// Values initialization
				isThereBlockTopLeft = true;
				isThereBlockTopCenter = true;
				isThereBlockTopRight = true;

				isThereBlockMiddleLeft = true;
				isThereBlockMiddleRight = true;

				isThereBlockBottomLeft = true;
				isThereBlockBottomCenter = true;
				isThereBlockBottomRight = true;

				// Is there white pixel? -> Make block here!
				if (IsThereBlock (i, j)) {

					if (j == 0) {						// Bottom
						isThereBlockBottomLeft = false;
						isThereBlockBottomCenter = false;
						isThereBlockBottomRight = false;
						if (i == 0) {						// Left
							// X ? ?
							// X O ?
							// X X X
							isThereBlockTopLeft = false;
							isThereBlockMiddleLeft = false;
							isThereBlockTopCenter = IsThereBlock (i, j + 1);
							isThereBlockMiddleRight = IsThereBlock (i + 1, j);
							isThereBlockTopRight = IsThereBlock (i + 1, j + 1);

						} else if (i == map.width - 1) {	// Right
							// ? ? X
							// ? O X
							// X X X
							isThereBlockTopRight = false;
							isThereBlockMiddleRight = false;
							isThereBlockTopCenter = IsThereBlock (i, j + 1);
							isThereBlockMiddleLeft = IsThereBlock (i - 1, j);
							isThereBlockTopLeft = IsThereBlock (i - 1, j + 1);

						} else {							// Center
							// ? ? ?
							// ? O ?
							// X X X
							isThereBlockTopLeft = IsThereBlock (i - 1, j + 1);
							isThereBlockTopCenter = IsThereBlock (i, j + 1);
							isThereBlockTopRight = IsThereBlock (i + 1, j + 1);
							isThereBlockMiddleLeft = IsThereBlock (i - 1, j);
							isThereBlockMiddleRight = IsThereBlock (i + 1, j);

						}

					} else if (j == map.height - 1) {	// Top
						isThereBlockTopLeft = false;
						isThereBlockTopCenter = false;
						isThereBlockTopRight = false;
						if (i == 0) {						// Left
							// X X X
							// X O ?
							// X ? ?
							isThereBlockMiddleLeft = false;
							isThereBlockBottomLeft = false;
							isThereBlockBottomCenter = IsThereBlock (i, j - 1);
							isThereBlockMiddleRight = IsThereBlock (i + 1, j);
							isThereBlockBottomRight = IsThereBlock (i + 1, j - 1);

						} else if (i == map.width - 1) {	// Right
							// X X X
							// ? O X
							// ? ? X
							isThereBlockMiddleRight = false;
							isThereBlockBottomRight = false;
							isThereBlockBottomCenter = IsThereBlock (i, j - 1);
							isThereBlockMiddleLeft = IsThereBlock (i - 1, j);
							isThereBlockBottomRight = IsThereBlock (i + 1, j - 1);

						} else {							// Center
							// X X X
							// ? O ?
							// ? ? ?
							isThereBlockMiddleLeft = IsThereBlock (i - 1, j);
							isThereBlockMiddleRight = IsThereBlock (i + 1, j);
							isThereBlockBottomLeft = IsThereBlock (i - 1, j - 1);
							isThereBlockBottomCenter = IsThereBlock (i, j - 1);
							isThereBlockBottomRight = IsThereBlock (i + 1, j - 1);

						}

					} else {							// Middle
						if (i == 0) {						// Left
							// X ? ?
							// X O ?
							// X ? ?
							isThereBlockTopLeft = false;
							isThereBlockMiddleLeft = false;
							isThereBlockBottomLeft = false;
							isThereBlockTopCenter = IsThereBlock (i, j + 1);
							isThereBlockBottomCenter = IsThereBlock (i, j - 1);
							isThereBlockTopRight = IsThereBlock (i + 1, j + 1);
							isThereBlockMiddleRight = IsThereBlock (i + 1, j);
							isThereBlockBottomRight = IsThereBlock (i + 1, j - 1);

						} else if (i == map.width - 1) {	// Right
							// ? ? X
							// ? O X
							// ? ? X
							isThereBlockTopRight = false;
							isThereBlockMiddleRight = false;
							isThereBlockBottomRight = false;
							isThereBlockTopCenter = IsThereBlock (i, j + 1);
							isThereBlockBottomCenter = IsThereBlock (i, j - 1);
							isThereBlockTopLeft = IsThereBlock (i - 1, j + 1);
							isThereBlockMiddleLeft = IsThereBlock (i - 1, j);
							isThereBlockBottomLeft = IsThereBlock (i - 1, j - 1);

						} else {							// Center
							// ? ? ?
							// ? O ?
							// ? ? ?
							isThereBlockTopLeft = IsThereBlock (i - 1, j + 1);
							isThereBlockMiddleLeft = IsThereBlock (i - 1, j);
							isThereBlockBottomLeft = IsThereBlock (i - 1, j - 1);
							isThereBlockTopCenter = IsThereBlock (i, j + 1);
							isThereBlockBottomCenter = IsThereBlock (i, j - 1);
							isThereBlockTopRight = IsThereBlock (i + 1, j + 1);
							isThereBlockMiddleRight = IsThereBlock (i + 1, j);
							isThereBlockBottomRight = IsThereBlock (i + 1, j - 1);

						}
					}

					// Bool values -> Sprite select
					Sprite sprSelected;
					// Top
					if (!isThereBlockTopCenter && isThereBlockBottomCenter && isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprTopCenter;
					else if (!isThereBlockTopCenter && isThereBlockBottomCenter && !isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprTopLeft;
					else if (!isThereBlockTopCenter && isThereBlockBottomCenter && isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprTopRight;
					// Middle
					else if (isThereBlockTopCenter && isThereBlockBottomCenter && isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprMiddleCenter;
					else if (isThereBlockTopCenter && isThereBlockBottomCenter && !isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprMiddleLeft;
					else if (isThereBlockTopCenter && isThereBlockBottomCenter && isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprMiddleRight;
					// Bottom
					else if (isThereBlockTopCenter && !isThereBlockBottomCenter && isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprBottomCenter;
					else if (isThereBlockTopCenter && !isThereBlockBottomCenter && !isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprBottomLeft;
					else if (isThereBlockTopCenter && !isThereBlockBottomCenter && isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprBottomRight;
					// Alone
					else if (!isThereBlockTopCenter && isThereBlockBottomCenter && !isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprAloneTop;
					else if (isThereBlockTopCenter && isThereBlockBottomCenter && !isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprAloneMiddle;
					else if (isThereBlockTopCenter && !isThereBlockBottomCenter && !isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprAloneBottom;
					else if (!isThereBlockTopCenter && !isThereBlockBottomCenter && !isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprAloneLeft;
					else if (!isThereBlockTopCenter && !isThereBlockBottomCenter && isThereBlockMiddleLeft && isThereBlockMiddleRight)
						sprSelected = sprAloneCenter;
					else if (!isThereBlockTopCenter && !isThereBlockBottomCenter && isThereBlockMiddleLeft && !isThereBlockMiddleRight)
						sprSelected = sprAloneRight;
					else if (isThereBlockTopLeft && isThereBlockTopRight && isThereBlockBottomLeft && isThereBlockBottomRight)
						sprSelected = sprAloneMiddleCenter;
					else
						sprSelected = sprAloneMiddleCenter;

					// Make block object and set block's sprite.
					Vector3 startPos = new Vector3 (objSize.x * map.width, objSize.y * map.height, 0) * 0.5f - new Vector3 (objSize.x, objSize.y, 0) * 0.5f;
					GameObject objBlock = Instantiate (block, transform.position - startPos + new Vector3 (i, j, 0), Quaternion.identity, transform) as GameObject;
					objBlock.GetComponent<SpriteRenderer> ().sprite = sprSelected;
				}
			}
		}
	}

	bool IsThereBlock (int x, int y) {
		Color gettedPixel = map.GetPixel (x, y);
		if (gettedPixel.grayscale == 1f)	// Is this white pixel?
			return true;
		else
			return false;
	}
}
