using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public enum GameStates { RUNNING = 0, PAUSED, STOPPED};
    private GameStates currentState, previousState;

    private GameObject runningUI, pausedUI;

	// Use this for initialization
	void Start () {
        currentState = GameStates.RUNNING;
        previousState = GameStates.STOPPED;
        runningUI = GameObject.Find("RunningUI");
        pausedUI = GameObject.Find("PausedUI");
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

    //toggles the game paused state
    public void ToggleGamePaused()
    {
        if (currentState == GameStates.PAUSED) currentState = GameStates.RUNNING;
        else currentState = GameStates.PAUSED;
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
                break;
            case GameStates.PAUSED:
                Time.timeScale = 0;
                runningUI.SetActive(false);
                pausedUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameStates.STOPPED:
                break;
        }
    }
}
