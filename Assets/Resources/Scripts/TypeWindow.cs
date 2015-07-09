using UnityEngine;
using System.Collections;


public class TypeWindow : MonoBehaviour {

	GameObject typeTarget; //Where words take place
	GameObject typeBackground; //Background that sizes with word
	Vector3 originalWordScale; //Scale for original word when pulsing
	float wordHeight = 1.0f;
	PulseType currentPulse = PulseType.NONE;
	// Use this for initialization
	void Start () {
		typeBackground = new GameObject ("background");
		typeBackground.AddComponent<SpriteRenderer> ();
		typeBackground.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("Sprites/TypeBackground");
		typeBackground.transform.parent = this.transform;
		typeBackground.transform.localPosition = Vector3.forward;
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
		if (meshRender.bounds.size.y > wordHeight) {
			this.transform.localScale *= wordHeight/meshRender.bounds.size.y;
		}	
		originalWordScale = this.transform.localScale;
	}

	public void CutMesh()
	{
		if (this.name.Length > 0) {
			this.name = this.name.Substring (1);
			typeTarget.GetComponent<TextMesh> ().text = 
				typeTarget.GetComponent<TextMesh> ().text.Substring (1);
		}
		if (this.name.Equals (""))
			GameObject.Destroy (typeTarget);
		else
			currentPulse = PulseType.IN;
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void PulseMesh(float pulseSpeed)
	{
		if (currentPulse == PulseType.NONE)
			return;
		else if (currentPulse == PulseType.IN) this.transform.localScale -= new Vector3 (pulseSpeed, pulseSpeed, pulseSpeed);
		else this.transform.localScale += new Vector3 (pulseSpeed, pulseSpeed, pulseSpeed);
		
		if (this.transform.localScale.x >= originalWordScale.x)
			currentPulse = PulseType.NONE;
		else if (this.transform.localScale.x <= originalWordScale.x / 2.0f)
			currentPulse = PulseType.OUT;
		
	}
}
