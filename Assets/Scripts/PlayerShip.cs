﻿using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {

	bool isMovingLeft = false;
	public float shipAcceleration = 0.2f;
	public float currentVelocity = 0.0f;
	public float maxVelocity = 0.4f;
	public Vector3 targetPoint;
	public float turnSpeed = 0.01f;
	public Vector3 gunLocation = new Vector3(0.0f, 0.2f, 0.0f);
	GameObject gun;
	// Use this for initialization
	void Start () {
		targetPoint = Vector3.up * 4.0f;
		gun = new GameObject ("Gun");
		gun.AddComponent<PlayerShot> ();
		gun.transform.parent = this.transform;
		gunLocation = GameObject.Find ("playership").GetComponentsInChildren
			<MeshRenderer> () [0].bounds.size.x * Vector3.up / 3.0f;
		gun.transform.localPosition = gunLocation;
		gun.GetComponent<PlayerShot> ().SetTargetPoint (targetPoint);
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (this.transform.position.x - targetPoint.x) > 0.1f) {
			if (this.transform.position.x - targetPoint.x < 0.0f) {
				isMovingLeft = false;
			} else {
				isMovingLeft = true;
			}
			float oldVelocity = currentVelocity;
			if (currentVelocity <= maxVelocity) {
				if (isMovingLeft)
					currentVelocity -= (shipAcceleration * Time.deltaTime);
				else
					currentVelocity += (shipAcceleration * Time.deltaTime);
			}
			this.transform.Translate (Vector3.right * (oldVelocity + currentVelocity) / 2.0f);
		} else {
			currentVelocity = 0.0f;
			targetPoint = new Vector3(Random.Range(-5.0f, 5.0f), 4.0f, 0.0f);
			gun.GetComponent<PlayerShot> ().SetTargetPoint (targetPoint);
		}
		Turn (Time.time);

	}

	void Turn(float time)
	{
		if (currentVelocity == 0.0f)
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), Time.time * turnSpeed);
		else if (isMovingLeft)
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0.0f, 45.0f, 0.0f), Time.time * turnSpeed);
		else if (!isMovingLeft)
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0.0f, -45.0f, 0.0f), Time.time * turnSpeed);

		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, 0.0f);
	}
}
