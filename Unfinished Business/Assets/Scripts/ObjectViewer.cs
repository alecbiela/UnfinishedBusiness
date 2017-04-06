using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectViewer : MonoBehaviour {

    private GameManager gm;
    private GameObject viewedObj;
    private Vector3 selectedPos, selectedRot, selectedScale, lookRotation;
    //private GameObject examineText;
    private GameObject crosshair;
    private Sprite defaultCrosshair;
    private bool viewingObject = false;
    private Vector3 mouseDelta = Vector3.zero;
    private Vector3 lastMouse = Vector3.zero;
    private float distance = 3.0f;
    private float maxDistance;
    private float minDistance = 1.2f;
    private bool isTemporary = true;
    public float MOUSE_SENSITIVITY;

    // Use this for initialization
    void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //examineText = GameObject.Find("ExamineText");
        crosshair = GameObject.Find("Crosshair");
        crosshair.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        defaultCrosshair = crosshair.GetComponent<Image>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && viewingObject)
        {
            StopViewing();
            if(viewedObj.name == "cassetteTape")
            {
                TextHandler.handler.DisplayText("Paul: Hey Marino, you like that hole through your window?", 2000, 0, 0);
                TextHandler.handler.DisplayText("Well there’s more where that came from. ", 2000, 0, 0);
                TextHandler.handler.DisplayText("You fired me over a steak when I’ve got a family to feed at home.", 2000, 0, 0);
                TextHandler.handler.DisplayText("Thanks to you my wife is leaving me, and she’s taking the kids with her.", 2000, 0, 0);
                TextHandler.handler.DisplayText("I’m gonna kill you for what you did, Marino, and you’ll die miserable and alone.", 2000, 0, 0);
                TextHandler.handler.DisplayText("No one’s gonna miss your sorry rich ass.", 2000, 0, 0);
            }
        }

        if (Input.GetMouseButton(0) && viewingObject) RotateViewed();

        //Disabled scrolling, but the code is still there just in case we need it
        //if (viewingObject && Input.mouseScrollDelta.y != 0) ScrollViewed();
    }

    //starts selecting an object
    public void StartSelecting(GameObject obj)
    {
        //FIX THIS
        crosshair.GetComponent<Image>().sprite = (obj==null) ? defaultCrosshair : obj.GetComponent<Item>().uiImage;
    }

    //starts viewing an object
    public void StartViewing(GameObject selectedObj, bool temporary, float startingDistance)
    {
        viewingObject = true;
        isTemporary = temporary;
        selectedObj.GetComponent<Renderer>().enabled = true;

        //store a copy of the transform before modifying it
        selectedPos = selectedObj.transform.position;
        selectedRot = selectedObj.transform.rotation.eulerAngles;
        selectedScale = selectedObj.transform.localScale;

        //disable rigidbody so object doesn't just fall out of view
        selectedObj.GetComponent<Rigidbody>().isKinematic = true;

        //move the viewed object to our viewport
        //Posted by user "Julien-Lynge" on the Unity Forums
        //http://answers.unity3d.com/questions/466665/placing-a-gameobject-in-the-exact-center-of-the-ca.html
        selectedObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, startingDistance));

        //stop highlighting
        //selectedObj.GetComponent<MeshRenderer>().material.color = new Color(255,255,255,255);

        //turn off collider?
        selectedObj.GetComponent<Collider>().enabled = false;
        selectedObj.layer = 8;  //set to "always-on-top" layer

        viewedObj = selectedObj;
    }

    //gets us out of viewing an object
    public void StopViewing()
    {
        viewingObject = false;



        //put that thing back where it came from or so help me
        //but only if it's not obtainable
        if (!viewedObj.GetComponent<Item>().obtainable)
        {
            viewedObj.transform.position = selectedPos;
            viewedObj.transform.eulerAngles = selectedRot;
            viewedObj.transform.localScale = new Vector3(selectedScale.x, selectedScale.y, selectedScale.z);
            
            //enable collider
            viewedObj.GetComponent<Collider>().enabled = true;
            viewedObj.layer = 0;

            //free to move again
            viewedObj.GetComponent<Rigidbody>().isKinematic = false;
        }
        //otherwise add to the inventory
        else
        {
            //move to ignore raycast layer
            viewedObj.layer = 2;

            //add reference to it in inventory
            //inventory.Add(selectedObj.GetComponent<Item>().itemName, selectedObj.gameObject);
            if (!isTemporary)
            {
                GameObject.Find("Player").GetComponent<Inventory>().AddItem(viewedObj);

                //now tells the texthandler to do it up
                TextHandler.handler.DisplayExamineText(viewedObj.GetComponent<Item>().itemName + "\nadded to inventory.");
                TextHandler.handler.StartExamineTimer(2500);
                //until the player moves again it still displays the text to view the object that's been picked up
                //to get around this i'm just going to change it here to say "x added to inventory"
                //string added = viewedObj.GetComponent<Item>().itemName + "\nadded to inventory.";
                //examineText.GetComponent<Text>().text = added;
            }
            else
            {
                TextHandler.handler.ClearExamineText();
            }


            //hide the object
            viewedObj.GetComponent<Renderer>().enabled = false;
        }

        //Lastly, set state to running before we resume
        gm.SetState(GameManager.GameStates.RUNNING);
    }

    void RotateViewed()
    {
        //update mouse delta (in world space)
        Vector3 objOnScreen = Camera.main.WorldToScreenPoint(viewedObj.transform.position);
        mouseDelta = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objOnScreen.z)) - lastMouse;
        lastMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objOnScreen.z));

        //get out early if we can
        if (mouseDelta == Vector3.zero) return;

        //normalize delta vector and vector between us and the object
        Vector3 nMouseDelta = mouseDelta.normalized;
        Vector3 between = (viewedObj.transform.position - GameObject.Find("Player").transform.position).normalized;

        //cross above vectors to get axis of rotation, then rotate
        Vector3 moveAxis = Vector3.Cross(nMouseDelta, between).normalized;
        viewedObj.transform.Rotate(moveAxis, MOUSE_SENSITIVITY * Time.deltaTime, Space.World);
    }

    //zooms in and out
    void ScrollViewed()
    {
        /*if (Input.mouseScrollDelta.y < 0) selectedObj.transform.localScale *= 1 + SCALE_SENSITIVITY;
        else selectedObj.transform.localScale *= 1 - SCALE_SENSITIVITY;

        //check bounds
        if(selectedObj.transform.localScale.x <= 0.01f)
        {
            selectedObj.transform.localScale = Vector3.one;
            selectedObj.transform.localScale *= 0.01f;
        }*/
        distance = Input.mouseScrollDelta.y * Time.deltaTime * 6f;
        //Vector3 closest = selectedObj.GetComponent<Collider>().bounds.ClosestPoint(this.transform.position);

        //if (distance > maxDistance) distance = maxDistance;
        //else if (distance < minDistance) distance = minDistance;
        viewedObj.transform.position += Camera.main.transform.forward * distance;

    }
}
