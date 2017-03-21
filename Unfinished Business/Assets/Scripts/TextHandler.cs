using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour {

    //static instance for easy reference
    public static TextHandler handler;

    //holds all of the "message" arguments
    private class TextElement
    {
        public float duration;
        public string message;
        public float fadeInStep;
        public float fadeOutStep;
        public float fadeInTime;
        public float fadeOutTime;

        public TextElement(string m, float d, float fi, float fo)
        {
            message = m; duration = d; fadeInTime = fi; fadeOutTime = fo;
            
            //calculate fade-in-step and fade-out-step
            fadeInStep = Mathf.FloorToInt(fadeInTime / 255);
            fadeOutStep = Mathf.FloorToInt(fadeOutTime / 255);
        }
    };

    //private attributes
    private float timer;
    private Text textNode;
    private Queue<TextElement> textQueue;
    TextElement currentElement;

	// Use this for initialization
	private void Start ()
    {
        //find text ui element
        textNode = GameObject.Find("Text Node").GetComponent<Text>();

        //initialize variables
        textQueue = new Queue<TextElement>();
        handler = this;
        timer = 0;
	}


    // Public methods

    //adds text to the queue to be displayed with fade-in and fade-out
    public void DisplayText(string text, float duration, float fadeInTime, float fadeOutTime)
    {
        //add a new element to the queue to be displayed
        textQueue.Enqueue(new TextElement(text, duration, fadeInTime, fadeOutTime));
    }

    //immediately removes text from the screen and cancels all waiting text
    public void ClearAllText()
    {
        timer = 0;
        textQueue.Clear();
    }
	


    // Private (helper) methods

	//update is called once per frame
	private void Update ()
    {
        //if we're timing a message
	    if(timer > 0)
        {
            //check fading
            //(does not work for now, but will implement later)
            /*if((currentElement.duration - timer) < currentElement.fadeInTime)   //fading in
            {
                textNode.color = new Color(textNode.color.r, textNode.color.g, textNode.color.b, textNode.color.a + currentElement.fadeInStep);
            }
            else if(timer < currentElement.fadeOutTime) //fading out
            {
                textNode.color = new Color(textNode.color.r, textNode.color.g, textNode.color.b, textNode.color.a - currentElement.fadeOutStep);
            }*/

            //decrement the timer
            timer -= (1000 * Time.deltaTime);
        }

        //checks for change in queue elements
        if (timer <= 0)
        {
            if(textQueue.Count !=0) InitNewText();
            else
            {
                timer = 0;
                textNode.text = "";
            }
        }
	}
   
    // Initializes the next element in the queue for displaying
    private void InitNewText()
    {
        //pop a new text off of the front of the queue
        currentElement = textQueue.Dequeue();

        //change the text element on screen and reset the timer (and alpha, if needed)
        textNode.text = currentElement.message;
        textNode.color = currentElement.fadeInTime > 0 ? new Color(textNode.color.r, textNode.color.g, textNode.color.b, 0)
            : new Color(textNode.color.r, textNode.color.g, textNode.color.b, 255);
        timer = currentElement.duration;
    }
}
