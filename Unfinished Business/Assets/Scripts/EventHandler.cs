using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;
using System.Text;


public class EventHandler : MonoBehaviour {

    //static instance
    public static EventHandler handler;

    //holds a piece of dialogue (dialogue event)
    private class Event
    {
        public int eventID;
        public string mp3Path;
        public List<TextHandler.TextElement> dialogue;
        public bool important;
        public string animatic;
        public float gDelayTime;

        public Event(int id, string mp3, List<TextHandler.TextElement> dia, bool i, string anim, float gdt)
        {
            eventID = id;
            mp3Path = mp3;
            dialogue = dia;
            important = i;
            animatic = anim;
            gDelayTime = gdt;
        }
    };

    //Attributes
    private List<Event> thisSceneEvents;       //holds events that happen in current scene
    private Ghost gScript;                     //ghost script
    private bool playingAnimatic;

    // Use this for initialization
    void Start () {

        gScript = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();

        //populate the initial dialogue
        this.LoadEvents("foyer");
        playingAnimatic = false;
        handler = this;
    }

    //force-stops the current event
    public void ForceStopEvent()
    {
        TextHandler.handler.ClearAllText();
        AudioHandler.handler.ForceStopSound();
        gScript.SkipAction();
        if (playingAnimatic) GameObject.Find("memoryAnimaticGO").GetComponent<MemoryAnimatic>().Stop();
        playingAnimatic = false;
    }

    // Loads dialogue from a file and makes it accessable by the handler
    public void LoadEvents(string path)
    {
        thisSceneEvents = LoadFromFile(path);
    }

    //note to self, add enum for what scene we're in,
    //then add switch statement to load all of the dialogue for that scene
    //and pass as arg to LoadFromFile, so that texthandler only gets what it needs for this scene
    private List<Event> LoadFromFile(string path)
    {
        //open the dialogue file
        TextAsset eventsText = Resources.Load(path) as TextAsset;
        byte[] contents = Encoding.ASCII.GetBytes(eventsText.text);

        MemoryStream str = new MemoryStream(contents);
        StreamReader reader = new StreamReader(str);


        List<Event> inputData = new List<Event>();

        using (reader)
        {
            //start with first line of file
            string line = reader.ReadLine();

            //collect data
            while (!reader.EndOfStream)
            {
                //get ready to push
                int _id = int.Parse(line.Split(' ')[1]);
                string _mp3path = string.Empty;
                string _anim = string.Empty;
                List<TextHandler.TextElement> _lines = new List<TextHandler.TextElement>();
                bool _important = false;
                float _gdelay = 0.0f;

                //get the rest of the data
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    else if (line[0] == 'e') break;

                    string[] lineData = line.Split(new char[] { ' ' }, 2);
                    switch (line[0])
                    {
                        //animatic
                        case 'a':
                        case 'A':
                            _anim = lineData[1];
                            break;

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
                            string[] messageData = lineData[1].Split(new string[] { ",," }, 4, System.StringSplitOptions.None);
                            _lines.Add(new TextHandler.TextElement(messageData[0], float.Parse(messageData[1]), float.Parse(messageData[2]), float.Parse(messageData[3])));
                            break;

                        //parsing ghost delay (float)
                        case 'g':
                        case 'G':
                            _gdelay = float.Parse(lineData[1]);
                            break;

                        //if the identifier doesn't match anything, print an error
                        default:
                            if (line != "") Debug.LogError("Error in File Loading: Line Identifier '" + line[0] + "' not Recognized.");
                            break;
                    }
                }

                //make a new dialogue element with the data
                inputData.Add(new Event(_id, _mp3path, _lines, _important, _anim, _gdelay));
            }
        }

        //close stream and return the data we collected
        reader.Close();
        return inputData;
    }





    //Plays the animatic passed in (FOR NOW)
    //This is not the final resting place of the Animatic handling,
    //will be changed when we update how events in general are handled
    private void PlayAnimatic(string type)
    {
        if (type == string.Empty) return;

        GameObject currentAnimCamera = GameObject.Find(type + "Camera");
        if (currentAnimCamera == null) { Debug.LogError("Couldn't find the camera for " + type); return; }


        //we can play this animatic
        currentAnimCamera.GetComponent<Camera>().enabled = true;

        playingAnimatic = true;
    }

    //called by outside scripts to execute an event that (should) exist in file
    public void TriggerEvent(int eventIn)
    {
        foreach (Event e in thisSceneEvents)
        {
            if (e.eventID == eventIn)
            {
                //play text event
                TextHandler.handler.TextEvent(e.dialogue, e.important);

                //play sound (path, audio node to use, type of sound)
                if(e.mp3Path != string.Empty)
                    AudioHandler.handler.PlaySound(
                        e.mp3Path,
                        GameObject.Find("PlayerAudioNode").GetComponent<AudioSource>(),
                        AudioHandler.SoundType.Dialogue
                        );

                //play animatic, if there is one
                PlayAnimatic(e.animatic);

                //delay ghost and call event
                gScript.ProcessEvent(e.eventID, e.gDelayTime);


                //return early to prevent unneccessary loop iterations
                return;
            }
        }

        //if we get here, that means there isn't an event id that matches in this scene
        Debug.LogError("ERROR: No suitable event id found in this scene!");
    }
}
