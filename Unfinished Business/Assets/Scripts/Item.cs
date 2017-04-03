using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int itemID;
    public string itemName;
    public bool obtainable;
    public Sprite uiImage;
    public int reagentID;  //the ID to use on me to return true 
    public float viewDistance; //how far away the object will move from the camera in viewing mode

    //a method which returns true if the item being used on this one
    //is a match (e.x. key to a locked door) or false otherwise
    public virtual bool UseOnMe(int itemID)
    {
        if(itemID == reagentID)
        {
            PlaySubtitles();
            return true;
        }

        return false;
    }

    //plays the subtitles that are present in the child class (MUST BE OVERRIDDEN)
    protected virtual void PlaySubtitles(){}
}
