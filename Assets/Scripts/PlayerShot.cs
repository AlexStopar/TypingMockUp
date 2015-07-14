using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShot : MonoBehaviour {

	List<GameObject> shots;
	public float velocity = 9.0f;
	Vector3 targetPoint = Vector3.zero;
	// Use this for initialization
	void Start () {
		shots = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject shot in shots) {
			Vector3 direction = Vector3.Normalize (targetPoint - this.transform.position);
			shot.transform.rotation = Quaternion.identity;
			shot.transform.position += direction * velocity * Time.deltaTime;
		}
	}

	public void Shoot(){
		string name = "Shot" + shots.Count.ToString ();
		GameObject shot = new GameObject (name);
		shot.AddComponent<SpriteRenderer> ();
		SpriteRenderer sRender = shot.GetComponent<SpriteRenderer> ();
		sRender.sprite = Resources.Load<Sprite>("Sprites/shotTexture");
		sRender.color = (sRender.color * 0.5f) + (Color.cyan * 0.5f);
		shot.transform.parent = this.transform;
		shot.transform.position = this.transform.position;

		GameObject exhaustSystem = Instantiate (Resources.Load<GameObject> ("Particles/ExhaustSystem"));
		exhaustSystem.transform.parent = shot.transform;
		exhaustSystem.transform.localPosition = Vector3.zero;
		shots.Add (shot);
	}

	public void SetTargetPoint(Vector3 target)
	{
		targetPoint = target;
	}
}
