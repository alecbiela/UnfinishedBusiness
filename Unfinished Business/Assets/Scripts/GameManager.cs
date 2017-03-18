using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {
    
    public enum GameStates { RUNNING = 0, PAUSED, VIEWING_OBJECT, PLACING_OBJECT, STOPPED};
    private GameStates currentState, previousState;

    private GameObject runningUI, pausedUI;
    private GameObject[] invSlots;
    private ObjectViewer objViewer;
    private RigidbodyFirstPersonController player;
    private GameObject heldObject;
    private bool stateChanged;

    public Item HeldItem { get { return heldObject.GetComponent<Item>(); } }

	// Use this for initialization
	void Start () {
        currentState = GameStates.RUNNING;
        previousState = GameStates.RUNNING;
        runningUI = GameObject.Find("RunningUI");
        pausedUI = GameObject.Find("PausedUI");
        objViewer = this.gameObject.GetComponent<ObjectViewer>();
        player = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();
        invSlots = GameObject.FindGameObjectsWithTag("InventorySlot");

        //call statechange once to initialize state
        stateChanged = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (stateChanged) StateChange();
	}

    //sets the current game state
    public void SetState(GameStates state)
    {
        currentState = state;
        stateChanged = true;
    }

    //gets the current game state
    public GameStates GetState()
    {
        return currentState;
    }

    //toggles the game paused state
    public void ToggleGamePaused()
    {
        //if paused, unpause to current state, else save current state for later and pause
        if (currentState == GameStates.PAUSED) SetState(previousState);
        else
        {
            previousState = currentState;
            SetState(GameStates.PAUSED);
        }
    }

    //start viewing an object
    //takes: the object to view
    public void StartViewingObject(GameObject obj, bool isTemp)
    {
        //stop the player from moving
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //change state
        SetState(GameStates.VIEWING_OBJECT);
        objViewer.StartViewing(obj, isTemp);
    }

    //brings an object "outside" the inventory for use in the world
    public void SelectObject(GameObject obj)
    {
        //change state
        SetState((obj == null) ? GameStates.RUNNING : GameStates.PLACING_OBJECT);
        heldObject = obj;
        objViewer.StartSelecting(obj);
    }

    //statechange
    private void StateChange()
    {
        stateChanged = false;

        switch (currentState)
        {
            case GameStates.RUNNING:
                Time.timeScale = 1;
                pausedUI.SetActive(false);
                runningUI.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                player.ViewingObj = false;
                break;
            case GameStates.PAUSED:
                Time.timeScale = 0;
                runningUI.SetActive(false);
                pausedUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.ViewingObj = false;
                break;
            case GameStates.STOPPED:
                break;
            case GameStates.VIEWING_OBJECT:
                Time.timeScale = 1;
                pausedUI.SetActive(false);
                runningUI.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.ViewingObj = true;
                break;
            case GameStates.PLACING_OBJECT:
                Time.timeScale = 1;
                pausedUI.SetActive(false);
                runningUI.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                player.ViewingObj = false;
                break;
        }
    }
}
