using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum PulseType {NONE, IN, OUT, TOTAL };
public class TextMaster : MonoBehaviour {

	List<GameObject> createdWords;
	List<string> possibleWords;
	public GameObject targetedWord;

	public Font typeFont;
	public int fontSize = 20;
	public float pulseSpeed = 0.03f;
	// Use this for initialization
	void Start () {
		createdWords = new List<GameObject> ();
		possibleWords = new List<string> ();
		LoadWords ();
		typeFont = (Font)Resources.Load("Fonts/unispace");


		createdWords.Add(CreateMesh(possibleWords[0]));
		targetedWord = createdWords[0];
	}

	void LoadWords() {
		possibleWords.Add ("Tell it like it is");
	}

	GameObject CreateMesh(string targetWord)
	{
		GameObject textMesh = new GameObject (targetWord);
		textMesh.AddComponent<TypeWindow> ();
		textMesh.GetComponent<TypeWindow>().CreateWord (targetWord, fontSize, typeFont);
		textMesh.transform.parent = transform;
		return textMesh;
		
	}

	// Update is called once per frame
	void Update () {
		if (targetedWord != null && Input.inputString.Length > 0 && 
		    Input.inputString.Contains(targetedWord.name[0].ToString()))
			targetedWord.GetComponent<TypeWindow>().CutMesh ();

		if(targetedWord != null) targetedWord.GetComponent<TypeWindow>().PulseMesh (pulseSpeed);
	}


}
