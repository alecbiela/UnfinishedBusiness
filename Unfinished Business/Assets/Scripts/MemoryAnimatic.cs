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
            canvas.GetComponent<Canvas>().enabled = true;
            memoryCam.enabled = false;
            gameCam.enabled = true;

            return;
        }

        //update displaying frame and timer
        gameObject.GetComponent<SpriteRenderer>().sprite = frames[index];

        timer += Time.deltaTime;
        if (timer >= timeBetweenFrames[index])
        {
            timer = 0;
            index++;
        }
    }
}
