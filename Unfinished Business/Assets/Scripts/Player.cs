using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public float ObjInteractDist;
    public float MOUSE_SENSITIVITY;

    [SerializeField]
    private GameObject selectedObj = null;
    private Transform viewedObj = null;
    private Color selectedColor;
    private bool viewingObject = false;
    private Vector3 selectedPos, selectedRot, selectedScale, lookRotation;
    private GameObject crosshair;
    private Vector3 mouseDelta = Vector3.zero;
    private Vector3 lastMouse = Vector3.zero;
    private Ray meToSelected;

    // Use this for initialization
    void Start ()
    {
        crosshair = GameObject.Find("Crosshair");
	}
	
    // Update is called once per frame
    void Update()
    {
        //if we right-click while viewing, stop viewing
        if(Input.GetMouseButtonDown(1) && viewingObject)
        {
            StopViewing();
        }

        //if we click on a selected object, start viewing it
        if(Input.GetMouseButtonDown(0) && selectedObj != null)
        {
            if (!viewingObject) StartViewing();
        }

        if (Input.GetMouseButton(0) && viewingObject && mouseDelta != Vector3.zero) RotateViewed();

        if (viewingObject && Input.mouseScrollDelta.y != 0) ScrollViewed();

        //update mouse delta
        mouseDelta = Input.mousePosition - lastMouse;
        lastMouse = Input.mousePosition;
    }


	// FixedUpdate for Physics
	void FixedUpdate ()
    {
        if (!viewingObject) CheckHighlight();
	}

    //dragging to rotate the object around if we're looking at it
    void RotateViewed()
    {
        Vector3 nMouseDelta = mouseDelta.normalized;
        Vector3 moveAxis = Vector3.Cross(nMouseDelta, Camera.main.transform.forward);
        Debug.Log(moveAxis);
        selectedObj.transform.Rotate(moveAxis, MOUSE_SENSITIVITY * Time.deltaTime, Space.World);
    }

    //zooms in and out
    void ScrollViewed()
    {
        float zoomDelta = Input.mouseScrollDelta.y * 3.0f;
        selectedObj.transform.position += Camera.main.transform.forward * zoomDelta * Time.deltaTime;
    }

    //raycasts forward and checks if we should highlight an object
    private void CheckHighlight()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if we are in range of interacting with something
        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, ObjInteractDist))
        {
            //only select if we're hitting something NEW
            if (selectedObj == null)
            {
                //set color and store a reference to the object
                selectedObj = hit.collider.gameObject;
                MeshRenderer mr = selectedObj.GetComponent<MeshRenderer>();
                selectedColor = mr.material.color;
                mr.material.color = Color.yellow;
                meToSelected = ray;
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


    //gets us out of viewing an object
    private void StopViewing()
    {
        viewingObject = false;
        this.GetComponent<RigidbodyFirstPersonController>().ViewingObj = false;

        //free to move again
        selectedObj.GetComponent<Rigidbody>().isKinematic = false;

        //put that thing back where it came from or so help me
        selectedObj.transform.position = selectedPos;
        selectedObj.transform.eulerAngles = selectedRot;
        selectedObj.transform.localScale = new Vector3(selectedScale.x, selectedScale.y, selectedScale.z);
        

        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
    }

    //starts viewing the selected GameObject
    private void StartViewing()
    {
        viewingObject = true;
        this.GetComponent<RigidbodyFirstPersonController>().ViewingObj = true;


        //store a copy of the transform before modifying it
        selectedPos = selectedObj.transform.position;
        selectedRot = selectedObj.transform.rotation.eulerAngles;
        selectedScale = selectedObj.transform.localScale;

        //disable rigidbody so object doesn't just fall out of view
        selectedObj.GetComponent<Rigidbody>().isKinematic = true;

        //move the viewed object to our viewport
        //Posted by user "Julien-Lynge" on the Unity Forums
        //http://answers.unity3d.com/questions/466665/placing-a-gameobject-in-the-exact-center-of-the-ca.html
        selectedObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 3.0f));

        //free the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);

        //stop highlighting
        selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
    }

}
