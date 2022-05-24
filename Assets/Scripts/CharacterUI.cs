using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour {

	TextMesh textMesh;
	string textInit;



	void Awake () {
		textMesh = GetComponent<TextMesh> ();
		textInit = textMesh.text;
	}

	public void ChangeText (string addText) {
		textMesh.text = textInit + "\n" + addText;
	}
}
