using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

//Represents the player
//Currently moved using the rigidbody FPS controller
public class Player : MonoBehaviour {
    public float ObjInteractDist;
    public float MOUSE_SENSITIVITY;
    public float SCALE_SENSITIVITY;

    [SerializeField]
    private GameObject selectedObj = null;
    private Color selectedColor;
    private GameManager gm;
    private Inventory inventory;

    // Use this for initialization
    void Start ()
    {
        //crosshair = GameObject.Find("Crosshair");
        inventory = this.gameObject.GetComponent<Inventory>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //add case file and badge to the inventory
        GameObject badge = GameObject.Find("detective_badge");
        GameObject file = GameObject.Find("case_file");

        inventory.AddItem(badge);
        inventory.AddItem(file);
	}
	
    // Update is called once per frame
    void Update()
    {
        if (TextHandler.handler.PlayingImportantText) return;

        //check for mouse click on object
        if (Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<Renderer>().enabled && selectedObj.GetComponent<Item>() != null)
        {
            if (gm.GetState() == GameManager.GameStates.PLACING_OBJECT)
            {
                //if the used item matches the necessary reagent item
                if(selectedObj.GetComponent<Item>().UseOnMe(gm.HeldItem.itemID))
                {
                    //remove object from inventory and stop selecting it
                    inventory.RemoveItem(gm.HeldItem.itemName);
                    gm.SelectObject(null);
                }
            }
            else if (!(gm.GetState() == GameManager.GameStates.VIEWING_OBJECT) && !TextHandler.handler.PlayingImportantText)
            {
                selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
                selectedObj.GetComponent<Item>().Activate();
            }
        }

        //stop selecting object if we right click (and don't click on anything else)
        else if(Input.GetMouseButtonDown(1) && gm.GetState() == GameManager.GameStates.PLACING_OBJECT)
        {
            gm.SelectObject(null);
        }

        //no longer able to pause while playing important text
        else if (Input.GetKeyDown(KeyCode.P)
            && (gm.GetState() != GameManager.GameStates.PAUSEMENU) && (gm.GetState() != GameManager.GameStates.SETTINGSMENU)
            ) gm.ToggleGamePaused();

        //opens pause menu (if not playing important text)
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gm.TogglePauseMenu();
        }
    }


	// FixedUpdate for Physics
	void FixedUpdate ()
    {
        if (!(gm.GetState() == GameManager.GameStates.VIEWING_OBJECT)) CheckHighlight();
	}


    //raycasts forward and checks if we should highlight an object
    private void CheckHighlight()
    {
        //return if this camera is not available
        if (Camera.main == null) return;

        RaycastHit hit;

        //if we are in range of interacting with something
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, ObjInteractDist))
        {
            //only select if we're hitting something NEW and visible
            //edited: only selecting if selectedObj == null created issues with objects that are very close together + updating text
            if (hit.collider.gameObject.GetComponent<Renderer>().enabled)
            {
                //get the gamestate
                GameManager.GameStates currentState = gm.GetState();
                string examine = "";

                //decide how to handle the object based on the current state


                //restore the color of the previous selected object before highlighting the new one
                if (selectedObj != null)
                {
                    MeshRenderer prev = selectedObj.GetComponent<MeshRenderer>();
                    prev.material.color = selectedColor;
                }

                //store new selected object
                selectedObj = hit.collider.gameObject;


                //adjust examining text depending on state of game
                switch (currentState)
                {
                    case GameManager.GameStates.PLACING_OBJECT: //using item on item

                        if (selectedObj.GetComponent<Item>() != null)
                        {
                            examine = "Left click to use item on \n" + selectedObj.GetComponent<Item>().itemName;
                        }
                        break;

                    case GameManager.GameStates.RUNNING:    //normal object examination
                        examine = selectedObj.GetComponent<Item>().ExamineInfo;
                        break;

                    default:
                        break;
                }

                //set color and store a reference to the object               
                MeshRenderer mr = selectedObj.GetComponent<MeshRenderer>();
                selectedColor = mr.material.color;
                mr.material.color = Color.yellow;

                //display the text on screen
                TextHandler.handler.DisplayExamineText(examine);
            }

        }
        else
        {
            //if we're leaving collision, reset color and text
            if (selectedObj != null)
            {
                selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
                selectedObj = null;

                //clear text if there's no timer
                TextHandler.handler.ClearExamineTextSafe();
            }
        }
    }

    //loads the main menu
    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
