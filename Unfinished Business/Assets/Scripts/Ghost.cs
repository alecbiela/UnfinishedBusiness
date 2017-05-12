using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    //event data
    private int currentEvent;
    private float delayTime;
    private bool eventPlaying;

    private Vector3 startPosition;
    private float speed;
    private float rotSpeed;
    private bool track;
    private bool talking;
    private bool idle;
    private bool beer;
    private float startTalkTimer;
    private string state;

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
        startTalkTimer = 0;
        currentEvent = 0;
        delayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //start scene
        ProcessEvent(1, 0);

        //if event is playing, decrement timer, and if timer is 0 call DoEventAction()
        if (eventPlaying)
        {
            delayTime -= (1000 * Time.deltaTime);
            if (delayTime <= 0) DoEventAction();
        }
       
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
                TalkSequence1();
                //StartCoroutine(IdleSequence(3.1f, 10f));
                break;
            case 2:     //after trying to get beer
                //StartCoroutine(TalkSequence());
                break;
            case 3:     //memory animatic
                //StartCoroutine(TalkSequence());
                break;
            case 4:     //after memory animatic completed
                //StartCoroutine(TalkSequence());
                break;
            case 5:     //Paul's message and conversation after
                //StartCoroutine(TalkSequence());
                break;
            case 6:     //Second conversation
                //StartCoroutine(TalkSequence());
                break;
        }
    }

    //forces the end of the current action
    public void SkipAction()
    {
        //stub
    }

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

    //sets animation states, call when you need to change and set the state to "idle, "talking" or "beer"
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

    void TalkSequence1()
    {
        startTalkTimer += 1 * Time.deltaTime;

        if(startTalkTimer < 3)
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking");
                talking = true;
            }   
        }
        else if (startTalkTimer >= 3 && startTalkTimer < 25) //dear sir or madamn
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTalkTimer >= 25 && startTalkTimer < 31) //milk cahton
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking");
                talking = true;
            }
        }
        else if (startTalkTimer >= 31 && startTalkTimer < 40.5) //living challegned
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }

        }
        else if (startTalkTimer >= 40.5 && startTalkTimer < 44) //get outta mah house
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking");
                talking = true;
            }

        }
        else if (startTalkTimer >= 44 && startTalkTimer < 55) //never works anyway
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTalkTimer >= 55 && startTalkTimer < 57.5) //how you know mah name
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking");
                talking = true;
            }

        }
        else if (startTalkTimer >= 57.75 && startTalkTimer < 67) //file on desk
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }
        }
        else if (startTalkTimer >= 67 && startTalkTimer < 74) //came home
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking");
                talking = true;
            }

        }
        else if (startTalkTimer >= 74 && startTalkTimer < 80) //sounds about right
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }

        }
        else if (startTalkTimer >= 80 && startTalkTimer < 83) //not a ghost, aint afraid of no ghost
        {
            idle = false;
            if (!talking)
            {
                SetAnimation("Talking");
                talking = true;
            }

        }
        else if (startTalkTimer >= 83 && startTalkTimer < 89) //pick up that can
        {
            talking = false;
            if (!idle)
            {
                SetAnimation("Idle 1");
                idle = true;
            }

        }
        else if (startTalkTimer >= 89) //fine!
        {
            track = false;
            talking = false;
            idle = false;

            MoveToPosition(new Vector3(24.7f, 0.28f, 34.66f), 7.758f);
            if (transform.position == new Vector3(24.7f, 0.28f, 34.66f) && !beer)
            {
                SetAnimation("Beer");
                beer = true;
            }
        }
    }
}
