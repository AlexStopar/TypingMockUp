using UnityEngine;
using System.Collections;


public class TypeWindow : MonoBehaviour {

	GameObject typeTarget; //Where words take place
	GameObject typeBackground; //Background that sizes with word
	Vector3 originalWordScale; //Scale for original word when pulsing
	float wordHeight = 1.0f;
	float padding = 0.2f;
	bool isFading = false;
	float startFadeTime = 0.0f;
	public float fadeDuration = 0.5f;
	PulseType currentPulse = PulseType.NONE;
	// Use this for initialization
	void Start () {

	}

	public void CreateWord(string targetWord, int fontSize, Font typeFont)
	{
		typeTarget = new GameObject ("textWords");
		typeTarget.AddComponent<TextMesh>();
		MeshRenderer meshRender = typeTarget.GetComponent<MeshRenderer>();
		meshRender.material = Resources.Load<Material>("Fonts/unispace");
		TextMesh mesh = typeTarget.GetComponent<TextMesh>();
		mesh.text = targetWord;
		mesh.font = typeFont;
		mesh.fontSize = fontSize;
		mesh.color = Color.green;
		typeTarget.transform.parent = this.transform;
		mesh.anchor = TextAnchor.MiddleCenter;
		if (meshRender.bounds.size.y > wordHeight) 
			this.transform.localScale *= wordHeight/meshRender.bounds.size.y;



		//Make background fit word
		typeBackground = new GameObject ("background");
		typeBackground.AddComponent<SpriteRenderer> ();
		typeBackground.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("Sprites/TypeBackground");
		typeBackground.transform.parent = this.transform;
		typeBackground.transform.localPosition = Vector3.forward;
		if (meshRender.bounds.size.x + (padding * 2.0f) > typeBackground.GetComponent<SpriteRenderer> ().bounds.size.x) {
			typeBackground.transform.localScale = new Vector3 ((meshRender.bounds.size.x + (padding * 2.0f)) /
			typeBackground.GetComponent<SpriteRenderer> ().bounds.size.x * typeBackground.transform.localScale.x, 
			typeBackground.transform.localScale.y, typeBackground.transform.localScale.z);
		}
		originalWordScale = typeBackground.transform.localScale;
	}

	public void CutMesh(float time)
	{
		if (!this.name.Equals("")) {
			this.name = this.name.Substring (1);
			typeTarget.GetComponent<TextMesh> ().text = 
				typeTarget.GetComponent<TextMesh> ().text.Substring (1);
			MeshRenderer meshRender = typeTarget.GetComponent<MeshRenderer> ();
			if (meshRender.bounds.size.x + (padding * 2.0f) < typeBackground.GetComponent<SpriteRenderer> ().bounds.size.x) {
				typeBackground.transform.localScale = new Vector3 ((meshRender.bounds.size.x + (padding * 2.0f)) /
				typeBackground.GetComponent<SpriteRenderer> ().bounds.size.x * typeBackground.transform.localScale.x, 
				typeBackground.transform.localScale.y, typeBackground.transform.localScale.z);
			}
			currentPulse = PulseType.IN;
			if (this.name.Equals (""))
			{
				startFadeTime = time;
				isFading = true;
			}
		}

	}
	// Update is called once per frame
	void Update () {
		if (isFading) {
			float t = (Time.time - startFadeTime) / fadeDuration;
			Color prevColor = typeBackground.GetComponent<SpriteRenderer>().color;
			typeBackground.GetComponent<SpriteRenderer>().color = new Color (prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep (1.0f, 0.0f, t));
		}
		if (typeBackground.GetComponent<SpriteRenderer> ().color.a < 0.01f)
			Destroy (this);
	}

	public void PulseMesh(float pulseSpeed)
	{
		if (currentPulse == PulseType.NONE)
			return;
		else if (currentPulse == PulseType.IN) typeBackground.transform.localScale -= new Vector3 (0, pulseSpeed, 0);
		else typeBackground.transform.localScale += new Vector3 (0, pulseSpeed, 0);
		
		if (typeBackground.transform.localScale.y >= originalWordScale.y)
			currentPulse = PulseType.NONE;
		else if (typeBackground.transform.localScale.y <= originalWordScale.y * 0.75f)
			currentPulse = PulseType.OUT;
		
	}
}
