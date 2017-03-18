using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int itemID;
    public string itemName;
    public bool obtainable;
    public Sprite uiImage;
    public int reagentID;  //the ID to use on me to return true

    //a method which returns true if the item being used on this one
    //is a match (e.x. key to a locked door) or false otherwise
    public bool UseOnMe(int itemID)
    {
        return (itemID == reagentID);
    }
}
