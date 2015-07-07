using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum PulseType {NONE, IN, OUT, TOTAL };
public class TextMaster : MonoBehaviour {

	List<GameObject> createdWords;
	List<string> possibleWords;
	public GameObject targetedWord;
	Vector3 originalWordScale; //Scale for original word when pulsing
	public Font typeFont;
	public int fontSize = 20;
	public float pulseSpeed = 0.1f;
	PulseType currentPulse = PulseType.NONE;
	// Use this for initialization
	void Start () {
		createdWords = new List<GameObject> ();
		possibleWords = new List<string> ();
		LoadWords ();
		typeFont = (Font)Resources.Load("Fonts/unispace");


		createdWords.Add(CreateMesh(possibleWords[0]));
		targetedWord = createdWords[0];
		originalWordScale = targetedWord.transform.localScale;
	}

	void LoadWords() {
		possibleWords.Add ("Example");
	}

	GameObject CreateMesh(string targetWord)
	{
		GameObject textMesh = new GameObject (targetWord);
		textMesh.AddComponent<TextMesh>();
		MeshRenderer meshRender = textMesh.GetComponent<MeshRenderer>();
		meshRender.material = Resources.Load<Material>("Fonts/unispace");
		TextMesh mesh = textMesh.GetComponent<TextMesh>();
		mesh.text = targetWord;
		mesh.font = typeFont;
		mesh.fontSize = fontSize;
		mesh.color = Color.green;
		textMesh.transform.parent = transform;
		mesh.anchor = TextAnchor.MiddleCenter;
		return textMesh;
	}
	void CutMesh()
	{
		if (targetedWord.name.Length > 0) {
			targetedWord.name = targetedWord.name.Substring (1);
			targetedWord.GetComponent<TextMesh> ().text = 
				targetedWord.GetComponent<TextMesh> ().text.Substring (1);
		}
		if (targetedWord.name.Equals (""))
			GameObject.Destroy (targetedWord);
		else
			currentPulse = PulseType.IN;
	}
	// Update is called once per frame
	void Update () {
		if (targetedWord != null && Input.inputString.Length > 0 && 
		    Input.inputString.Contains(targetedWord.name[0].ToString()))
			CutMesh ();

		if(targetedWord != null) PulseMesh ();
	}

	void PulseMesh()
	{
		if (currentPulse == PulseType.NONE)
			return;
		else if (currentPulse == PulseType.IN) targetedWord.transform.localScale -= new Vector3 (pulseSpeed, pulseSpeed, pulseSpeed);
		else targetedWord.transform.localScale += new Vector3 (pulseSpeed, pulseSpeed, pulseSpeed);

		if (targetedWord.transform.localScale.x >= originalWordScale.x)
			currentPulse = PulseType.NONE;
		else if (targetedWord.transform.localScale.x <= originalWordScale.x / 2.0f)
			currentPulse = PulseType.OUT;

	}
}
