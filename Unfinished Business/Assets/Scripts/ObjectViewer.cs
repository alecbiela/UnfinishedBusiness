using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectViewer : MonoBehaviour {

    private GameManager gm;
    private GameObject viewedObj;
    private Vector3 selectedPos, selectedRot, selectedScale, lookRotation;
    private GameObject crosshair;
    private Sprite defaultCrosshair;
    private bool viewingObject = false;
    private Vector3 mouseDelta = Vector3.zero;
    private Vector3 lastMouse = Vector3.zero;
    private float distance = 3.0f;
    private bool isTemporary = true;
    public float MOUSE_SENSITIVITY;
    private Slider sliderSens;
    private bool displayXHair;
    private Toggle toggleXhair;
    private Image crosshairImgComponent;

    // Use this for initialization
    void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        crosshair = GameObject.Find("Crosshair");
        crosshair.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        sliderSens = GameObject.Find("SliderSens").GetComponent<Slider>();
        displayXHair = true;
        toggleXhair = GameObject.Find("CrosshairToggle").GetComponent<Toggle>();
        crosshairImgComponent = crosshair.GetComponent<Image>();
        defaultCrosshair = crosshairImgComponent.sprite;
    }
	
	// Update is called once per frame
	void Update () {
        //stop viewing if we right click
        if (Input.GetMouseButtonDown(1) && viewingObject)
        {
            if(!TextHandler.handler.PlayingImportantText) StopViewing();
        }

        SetSensivity();
        if (Input.GetMouseButton(0) && viewingObject) RotateViewed();

        //Disabled scrolling, but the code is still there just in case we need it
        //if (viewingObject && Input.mouseScrollDelta.y != 0) ScrollViewed();
    }

    //starts selecting an object
    public void StartSelecting(GameObject obj)
    {
        //FIX THIS
        crosshair.GetComponent<Image>().sprite = (obj == null) ? defaultCrosshair : obj.GetComponent<Item>().uiImage;
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

        //set shaders to unlit
        //Material[] shaders = selectedObj.GetComponent<Renderer>().materials;
        //foreach (Material m in shaders) m.shader = Shader.Find("Unlit/Texture");

        //disable rigidbody so object doesn't just fall out of view
        selectedObj.GetComponent<Rigidbody>().isKinematic = true;

        //move the viewed object to our viewport
        //Posted by user "Julien-Lynge" on the Unity Forums
        //http://answers.unity3d.com/questions/466665/placing-a-gameobject-in-the-exact-center-of-the-ca.html
        selectedObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, startingDistance));

        //turn off collider
        selectedObj.GetComponent<Collider>().enabled = false;
        selectedObj.layer = 8;  //set to "always-on-top" layer

        viewedObj = selectedObj;
    }

    //gets us out of viewing an object
    public void StopViewing()
    {
        viewingObject = false;

        //set shaders to lit
        //Material[] shaders = viewedObj.GetComponent<Renderer>().materials;
        //foreach(Material m in shaders) m.shader = Shader.Find("Standard");


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
            if (!isTemporary)
            {
                GameObject.Find("Player").GetComponent<Inventory>().AddItem(viewedObj);

                //now tells the texthandler to do it up
                TextHandler.handler.DisplayExamineText(viewedObj.GetComponent<Item>().itemName + "\nadded to inventory.");
                TextHandler.handler.StartExamineTimer(2500);
            }
            else
            {
                TextHandler.handler.ClearExamineText();
            }


            //hide the object
            viewedObj.GetComponent<Renderer>().enabled = false;
        }

        //Lastly, set state to running before we resume
        // and try to trigger the event
        //Also, resume player movement
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().isKinematic = false;
        if (!isTemporary) viewedObj.GetComponent<Item>().PutMeDown();
        gm.SetState(GameManager.GameStates.RUNNING);
    }

    //Rotates the viewed object
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
        distance = Input.mouseScrollDelta.y * Time.deltaTime * 6f;
        viewedObj.transform.position += Camera.main.transform.forward * distance;
    }

    
    public void ToggleXhair()
    {
        displayXHair = toggleXhair.isOn;
        crosshairImgComponent.enabled = displayXHair;
    }

    public void SetSensivity()
    {
        MOUSE_SENSITIVITY = sliderSens.value;
    }
}
