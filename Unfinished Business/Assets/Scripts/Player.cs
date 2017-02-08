using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float ObjInteractDist;
    private GameObject selectedObj = null;
    private Color selectedColor;
    private bool viewingObject = false;

	// Use this for initialization
	void Start () {
		
	}
	
    // Update is called once per frame
    void Update()
    {
        //checks if we're clicking on a selected object
        if(Input.GetMouseButtonDown(0) && selectedObj != null)
        {
            //if we're already viewing and trying to get out
            if(viewingObject)
            {
                Time.timeScale = 1;
                viewingObject = false;

                //destroy the viewed object

                //lock the cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else //start viewing the object
            {
                Time.timeScale = 0;
                viewingObject = true;

                //create a viewed object

                //free the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }


	// FixedUpdate for Physics
	void FixedUpdate () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if we are in range of interacting with something
        if(Physics.Raycast(this.transform.position, ray.direction, out hit, ObjInteractDist))
        {
            //only select if we're hitting something NEW
            if (selectedObj == null)
            {
                //set color and store a reference to the object
                selectedObj = hit.collider.gameObject;
                MeshRenderer mr = selectedObj.GetComponent<MeshRenderer>();
                selectedColor = mr.material.color;
                mr.material.color = Color.yellow;
            }
        }
        else
        {
            //if we're leaving collision, reset color
            if (selectedObj != null)
            {
                selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
                selectedObj = null;
            }
        }


	}


}
