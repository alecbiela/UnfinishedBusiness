using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {
    
    public enum GameStates { RUNNING = 0, PAUSED, VIEWING_OBJECT, STOPPED};
    private GameStates currentState, previousState;

    private GameObject runningUI, pausedUI;
    private GameObject[] invSlots;
    private ObjectViewer objViewer;
    private RigidbodyFirstPersonController player;

	// Use this for initialization
	void Awake () {
        currentState = GameStates.RUNNING;
        previousState = GameStates.STOPPED;
        runningUI = GameObject.Find("RunningUI");
        pausedUI = GameObject.Find("PausedUI");
        objViewer = this.gameObject.GetComponent<ObjectViewer>();
        player = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();
        invSlots = GameObject.FindGameObjectsWithTag("InventorySlot");
	}
	
	// Update is called once per frame
	void Update () {
        if (currentState != previousState) StateChange();
	}

    //sets the current game state
    public void SetState(GameStates state)
    {
        currentState = state;
    }

    //gets the current game state
    public GameStates GetState()
    {
        return currentState;
    }

    //toggles the game paused state
    public void ToggleGamePaused()
    {
        if (currentState == GameStates.PAUSED) currentState = GameStates.RUNNING;
        else currentState = GameStates.PAUSED;
    }

    //start viewing an object
    //takes: the object to view
    public void StartViewingObject(GameObject obj, bool isTemp)
    {
        //change state
        currentState = GameStates.VIEWING_OBJECT;
        objViewer.StartViewing(obj, isTemp);
    }

    //statechange
    private void StateChange()
    {
        previousState = currentState;

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
        }
    }
}
