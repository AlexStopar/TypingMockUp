using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO; 

enum PulseType {NONE, IN, OUT, TOTAL };

struct WordType
{
	public List<string> lines;
	public bool isUsed;
};

public class TextMaster : MonoBehaviour {

	List<GameObject> createdWords;
	List<WordType> possibleWords;
	public GameObject targetedWord;
	WordType currentWord;
	int currentLine;
	public Vector3 windowPosition = new Vector3(0.0f, -3.5f, 0.0f);

	public Font typeFont;
	public int characterLimit = 42; //For each line for each word
	public int fontSize = 20;
	public float pulseSpeed = 0.2f;
	// Use this for initialization
	void Start () {
		createdWords = new List<GameObject> ();
		possibleWords = new List<WordType> ();
		LoadWords ("Assets/Resources/WordLists/BeginnerWordList.txt");
		typeFont = (Font)Resources.Load("Fonts/unispace");
		currentWord = possibleWords[Random.Range (0, possibleWords.Count)];
		currentLine = 0;
		createdWords.Add(CreateMesh(currentWord.lines[currentLine]));
		targetedWord = createdWords[0];
		targetedWord.transform.localPosition = windowPosition;
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
					addWord.lines = new List<string>();
					if(line.Length <= characterLimit) addWord.lines.Add(line);
					else 
					{
						while(line.Length >= characterLimit)
						{
							int cutoffPoint = characterLimit;
							while(line[cutoffPoint] != ' ')
							{
								cutoffPoint--;
							}
							addWord.lines.Add(line.Substring(0, cutoffPoint));
							line = line.Substring(cutoffPoint + 1);
						}
						addWord.lines.Add(line);
					}
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
			if(currentLine >= currentWord.lines.Count - 1) 
			{
				currentWord = possibleWords[Random.Range (0, possibleWords.Count)];
				currentLine = 0;
			}
			else currentLine ++;
			createdWords.Add(CreateMesh(currentWord.lines[currentLine]));
			targetedWord = createdWords[0];
			targetedWord.transform.localPosition = windowPosition;
			targetedWord.GetComponent<TypeWindow>().isDead = false;
			targetedWord.GetComponent<TypeWindow>().currentFade = FadeType.FADEIN;
		}
	}


}
