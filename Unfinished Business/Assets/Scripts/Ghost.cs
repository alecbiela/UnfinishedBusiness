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
    private bool skip, startEvent, waitForPlayer;
    private int currentStage;

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
            if (startEvent == false)
            {
                startEvent = true;
                skip = false;
            }
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
                TalkSequence(currentEvent);
                break;
            case 2:     //after trying to get beer
                waitForPlayer = false;
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 3:     //memory animatic
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 4:     //after memory animatic completed
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 5:     //Paul's message and conversation after
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 6:     //Second conversation
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 7:     //wedding photo
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 8:     //car doc
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 9:     //stuck drawer open
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 10:     //stuck drawer closed
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 11:     //grad photo
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 12:     //resturaunt opening
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 13:     //recipe
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 14:     //news review
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 15:     //parents letter
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 16:     //crowbar
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 17:     //contract
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
            case 18:     //acting
                if (startTimer != 0 && currentStage != currentEvent)
                {
                    startTimer = 0.0f;
                }
                TalkSequence(currentEvent);
                break;
        }
    }

    //forces the end of the current action
    public void SkipAction()
    {
        if (skip == false)
        {
            startTimer = endTime;
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

    //sequences for talking parts
    void TalkSequence(int stage)
    {
        switch (stage)
        {
            case 1:     //opening scene
                currentStage = stage;

                startTimer += 1 * Time.deltaTime;
                endTime = 95.952;
                if (startTimer < 3.350)
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
                        if (waitForPlayer != true)
                        {
                            return;
                        }
                    }
                }
                break;
            case 2:     //after trying to get beer
                currentStage = stage;

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
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 3:     //memory animatic
               
                break;
            case 4:     //after memory animatic completed
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 21.365;

                if (startTimer < 2.550)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 2.550 && startTimer < 16.955)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 16.955 && startTimer < 18.905)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 18.905 && startTimer < 21.365)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 5:     //Paul's message and conversation after
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 28.447;

                if (startTimer < 15.50)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 15.50 && startTimer < 20.957)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 20.957 && startTimer < 26.507)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 26.507 && startTimer < 28.447)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 6:     //Second conversation
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 13.215;

                if (startTimer < 3.280)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 3.280 && startTimer < 7.83)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 7.83 && startTimer < 10.965)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 10.965 && startTimer < 13.215)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 7:     //wedding photo
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 13.329;

                if (startTimer < 2.392)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 2.392 && startTimer < 7.767)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 7.767 && startTimer < 12.574)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 12.574 && startTimer < 13.329)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 8:     //car doc
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 10.193;

                if (startTimer < 1.950)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 1.950 && startTimer < 5.630)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 5.630 && startTimer < 10.193)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 9:     //stuck drawer open
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 5.143;

                if (startTimer < 1.869)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 1.869 && startTimer < 5.143)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 10:     //stuck drawer closed
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 13.797;

                if (startTimer < 3.773)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 3.773 && startTimer < 11.324)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 11.324 && startTimer < 13.797)
                {
                    talking = false;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 11:     //grad photo
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 9.578;

                if (startTimer < 2.287)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 2.287 && startTimer < 4.458)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 4.458 && startTimer < 7.987)
                {
                    talking = false;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 7.987 && startTimer < 9.578)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 12:     //resturaunt opening
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 11.239;

                if (startTimer < 5.341)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 5.341 && startTimer < 9.567)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 9.567 && startTimer < 11.239)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 13:     //recipe
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 9.8;

                if (startTimer < 3.228)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 3.228 && startTimer < 6.177)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 6.177 && startTimer < 8.395)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 8.395 && startTimer < 9.8)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 14:     //news review
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 8.847;

                if (startTimer < 3.843)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 3.843 && startTimer < 8.847)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 15:     //parents letter
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 8.519;

                if (startTimer < 3.158)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 3.158 && startTimer < 8.519)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 16:     //crowbar
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 9.265;

                if (startTimer < 2.125)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                else if (startTimer >= 2.125 && startTimer < 6.525)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 6.525 && startTimer < 9.265)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Short");
                        talking = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 17:     //contract
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 16.881;

                if (startTimer < 9.381)
                {
                    track = true;
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 9.381 && startTimer < 11.61)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 11.61 && startTimer < 15.685)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 15.685 && startTimer < 16.881)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
            case 18:     //acting
                currentStage = stage;

                //iterates through time
                startTimer += 1 * Time.deltaTime;
                endTime = 6.374;

                if (startTimer < 2.032)
                {
                    track = true;
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                else if (startTimer >= 2.032 && startTimer < 5.213)
                {
                    idle = false;
                    if (!talking)
                    {
                        SetAnimation("Talking Long");
                        talking = true;
                    }
                }
                else if (startTimer >= 5.213 && startTimer < 6.374)
                {
                    talking = false;
                    if (!idle)
                    {
                        SetAnimation("Idle 1");
                        idle = true;
                    }
                }
                if (startTimer == endTime)
                {
                    return;
                }
                break;
        }
    }
}

 