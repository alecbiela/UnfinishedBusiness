using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float ObjInteractDist;
    public float MOUSE_SENSITIVITY;
    public float SCALE_SENSITIVITY;

    [SerializeField]
    private GameObject selectedObj = null;
    private Transform viewedObj = null;
    private Color selectedColor;
    private bool viewingObject = false;
    private Vector3 selectedPos, selectedRot, selectedScale, lookRotation;
    private GameObject crosshair;
    private GameObject examineText;
    private Vector3 mouseDelta = Vector3.zero;
    private Vector3 lastMouse = Vector3.zero;
    private float distance = 3.0f;
    private float maxDistance;
    private float minDistance;
    private GameManager gm;

    //inventory 
    //private Dictionary<string, GameObject> inventory;
    private Inventory inventory;

    // Use this for initialization
    void Start ()
    {
        crosshair = GameObject.Find("Crosshair");
        inventory = this.gameObject.GetComponent<Inventory>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        examineText = GameObject.Find("ExamineText");
        //empty dictionary for inventory

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
		if(Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<Renderer>().enabled && selectedObj.GetComponent<Item>() != null)
        {
            if (!viewingObject) StartViewing();
        }

		//if we click on a door, open int
		if (Input.GetMouseButtonDown (0) && selectedObj != null && selectedObj.GetComponent<Door>() != null) {
			ActivateDoor ();
		}

        //if we click a cabinet/drawer, open/close it
        if (Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<Cabinet>() != null)
        {
            //switch its state
            selectedObj.GetComponent<Cabinet>().open = !selectedObj.GetComponent<Cabinet>().open;
        }

        if (Input.GetMouseButton(0) && viewingObject) RotateViewed();

        if (viewingObject && Input.mouseScrollDelta.y != 0) ScrollViewed();

        if (Input.GetKeyDown(KeyCode.P)) gm.ToggleGamePaused();
    }


	// FixedUpdate for Physics
	void FixedUpdate ()
    {
        if (!viewingObject) CheckHighlight();
	}

    //dragging to rotate the object around if we're looking at it
    void RotateViewed()
    {
        //update mouse delta (in world space)
        Vector3 objOnScreen = Camera.main.WorldToScreenPoint(selectedObj.transform.position);
        mouseDelta = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objOnScreen.z)) - lastMouse;
        lastMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objOnScreen.z));

        //get out early if we can
        if (mouseDelta == Vector3.zero) return;

        //normalize delta vector and vector between us and the object
        Vector3 nMouseDelta = mouseDelta.normalized;
        Vector3 between = (selectedObj.transform.position - this.transform.position).normalized;

        //cross above vectors to get axis of rotation, then rotate
        Vector3 moveAxis = Vector3.Cross(nMouseDelta, between).normalized;
        selectedObj.transform.Rotate(moveAxis, MOUSE_SENSITIVITY * Time.deltaTime, Space.World);
    }

    //zooms in and out
    void ScrollViewed()
    {
        if (Input.mouseScrollDelta.y < 0) selectedObj.transform.localScale *= 1 + SCALE_SENSITIVITY;
        else selectedObj.transform.localScale *= 1 - SCALE_SENSITIVITY;

        //check bounds
        if(selectedObj.transform.localScale.x <= 0.01f)
        {
            selectedObj.transform.localScale = Vector3.one;
            selectedObj.transform.localScale *= 0.01f;
        }
        /*distance += Input.mouseScrollDelta.y * Time.deltaTime;
        if (distance > maxDistance) distance = maxDistance;
        else if (distance < minDistance) distance = minDistance;
        selectedObj.transform.position += Camera.main.transform.forward * distance;*/
    }

    //raycasts forward and checks if we should highlight an object
    private void CheckHighlight()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if we are in range of interacting with something
        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, ObjInteractDist))
        {
            //only select if we're hitting something NEW and visible
            //edited: only selecting if selectedObj == null created issues with objects that are very close together + updating text
            if (hit.collider.gameObject.GetComponent<Renderer>().enabled)
            {
                //restore the color of the previous selected object before highlighting the new one
                if (selectedObj != null)
                {
                    MeshRenderer prev = selectedObj.GetComponent<MeshRenderer>();
                    prev.material.color = selectedColor;
                }

                //set color and store a reference to the object
                selectedObj = hit.collider.gameObject;
                MeshRenderer mr = selectedObj.GetComponent<MeshRenderer>();
                selectedColor = mr.material.color;
                mr.material.color = Color.yellow;

               //adjust examining text depending on type of object being highlighted
				string examine = "";
				if (selectedObj.GetComponent<Item> () != null) {
					 examine = "Left Click to Examine \n" + selectedObj.GetComponent<Item> ().itemName;
				}
				else if (selectedObj.GetComponent<Door> () != null) {
					if (!selectedObj.GetComponent<Door> ().open) {
						examine = "Left Click to Open Door";
					} else {
						examine = "Left Click to Close Door";
					}
				}
                else if (selectedObj.GetComponent<Cabinet>()!= null)
                {
                    if (!selectedObj.GetComponent<Cabinet>().open)
                    {
                        examine = "Left Click to Open Cabinet";
                    }
                    else
                    {
                        examine = "Left Click to Close Cabinet";
                    }
                }
                examineText.GetComponent<Text>().text = examine;
            }
        }
        else
        {
            //if we're leaving collision, reset color and text
            if (selectedObj != null)
            {
                selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
                selectedObj = null;
                examineText.GetComponent<Text>().text = " ";
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
        //but only if it's not obtainable
        if (!selectedObj.GetComponent<Item>().obtainable)
        {
            selectedObj.transform.position = selectedPos;
            selectedObj.transform.eulerAngles = selectedRot;
            selectedObj.transform.localScale = new Vector3(selectedScale.x, selectedScale.y, selectedScale.z);
        }
        //otherwise add to the inventory
        else
        {
            //hide the object
            selectedObj.GetComponent<Renderer>().enabled = false;

            //add reference to it in inventory
            //inventory.Add(selectedObj.GetComponent<Item>().itemName, selectedObj.gameObject);
            inventory.AddItem(selectedObj.GetComponent<Item>());

            //until the player moves again it still displays the text to view the object that's been picked up
            //to get around this i'm just going to change it here to say "x added to inventory"
            string added = selectedObj.GetComponent<Item>().itemName + "\nadded to inventory.";
            examineText.GetComponent<Text>().text = added;
        }
        

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
        selectedObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.WorldToScreenPoint(selectedObj.transform.position).z));


        //free the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);

        //stop highlighting
        selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
    }

	void ActivateDoor(){
		//in retrospect this probably didn't need to be its own method
		selectedObj.GetComponent<Door>().open = !selectedObj.GetComponent<Door>().open;
	}
}
