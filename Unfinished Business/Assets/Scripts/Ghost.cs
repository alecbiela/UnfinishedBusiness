using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    Vector3 startPosition;
    float t;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        t = 0f;
        MoveToPosition(new Vector3(24.22f, 0.4f, 33.78f), 10f);
	}
	
	// Update is called once per frame
	void Update () {
        //make him point at the main camera, actually the most terrifying thing ever
        //transform.LookAt(Camera.main.transform);
        
	}

    public void MoveToPosition(Vector3 pos, float timeToMove)
    {
        startPosition = transform.position;
        t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(startPosition, pos, t);
        }
    }
}
