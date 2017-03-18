using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    private GameObject mainScreen;
    private GameObject optionsScreen;

    //initialization
    void Awake()
    {
        mainScreen = GameObject.Find("MainUI");
        optionsScreen = GameObject.Find("OptionsUI");
        optionsScreen.SetActive(false);
    }

    //handles scene change button presses
    public void StartScene(string sceneName)
    {
        //closes all open scenes and loads the new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    //handles pressing of the 'options' button
    public void ToggleOptions()
    {
        //hide/show the options screen
        optionsScreen.SetActive(!optionsScreen.activeInHierarchy);

        //hide/show the main screen
        mainScreen.SetActive(!mainScreen.activeInHierarchy);
    }
}
