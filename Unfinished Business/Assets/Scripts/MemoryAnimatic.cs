using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryAnimatic : MonoBehaviour {

    public Sprite[] frames;
    public int[] timeBetweenFrames;

    private float timer = 0;
    private int index = 0;

    public Camera gameCam;
    public Camera memoryCam;

    private GameObject canvas;

	// Use this for initialization
	void Start () {

        //grab canvas to toggle hide
        canvas = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
		

        //only play if camera is looking
        if (memoryCam.enabled)
        {
            canvas.GetComponent<Canvas>().enabled = false;
            gameCam.enabled = false;
            Play();
        }
	}

    void Play()
    {
        //when finished playing return to game
        if (index == frames.Length)
        {
            Stop();
            return;
        }

        //update displaying frame and timer
        gameObject.GetComponent<SpriteRenderer>().sprite = frames[index];

        timer += (1000* Time.deltaTime);
        if (timer >= timeBetweenFrames[index])
        {
            timer = 0;
            index++;
        }
    }

    //stop the current animatic and return to game
    public void Stop()
    {
        canvas.GetComponent<Canvas>().enabled = true;
        memoryCam.enabled = false;
        gameCam.enabled = true;

        //trigger post memory
        EventHandler.handler.TriggerEvent(4);
    }
}
