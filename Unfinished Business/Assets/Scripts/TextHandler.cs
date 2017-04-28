using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A class that controls the text displayed to the UI canvas
public class TextHandler : MonoBehaviour {

    //static instance for easy reference
    public static TextHandler handler;

    //holds all of the "message" arguments
    public class TextElement
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
    private bool playingImportant;
    private float timer;
    private float examineTimer;
    private Text textNode;
    private GameObject bgPane;
    private Text examineTextNode;
    private Queue<TextElement> textQueue;
    private TextElement currentElement;
    private bool displaySubtitles;
    private Toggle toggleSubtitles;

    //getter for important playing flag
    public bool PlayingImportantText
    {
        get { return playingImportant; }
    }

    public bool DisplaySubtitles
    {
        get { return displaySubtitles; }
        set { displaySubtitles = value; }
    }

	// Use this for initialization
	private void Start ()
    {
        //find text ui element
        textNode = GameObject.Find("Text Node").GetComponent<Text>();
        examineTextNode = GameObject.Find("ExamineText").GetComponent<Text>();
        bgPane = GameObject.Find("TextBG");

        //initialize variables
        textQueue = new Queue<TextElement>();
        handler = this;
        timer = 0;
        examineTimer = 0;
        playingImportant = false;
        displaySubtitles = true;
        toggleSubtitles = GameObject.Find("Subtitles").GetComponent<Toggle>();
	}


    // Public methods

    //adds text to the queue to be displayed with fade-in and fade-out
    public void DisplayText(string text, float duration, float fadeInTime, float fadeOutTime)
    {
        //add a new element to the queue to be displayed
        textQueue.Enqueue(new TextElement(text, duration, fadeInTime, fadeOutTime));

        //activate the backdrop
        if(displaySubtitles)bgPane.SetActive(true);
    }

    //initializes text element from file
    //takes an int for event ID (we'll have to keep track of these)
    public void TextEvent(List<TextElement> eventText, bool isImportant)
    {
        ClearAllText();

        //set Texthandler important playing flag, if necessary
        this.playingImportant = isImportant;

        //display the stuff
        foreach (TextElement t in eventText)
        {
            textQueue.Enqueue(t);
        }

        //activate the backdrop
        if(displaySubtitles)bgPane.SetActive(true);
    }

    //immediately removes text from the screen and cancels all waiting text
    public void ClearAllText()
    {
        playingImportant = false;
        timer = 0;
        textQueue.Clear();
    }

    //immediately changes the "Examine Text" to the string passed in
    public void DisplayExamineText(string newText)
    {
        examineTextNode.text = newText;
        examineTimer = 0;
    }

    //starts the examine text timer
    public void StartExamineTimer(float duration)
    {
        examineTimer = duration;
    }

    //clears the examine text from the screen
    public void ClearExamineText()
    {
        examineTextNode.text = "";
        examineTimer = 0;
    }

    //clears the examine text ONLY if there is no timer
    public void ClearExamineTextSafe()
    {
        if (examineTimer <= 0) examineTextNode.text = "";
    }


    // Private (helper) methods

	//update is called once per frame
	private void Update ()
    {

        if (examineTimer > 0) examineTimer -= 1000 * Time.deltaTime;
        else if (examineTimer < 0) ClearExamineText();

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
                playingImportant = false;
                timer = 0;
                textNode.text = "";
                if(displaySubtitles)bgPane.SetActive(false);
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
        if (displaySubtitles) textNode.color = currentElement.fadeInTime > 0 ? new Color(textNode.color.r, textNode.color.g, textNode.color.b, 0)
              : new Color(textNode.color.r, textNode.color.g, textNode.color.b, 255);
        else textNode.color = new Color(textNode.color.r, textNode.color.g, textNode.color.b, 0);
        timer = currentElement.duration;
    }


    public void ToggleSubtitles()
    {
        //textNode.color = new Color(textNode.color.r, textNode.color.g, textNode.color.b, toggleSubtitles.isOn ? 1 : 0);
        displaySubtitles = toggleSubtitles.isOn;
        //Debug.Log(displaySubtitles);
        bgPane.SetActive(displaySubtitles);
    }


}   //end TextHandler.cs
