using UnityEngine;
using System.Collections;

public enum FadeType {FADEIN, FADEOUT, NOFADE};

public class TypeWindow : MonoBehaviour {

	//Audio for Window
	AudioClip typeSound;
	AudioClip enterSound;
	AudioClip exitSound;

	GameObject typeTarget; //Where words take place
	GameObject typeBackground; //Background that sizes with word
	Vector3 originalWordScale; //Scale for original word when pulsing
	float wordHeight = 0.7f;
	float padding = 0.2f; //Padding of words from sides in window
	float verticalSizeReduction = 0.5f; //Make window shorter
	float windowVolume = 0.5f;
	public bool isDead = false;
	float startFadeTime = 0.0f;
	public float fadeDuration = 0.2f;
	PulseType currentPulse = PulseType.NONE;
	public FadeType currentFade = FadeType.FADEIN;
	// Use this for initialization
	void Start () {
		startFadeTime = Time.time;
		typeSound = Resources.Load<AudioClip> ("Sounds/Type");
		enterSound = Resources.Load<AudioClip> ("Sounds/Enter");
		exitSound = Resources.Load<AudioClip> ("Sounds/Exit");
		AudioSource.PlayClipAtPoint(enterSound, Vector3.zero, windowVolume);
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
		meshRender.material.color = new Color (meshRender.material.color.r, meshRender.material.color.g, meshRender.material.color.b, 0.0f);
		//Add sound source on typeTarget


		//Make background fit word
		typeBackground = new GameObject ("background");
		typeBackground.AddComponent<SpriteRenderer> ();
		SpriteRenderer spriteRender = typeBackground.GetComponent<SpriteRenderer> ();
		spriteRender.sprite = (Sprite)Resources.Load<Sprite> ("Sprites/TypeBackground");
		spriteRender.color = new Color (spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 0.0f);
		typeBackground.transform.parent = this.transform;
		typeBackground.transform.localPosition = Vector3.forward;

		typeBackground.transform.localScale = new Vector3 ((meshRender.bounds.size.x + (padding * 2.0f)) /
		typeBackground.GetComponent<SpriteRenderer> ().bounds.size.x * typeBackground.transform.localScale.x, 
		typeBackground.transform.localScale.y * verticalSizeReduction, typeBackground.transform.localScale.z);

		originalWordScale = typeBackground.transform.localScale;
	}

	public void CutMesh(float time)
	{
		if (typeTarget.GetComponent<MeshRenderer> ().material.color.a < 0.9f)
			return;
		if (!this.name.Equals("")) {
			AudioSource.PlayClipAtPoint(typeSound, Vector3.zero, windowVolume);
			this.name = this.name.Substring (1);
			typeTarget.GetComponent<TextMesh> ().text = 
				typeTarget.GetComponent<TextMesh> ().text.Substring (1);
			MeshRenderer meshRender = typeTarget.GetComponent<MeshRenderer> ();
			typeBackground.transform.localScale = new Vector3 ((meshRender.bounds.size.x + (padding * 2.0f)) /
			typeBackground.GetComponent<SpriteRenderer> ().bounds.size.x * typeBackground.transform.localScale.x, 
			typeBackground.transform.localScale.y, typeBackground.transform.localScale.z);

			currentPulse = PulseType.IN;
			if (this.name.Equals (""))
			{
				startFadeTime = time;
				AudioSource.PlayClipAtPoint(exitSound, Vector3.zero, windowVolume);
				currentFade = FadeType.FADEOUT;
			}
		}

	}
	// Update is called once per frame
	void Update () {
		if (currentFade == FadeType.FADEIN) {
			float t = (Time.time - startFadeTime) / fadeDuration;
			Color prevColor = typeBackground.GetComponent<SpriteRenderer>().color;
			typeBackground.GetComponent<SpriteRenderer>().color = new Color (prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep (0.0f, 1.0f, t));
			prevColor = typeTarget.GetComponent<MeshRenderer>().material.color;
			typeTarget.GetComponent<MeshRenderer>().material.color = new Color (prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep (0.0f, 1.0f, t));
		}
		else if (currentFade == FadeType.FADEOUT) {
			float t = (Time.time - startFadeTime) / fadeDuration;
			Color prevColor = typeBackground.GetComponent<SpriteRenderer>().color;
			typeBackground.GetComponent<SpriteRenderer>().color = new Color (prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep (1.0f, 0.0f, t));
		}
		if (typeBackground.GetComponent<SpriteRenderer> ().color.a >= 1.0f && currentFade == FadeType.FADEIN) {
			currentFade = FadeType.NOFADE;
		}
		if (typeBackground.GetComponent<SpriteRenderer> ().color.a < 0.01f && currentFade == FadeType.FADEOUT) {
			isDead = true;
		}
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
