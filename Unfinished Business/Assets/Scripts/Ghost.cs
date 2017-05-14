using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    //event data
    private int currentEvent;
    private float delayTime;
    private bool eventPlaying;

    //ghost stats
    private Vector3 startPosition;
    private float speed, rotSpeed;

    //ghost anim check states
    private bool track, talking, idle, beer;

    //ghost event checks
    private double startTimer, endTime;
    private bool skip, startEvent;

    public EventHandler eScript;
    public Animator anim;
    public UVTextureAnim animScript;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        transform.Rotate(new Vector3(0.0f, transform.rotation.y + 180.0f, 0.0f));
        speed = 2f;
        rotSpeed = .5f;
        anim = GetComponent<Animator>();
        track = true;
        idle = true;
        talking = false;
        beer = false;
        skip = false;
        startTimer = 0;
        currentEvent = 0;
        delayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if event is playing, decrement timer, and if timer is 0 call DoEventAction()
        if (eventPlaying)
        {
            delayTime -= (1000 * Time.deltaTime);
            if (delayTime <= 0) DoEventAction();
        }

        //rotates the ghost to follow player if applicable
        TrackPlayer();
    }

    //performs the action specific to the event ID
    private void DoEventAction()
    {
        //zero the timer and stop running it
        delayTime = 0;
        //eventPlaying = false;
        //decide what to do
        switch (currentEvent)
        {
            case 1:     //opening scene
                //if its the first time, resets start timer
                OpeningSequence_1();
                break;
            case 2:     //after trying to get beer
                OpeningSequence_2();
                break;
            case 3:     //memory animatic
                break;
            case 4:     //after memory animatic completed
                break;
            case 5:     //Paul's message and conversation after
                break;
            case 6:     //Second conversation
                break;
        }
    }

    //forces the end of the current action
    public void SkipAction()
    {
        if(skip == false)
        {
            skip = true;
        }
    }

    //ghost tracks player
    void TrackPlayer()
    {
        if (track)
        {
            Vector3 targetPostition = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            transform.LookAt(targetPostition);
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + 180.0f, transform.rotation.z));
        }
    }

    //Moves ghost and sets desired rotation(ALL ROTATIONS CURRENTLY ADD 180.0F TO IT DON'T KNOW HOW TO FIX)
    void MoveToPosition(Vector3 endPos, float yRot)
    {

        startPosition = transform.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, yRot + 180.0f, 0.0f), Time.time * rotSpeed);
        transform.position = Vector3.MoveTowards(startPosition, endPos, speed * Time.deltaTime);
    }

    //sets animation states
    void SetAnimation(string animState)
    {
        anim.CrossFade(animState, .3f);
    }

    //processes event with delay
    public void ProcessEvent(int eventID, float delay)
    {
        //set the variables
        currentEvent = eventID;
        delayTime = delay;
        eventPlaying = true;
    }

    //runs opening sequence ends at beer pickup
    void OpeningSequence_1()
    {
        //skips if needed
        if (skip)
        {
            startTimer = endTime;
            skip = false;
        }
        //if its the first time, resets start timer
        if (startEvent)
        {
            startTimer = 0;
            startEvent = false;
        }

        //iterates through time
        startTimer += 1 * Time.deltaTime;
        endTime = 95.952;


        if(startTimer < 3.350)
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Short");
                talking = true;
            }   
        }
        else if (startTimer >= 3.350 && startTimer < 25.52) //dear sir or madamn
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTimer >= 25.52 && startTimer < 31.36) //milk cahton
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Long");
                talking = true;
            }
        }
        else if (startTimer >= 31.36 && startTimer < 40.57) //living challegned
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }

        }
        else if (startTimer >= 40.57 && startTimer < 44.02) //get outta mah house
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Long");
                talking = true;
            }

        }
        else if (startTimer >= 44.02 && startTimer < 55.61) //never works anyway
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTimer >= 55.61 && startTimer < 57.73) //how you know mah name
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Short");
                talking = true;
            }

        }
        else if (startTimer >= 57.73 && startTimer < 67.537) //file on desk
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTimer >= 67.537 && startTimer < 74.527) //came home
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Long");
                talking = true;
            }

        }
        else if (startTimer >= 74.527 && startTimer < 80.217) //sounds about right
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }

        }
        else if (startTimer >= 80.217 && startTimer < 83.467) //not a ghost, aint afraid of no ghost
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Short");
                talking = true;
            }

        }
        else if (startTimer >= 83.467 && startTimer < 88.922) //pick up that can
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }

        }
        else if (startTimer >= 88.922) //fine!
        {
            track = false;
            talking = false;
            idle = false;

            MoveToPosition(new Vector3(24.7f, 0.28f, 34.66f), 7.758f);
            if (transform.position == new Vector3(24.7f, 0.28f, 34.66f) && !beer)
            {
                SetAnimation("Beer Long");
                beer = true;
            }
        }
    }

    //sequence after beer pickup
    void OpeningSequence_2()
    {
        //skips if needed
        if (skip)
        {
            startTimer = endTime;
            skip = false;
        }
        //if its the first time, resets start timer
        if (startEvent)
        {
            startTimer = 0;
            startEvent = false;
        }

        //iterates through time
        startTimer += 1 * Time.deltaTime;
        endTime = 20.455;

        if (startTimer < 1.277) 
        {
            track = true;
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Short");
                talking = true;
            }
        }
        else if (startTimer >= 1.277 && startTimer < 3.204)
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTimer >= 3.204 && startTimer < 6.443)
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Short");
                talking = true;
            }
        }
        else if (startTimer >= 6.443 && startTimer < 14.836)
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTimer >= 14.836 && startTimer < 19.155)
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking Short");
                talking = true;
            }
        }
        else if (startTimer >= 19.155 && startTimer < 20.455)
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
                Debug.Log(startTimer);
            }
        }
    }
}
