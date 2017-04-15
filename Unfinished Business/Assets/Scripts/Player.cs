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
    private Color selectedColor;
    private GameObject examineText;
    private GameManager gm;
    //private string onScreenText;
    private Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    //inventory
    private Inventory inventory;

    // Use this for initialization
    void Start ()
    {
        //crosshair = GameObject.Find("Crosshair");
        inventory = this.gameObject.GetComponent<Inventory>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        examineText = GameObject.Find("ExamineText");
	}
	
    // Update is called once per frame
    void Update()
    {

        //if we click on a selected object, start viewing it
		if(Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<Renderer>().enabled && selectedObj.GetComponent<Item>() != null)
        {
            if (gm.GetState() == GameManager.GameStates.PLACING_OBJECT)
            {
                //if the used item matches the necessary reagent item
                if(selectedObj.GetComponent<Item>().UseOnMe(gm.HeldItem.itemID))
                {
                    Debug.Log("The item reacts!");
                    //onScreenText = gm.HeldItem.itemName + " used on " + selectedObj.GetComponent<Item>().itemName;

                    //remove object from inventory and stop selecting it
                    inventory.RemoveItem(gm.HeldItem.itemName);
                    gm.SelectObject(null);
                }
                else
                {
                    //debug log - remove once object use-on-item is nailed down
                    Debug.Log("Item doesn't work on that");
                    //onScreenText = "Nothing happens.";
                }
            }
            else if (!(gm.GetState() == GameManager.GameStates.VIEWING_OBJECT) && !TextHandler.handler.PlayingImportantText)
            {
                selectedObj.GetComponent<MeshRenderer>().material.color = selectedColor;
                gm.StartViewingObject(selectedObj, false);
            }
        }

        if (Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<AnimatedObject>() != null)
        {
            selectedObj.GetComponent<AnimatedObject>().Animate();
        }

		//if we click on a door, open it
		/*if (Input.GetMouseButtonDown (0) && selectedObj != null && selectedObj.GetComponent<Door>() != null) {
			ActivateDoor ();
		}

        //if we click a cabinet/drawer, open/close it
        if (Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<Cabinet>() != null)
        {
            //switch its state
            selectedObj.GetComponent<Cabinet>().open = !selectedObj.GetComponent<Cabinet>().open;
        }

        //toolboxes too!
        if (Input.GetMouseButtonDown(0) && selectedObj != null && selectedObj.GetComponent<ToolBoxDoor>() != null)
        {
            //switch its state
            selectedObj.GetComponent<ToolBoxDoor>().open = !selectedObj.GetComponent<ToolBoxDoor>().open;
        }*/

        //update on screen text
        //UpdateOnScreenText();

        //stop selecting object if we right click (and don't click on anything else)
        if(Input.GetMouseButtonDown(1) && gm.GetState() == GameManager.GameStates.PLACING_OBJECT)
        {
            gm.SelectObject(null);
        }

        //no longer able to pause while playing important text
        if (Input.GetKeyDown(KeyCode.P) && !TextHandler.handler.PlayingImportantText) gm.ToggleGamePaused();

        //opens pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gm.TogglePauseMenu();
        }
    }


	// FixedUpdate for Physics
	void FixedUpdate ()
    {
        if (!(gm.GetState() == GameManager.GameStates.VIEWING_OBJECT)) CheckHighlight();
	}

    //pushes whatever is in the onScreenText variable onto the screen
    //private void UpdateOnScreenText()
    //{
    //    examineText.GetComponent<Text>().text = onScreenText;
    //}

    //raycasts forward and checks if we should highlight an object
    private void CheckHighlight()
    {
        //return if this camera is not available
        if (Camera.main == null) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

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

                        if (selectedObj.GetComponent<Item>() != null)
                        {
                            examine = "Left Click to Examine \n" + selectedObj.GetComponent<Item>().itemName;
                        }
                        else
                        {
                            examine = "Left Click to Activate \n" + selectedObj.name;
                        }
                        /*else if (selectedObj.GetComponent<Door>() != null)
                        {
                            if (!selectedObj.GetComponent<Door>().open)
                            {
                                examine = "Left Click to Open Door";
                            }
                            else {
                                examine = "Left Click to Close Door";
                            }
                        }
                        else if (selectedObj.GetComponent<Cabinet>() != null)
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
                        else if (selectedObj.GetComponent<ToolBoxDoor>() != null)
                        {
                            if (!selectedObj.GetComponent<ToolBoxDoor>().open)
                            {
                                examine = "Left Click to Open Tool Box";
                            }
                            else
                            {
                                examine = "Left Click to Close Tool Box";
                            }
                        }*/
                        break;
                    default:
                        break;
                }


            //if object is not something interactable, don't highlight
            //else
            //{
            //    selectedColor = selectedObj.GetComponent<MeshRenderer>().material.color;
            //    return;
            //}

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
                //onScreenText = " ";

                //clear text if there's no timer
                TextHandler.handler.ClearExamineTextSafe();
            }
        }
    }

	//void ActivateDoor(){
		//in retrospect this probably didn't need to be its own method
		//selectedObj.GetComponent<Door>().open = !selectedObj.GetComponent<Door>().open;
	//}

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
