using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO; 

enum PulseType {NONE, IN, OUT, TOTAL };

struct WordType
{
	public string word;
	public bool isUsed;
};

public class TextMaster : MonoBehaviour {

	List<GameObject> createdWords;
	List<WordType> possibleWords;
	public GameObject targetedWord;

	public Font typeFont;
	public int fontSize = 20;
	public float pulseSpeed = 0.5f;
	// Use this for initialization
	void Start () {
		createdWords = new List<GameObject> ();
		possibleWords = new List<WordType> ();
		LoadWords ("Assets/Resources/WordLists/BeginnerWordList.txt");
		typeFont = (Font)Resources.Load("Fonts/unispace");
		createdWords.Add(CreateMesh(possibleWords[Random.Range (0, possibleWords.Count)].word));
		targetedWord = createdWords[0];
	}

	void LoadWords(string fileName) {

		string line;
		StreamReader theReader = new StreamReader(fileName, Encoding.Default);
		using (theReader) {
			do {
				line = theReader.ReadLine ();
					
				if (line != null) {
					WordType addWord = new WordType ();
					addWord.isUsed = false;
					addWord.word = line;
					possibleWords.Add (addWord);
				}
			} while (line != null);

			theReader.Close();
		}
		
	}

	GameObject CreateMesh(string targetWord)
	{
		GameObject textMesh = new GameObject (targetWord);
		textMesh.AddComponent<TypeWindow> ();
		textMesh.GetComponent<TypeWindow>().CreateWord (targetWord, fontSize, typeFont);
		textMesh.transform.parent = transform;
		return textMesh;
		
	}

	//Take all null objects out


	// Update is called once per frame
	void Update () {
		if (!targetedWord.GetComponent<TypeWindow>().isDead && !targetedWord.name.Equals ("") 
		    && Input.inputString.Length > 0 && 
		    Input.inputString.Contains(targetedWord.name[0].ToString()))
			targetedWord.GetComponent<TypeWindow>().CutMesh (Time.time);
		if(!targetedWord.GetComponent<TypeWindow>().isDead && !targetedWord.name.Equals ("")) targetedWord.GetComponent<TypeWindow>().PulseMesh (pulseSpeed);

		if (targetedWord.GetComponent<TypeWindow>().isDead) {
			Destroy(targetedWord);
			createdWords.Clear();
			createdWords.Add(CreateMesh(possibleWords[Random.Range (0, possibleWords.Count)].word));
			targetedWord = createdWords[0];
			targetedWord.GetComponent<TypeWindow>().isDead = false;
			targetedWord.GetComponent<TypeWindow>().currentFade = FadeType.FADEIN;
		}
	}


}
