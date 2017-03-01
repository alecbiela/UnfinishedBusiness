using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour {
	//cabinet functions similarly to door- moves in and out
	public bool open = false;


	public float speed = 2.0f;

	//moves in the z direction of the object
	public float distance = 1.0f;
	private Vector3 closePos;
	private Vector3 openPos;

	// Use this for initialization
	void Start () {
		closePos = this.gameObject.transform.position;
		openPos = closePos;
		openPos.z += distance;
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {
			//move toward open position
			transform.position = Vector3.MoveTowards(transform.position, openPos, speed * Time.deltaTime);
		}
		if (!open) {
			//move back to closed position
			transform.position = Vector3.MoveTowards(transform.position, closePos, speed * Time.deltaTime);
		}
		
	}
}
