﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class GameManager : MonoBehaviour {
    
    public enum GameStates { RUNNING = 0, PAUSED, VIEWING_OBJECT, PLACING_OBJECT, STOPPED, PAUSEMENU, SETTINGSMENU, START};
    private GameStates currentState, previousState;

    private GameObject runningUI, pausedUI, pauseMenuUI, settingsMenuUI, objViewingUI;
    private ObjectViewer objViewer;
    private RigidbodyFirstPersonController player;
    private GameObject heldObject;
    private bool stateChanged;

    public Ghost ghost;

    public Item HeldItem { get { return heldObject.GetComponent<Item>(); } }

	// Use this for initialization
	void Start () {

        currentState = GameStates.START;
        previousState = GameStates.START;
        runningUI = GameObject.Find("RunningUI");
        pausedUI = GameObject.Find("PausedUI");
        pauseMenuUI = GameObject.Find("PauseMenuUI");
        settingsMenuUI = GameObject.Find("SettingsUI");
        objViewingUI = GameObject.Find("ObjViewingUI");
        objViewer = this.gameObject.GetComponent<ObjectViewer>();
        player = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();

        //fixed screen resolution
        Screen.SetResolution(1024, 768, true, 60);

        //call statechange once to initialize state
        stateChanged = true;

	}


	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EventHandler.handler.ForceStopEvent();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            //ghost.MoveToPosition(new Vector3(24.22f, 0.4f, 33.78f));
        }
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

    //toggles the pause menu
    public void TogglePauseMenu()
    {
        if(currentState != GameStates.PAUSED)
        {
            //if pause menu, unpause to current state, else save current state for later and pause
            if (currentState == GameStates.PAUSEMENU) SetState(previousState);
            else
            {
                if(currentState != GameStates.SETTINGSMENU) previousState = currentState;
                SetState(GameStates.PAUSEMENU);
            }
        }
        
    }

    public void ToggleSettingsMenu()
    {
        SetState((currentState == GameStates.SETTINGSMENU) ? GameStates.PAUSEMENU : GameStates.SETTINGSMENU);
    }

    public void ToggleXhair(bool value)
    {
        
    }

    //start viewing an object
    //takes: the object to view
    public void StartViewingObject(GameObject obj, bool isTemp)
    {
        //stop the player from moving
        Rigidbody prb = player.GetComponent<Rigidbody>();
        prb.velocity = Vector3.zero;
        prb.isKinematic = true;

        //change state
        SetState(GameStates.VIEWING_OBJECT);
        objViewer.StartViewing(obj, isTemp, obj.GetComponent<Item>().viewDistance);
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
                pauseMenuUI.SetActive(false);
                settingsMenuUI.SetActive(false);
                objViewingUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                player.ViewingObj = false;
                break;
            case GameStates.PAUSED:
                Time.timeScale = 0;
                runningUI.SetActive(false);
                pausedUI.SetActive(true);
                pauseMenuUI.SetActive(false);
                settingsMenuUI.SetActive(false);
                objViewingUI.SetActive(false);
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
                pauseMenuUI.SetActive(false);
                settingsMenuUI.SetActive(false);
                objViewingUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.ViewingObj = true;
                break;
            case GameStates.PLACING_OBJECT:
                Time.timeScale = 1;
                pausedUI.SetActive(false);
                runningUI.SetActive(true);
                settingsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(false);
                objViewingUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                player.ViewingObj = false;
                break;
            case GameStates.PAUSEMENU:
                Time.timeScale = 0;
                runningUI.SetActive(false);
                pausedUI.SetActive(false);
                settingsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
                objViewingUI.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.ViewingObj = false;
                break;
            case GameStates.SETTINGSMENU:
                Time.timeScale = 0;
                runningUI.SetActive(false);
                pausedUI.SetActive(false);
                settingsMenuUI.SetActive(true);
                pauseMenuUI.SetActive(false);
                objViewingUI.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.ViewingObj = false;
                break;
            case GameStates.START:
                EventHandler.handler.TriggerEvent(1);
                SetState(GameStates.RUNNING);
                break;

        }
    }
}
