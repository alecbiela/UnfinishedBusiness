using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO SCRIPT AN ITEM W/ SUBTITLES
// 1. COPY (don't edit) THIS SCRIPT AND RENAME THE FILE
// 2. ADD YOUR SUBTITLES IN PlaySubtitles()
// 3. ADD AS A COMPONENT TO THE OBJECT
// 4. COPY OVER THE VALUES FROM THE ORIGINAL ITEM SCRIPT IN THE INSPECTOR
// 5. REMOVE THE OLD ITEM SCRIPT FROM THE OBJECT

    //NOTE: You should be creating this script to go on the item that you use the inventory item on
    //      For example, if you had a key and a door, this script would go on the door.

public class SpecificItem : Item {


    //plays the subtitles of this item
    //may also add the playing of audio (dialogue) here later
    protected override void PlaySubtitles()
    {
        TextHandler.handler.ClearAllText();

        //copy+paste this statement for as many subtitles as you need
        TextHandler.handler.DisplayText("", 0, 0, 0);   //text to display, duration, fadeIn(0 for now), fadeOut(0 for now)
    }
}
