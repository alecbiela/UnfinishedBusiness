using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

    //holds a piece of dialogue (dialogue event)
    private class Dialogue
    {
        public int eventID;
        public string mp3Path;
        public List<TextElement> dialogue;
        public bool important;

        public Dialogue(int id, string mp3, List<TextElement> dia, bool i)
        {
            eventID = id;
            mp3Path = mp3;
            dialogue = dia;
            important = i;
        }
    };

    //private attributes
    private bool playingImportant;
    private float timer;
    private float examineTimer;
    private Text textNode;
    private Text examineTextNode;
    private Queue<TextElement> textQueue;
    private List<Dialogue> thisSceneDialogue;
    TextElement currentElement;

    //getter for important playing flag
    public bool PlayingImportantText
    {
        get { return playingImportant; }
    }


	// Use this for initialization
	private void Start ()
    {
        //populate the initial dialogue
        this.LoadDialogue(Application.dataPath + "/Dialogue/foyer.txt");

        //find text ui element
        textNode = GameObject.Find("Text Node").GetComponent<Text>();
        examineTextNode = GameObject.Find("ExamineText").GetComponent<Text>();

        //initialize variables
        textQueue = new Queue<TextElement>();
        handler = this;
        timer = 0;
        examineTimer = 0;
        playingImportant = false;
	}


    // Public methods

    //adds text to the queue to be displayed with fade-in and fade-out
    public void DisplayText(string text, float duration, float fadeInTime, float fadeOutTime)
    {
        //add a new element to the queue to be displayed
        textQueue.Enqueue(new TextElement(text, duration, fadeInTime, fadeOutTime));
    }

    //initializes text element from file
    //takes an int for event ID (we'll have to keep track of these)
    public void TriggerTextEvent(int eventID)
    {
        foreach(Dialogue d in thisSceneDialogue)
        {
            if(d.eventID == eventID)
            {
                ClearAllText();

                //set Texthandler important playing flag, if necessary
                this.playingImportant = d.important;

                //display the stuff
                foreach(TextElement t in d.dialogue)
                {
                    textQueue.Enqueue(t);
                }

                //code to play mp3 begins

                return;
            }
        }

        //if we get here, that means there isn't an event id that matches in this scene
        Debug.LogError("ERROR: No suitable event id found in this scene!");
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

    // Loads dialogue from a file and makes it accessable by the handler
    public void LoadDialogue(string path)
    {
        thisSceneDialogue = LoadFromFile(path);
    }



    //note to self, add enum for what scene we're in,
    //then add switch statement to load all of the dialogue for that scene
    //and pass as arg to LoadFromFile, so that texthandler only gets what it needs for this scene
    private List<Dialogue> LoadFromFile(string path)
    {
        //open the dialogue file
        StreamReader reader = new StreamReader(path, System.Text.Encoding.ASCII);

        List<Dialogue> inputData = new List<Dialogue>();

        using (reader)
        {
            //start with first line of file
            string line = reader.ReadLine();

            //collect data
            while (!reader.EndOfStream)
            {
                //get ready to push
                int _id = int.Parse(line.Split(' ')[1]);
                string _mp3path = "";
                List<TextElement> _lines = new List<TextElement>();
                bool _important = false;

                //get the rest of the data
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    else if (line[0] == 'e') break;

                    string[] lineData = line.Split(new char [] { ' ' }, 2);
                    switch (line[0])
                    {
                        //mp3 path
                        case 'm':
                        case 'M':
                            _mp3path = lineData[1];
                            break;

                        //the "important" flag
                        case 'i':
                        case 'I':
                            _important = (lineData[1] == "true");
                            break;

                        //parsing dialogue
                        case 'd':
                        case 'D':
                            string[] messageData = lineData[1].Split(new string[] { ",," }, 4 , System.StringSplitOptions.None);
                            _lines.Add(new TextElement(messageData[0], float.Parse(messageData[1]), float.Parse(messageData[2]), float.Parse(messageData[3])));
                            break;

                        //if the identifier doesn't match anything, print an error
                        default:
                            if(line != "") Debug.LogError("Error in File Loading: Line Identifier not Recognized.");
                            break;
                    }
                }

                //make a new dialogue element with the data
                inputData.Add(new Dialogue(_id, _mp3path, _lines, _important));
            }
        }

        //close stream and return the data we collected
        reader.Close();
        return inputData;
    }
}
