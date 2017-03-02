using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour {
	//cabinet functions similarly to door- moves in and out
	public bool open = false;


	public float speed = 1.5f;

	//moves in the z direction of the object
	public float distance = .2f;
	private Vector3 closePos;
	private Vector3 openPos;

	// Use this for initialization
	void Start () {
        //set initial positions depending if drawer starts as closed or open
        if (!open)
        {
            closePos = this.gameObject.transform.position;
            openPos = closePos;
            openPos += this.gameObject.transform.forward * distance;
        }
        else
        {
            openPos = this.gameObject.transform.position;
            closePos = openPos;
            closePos -= this.gameObject.transform.forward * distance;
        }
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
