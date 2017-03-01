using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	//door is parented to a cube that represents the rotation point
	//cube rotates if door is clicked
	//player can click the door to change the value of opening or closing
	public bool open = false;


	public float speed = 2.0f;
	private float closedAngle = 0;
	private float openAngle = -90.0f;
	private GameObject rotater;


	// Use this for initialization
	void Start () {
		rotater = this.gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//adjust position of door
		if (open) {
			//if door is open/being opened move toward open position
			var target = Quaternion.Euler(0,openAngle,0);
			rotater.transform.localRotation = Quaternion.Slerp (rotater.transform.localRotation, target, Time.deltaTime * speed);
		}
		if (!open) {
			//if door is closing/being closed rotate toward closed position
			var target = Quaternion.Euler(0,closedAngle,0);
			rotater.transform.localRotation = Quaternion.Slerp (rotater.transform.localRotation, target, Time.deltaTime * speed);
		}
	}
}
