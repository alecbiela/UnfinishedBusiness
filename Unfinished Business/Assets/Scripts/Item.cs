using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int itemID;
    public string itemName;
    public bool obtainable;
    public bool available;
    public Sprite uiImage;
    public int availableID;     //the ID to use on me to return true 
    public int unavailableID;   //the ID for attempts to use when the object is 'unavailable'
    public float viewDistance;  //how far away the object will move from the camera in viewing mode
    public string description;  //the description of the item when it shows up in the inventory
    public int putDownID;       //event ID for when the object is put down
    public string activateText; //verb to use when examining (view, open, unlock, etc.)

    private GameManager gm;
    protected string examineText;
    public string ExamineInfo { get { return examineText; } }

    //initializes examine information
    protected virtual void Start()
    {
        if (string.IsNullOrEmpty(activateText)) activateText = "activate";
        if (string.IsNullOrEmpty(itemName)) itemName = "Unknown Object";

        examineText = "Left click to " + activateText  + " " + itemName;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    //activates the item (usually start viewing)
    public virtual void Activate()
    {
        if(available) gm.StartViewingObject(this.gameObject, false);
    }

    //a method which returns true if the item being used on this one
    //is a match (e.x. key to a locked door) or false otherwise
    public virtual bool UseOnMe(int itemID)
    {
        if(available && itemID == availableID)
        {
            EventHandler.handler.TriggerEvent(availableID);
            return true;
        }

        if(!available && itemID == availableID)
        {
            if(this.itemID != -1) available = true;
            EventHandler.handler.TriggerEvent(availableID);
            return true;
        }

        return false;
    }

    //called when the object is put down
    public virtual void PutMeDown()
    {
        if (putDownID <= 0) return;

        EventHandler.handler.TriggerEvent(putDownID);
    }

    //plays the subtitles that are present in the child class (MUST BE OVERRIDDEN)
    protected virtual void PlaySubtitles(){}

    
}
