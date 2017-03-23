using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBoxDoor : MonoBehaviour {
    //basically the same as the door script but rotates around z instead of y


    //door is parented to a cube that represents the rotation point
    //cube rotates if door is clicked
    //player can click the door to change the value of opening or closing
    public bool open = false;


    public float speed = 2.0f;
    public float changeAngle = 90; //will usually be either 90 or -90
    private float closedAngle;
    private float openAngle;
    private GameObject rotater;


    // Use this for initialization
    void Start()
    {
        //find rotation point
        rotater = this.gameObject.transform.parent.gameObject;

        //find initial position
        if (open)
        {
            openAngle = transform.rotation.z;
            closedAngle = openAngle + changeAngle;
        }
        else
        {
            closedAngle = transform.rotation.z;
            openAngle = closedAngle + changeAngle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //adjust position of door
        if (open)
        {
            //if door is open/being opened move toward open position
            var target = Quaternion.Euler(0, 0, openAngle);
            rotater.transform.localRotation = Quaternion.Slerp(rotater.transform.localRotation, target, Time.deltaTime * speed);
        }
        if (!open)
        {
            //if door is closing/being closed rotate toward closed position
            var target = Quaternion.Euler(0, 0, closedAngle);
            rotater.transform.localRotation = Quaternion.Slerp(rotater.transform.localRotation, target, Time.deltaTime * speed);
        }
    }
}
