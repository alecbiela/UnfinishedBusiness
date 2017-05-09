using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    Vector3 startPosition;
    float speed;
    public Animator anim;
    public string state;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        speed = 10f;
        anim = GetComponent<Animator>();
        //MoveToPosition(new Vector3(24.22f, 0.4f, 33.78f), 10f);
	}
	
	// Update is called once per frame
	void Update () {
        //make him point at the main camera, actually the most terrifying thing ever
        //transform.LookAt(Camera.main.transform);
    }

    public void MoveToPosition(Vector3 endPos)
    {
        startPosition = transform.position;
       
            transform.position = Vector3.MoveTowards(startPosition, endPos, speed * Time.deltaTime);
        
        

        /*float elapsedTime = 0;
        startPosition = transform.position;
        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPosition, pos, (elapsedTime/timeToMove));
            elapsedTime += Time.deltaTime;
        }
        transform.position = pos;
        t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(startPosition, pos, t);
        }*/
    }

    public void TextureAnimation()
    {

    }

    //processes event with delay
    public void ProcessEvent(int eventID, float delay)
    {

    }
}
