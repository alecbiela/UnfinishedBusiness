using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

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

    public EventHandler eScript;
    public Animator anim;
    public string state;
    public UVTextureAnim animScript;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        transform.Rotate(new Vector3(0.0f, transform.rotation.y + 180.0f, 0.0f));
        speed = 2f;
        rotSpeed = .5f;
        anim = GetComponent<Animator>();
        track = true;
        idle = false;
        talking = true;
        beer = false;
        startTalkTimer = 0;
        currentEvent = 0;
        delayTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
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
        switch(currentEvent)
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

        /*float elapsedTime = 0;
        startPosition = transform.position;
        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPosition, pos, (elapsedTime/timeToMove));
            elapsedTime += Time.deltaTime;
        }
        transform.position = pos;
        t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(startPosition, pos, t);
        }*/
    }

    //sets animation states, call when you need to change and set the state to "idle, "talking" or "beer"
    void SetAnimation(string animState)
    {
        if (animState == "talking")
        {
            anim.Play("Talking");
            animScript.talking = true;
        }
        if (animState == "beer")
        {
            anim.Play("Beer");
        }
        if(animState == "idle")
        {
            anim.Play("Idle 1");
            animScript.talking = false;
        }
    }

    //processes event with delay
    public void ProcessEvent(int eventID, float delay)
    {
        //set the variables
        currentEvent = eventID;
        delayTime = delay;
        eventPlaying = true;
    }

    /*IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(1);

        if (startRun)
        {
            yield break;
        }
        else
        {
            //rotates to face player
            track = true;
            MoveToPosition(transform.position, transform.rotation.y);
            MoveToPosition(new Vector3(24.61f, .28f, 28.39f), transform.rotation.y);
            //Moves to spot infront of player
            if (transform.position == new Vector3(24.61f, .28f, 28.39f))
            {
                startRun = true;
            }

        }
    }*/

    void TalkSequence1()
    {
        startTalkTimer = Time.time;
        if (talking)
        {
            SetAnimation("talking");
        }
        if (idle)
        {
            SetAnimation("idle");
        }
        if (beer)
        {
            track = false;
            MoveToPosition(new Vector3(24.7f, 0.28f, 34.66f), 7.758f);
            if (transform.position == new Vector3(24.7f, 0.28f, 34.66f))
            {
                SetAnimation("beer");
            }
        }


        if (startTalkTimer >= 3 && startTalkTimer < 25) //dear sir or madamn
        {
            talking = false;
            idle = true;
        }
        else if (startTalkTimer >= 25 && startTalkTimer < 31) //milk cahton
        {
            talking = true;
            idle = false;
        }
        else if (startTalkTimer >= 31 && startTalkTimer < 40.5) //living challegned
        {
            talking = false;
            idle = true;
        }
        else if (startTalkTimer >= 40.5 && startTalkTimer < 44) //get outta mah house
        {
            talking = true;
            idle = false;
        }
        else if (startTalkTimer >= 44 && startTalkTimer < 55) //never works anyway
        {
            talking = false;
            idle = true;
        }
        else if (startTalkTimer >= 55 && startTalkTimer < 57.5) //how you know mah name
        {
            talking = true;
            idle = false;
        }
        else if (startTalkTimer >= 57.75 && startTalkTimer < 67) //file on desk
        {
            talking = false;
            idle = true;
        }
        else if (startTalkTimer >= 67 && startTalkTimer < 74) //came home
        {
            talking = true;
            idle = false;
        }
        else if (startTalkTimer >= 74 && startTalkTimer < 80) //sounds about right
        {
            talking = false;
            idle = true;
        }
        else if (startTalkTimer >= 80 && startTalkTimer < 83) //not a ghost, aint afraid of no ghost
        {
            talking = true;
            idle = false;
        }
        else if (startTalkTimer >= 83 && startTalkTimer < 89) //pick up that can
        {
            talking = false;
            idle = true;
        }
        else if (startTalkTimer >= 89 && startTalkTimer < 90) //fine!
        {
            track = false;
            talking = false;
            idle = false;
            beer = true;
        }
    }


    IEnumerator IdleSequence(float delay, float timer)
    {
        yield break;

        if (idle == false)
        {
            yield break;
        }
        else
        {
            track = true;
            startTalkTimer = Time.time;
            SetAnimation("idle");
            if (startTalkTimer >= timer)
            {
                idle = false;
            }
        }
    }

    IEnumerator BeerSequence()
    {
        yield return (0);
        //move to beer can
        
        MoveToPosition(new Vector3(24.7f, 0.28f, 34.66f), 7.758f);
        if (transform.position == new Vector3(24.7f, 0.28f, 34.66f))
        {
            SetAnimation("beer");
        }

    }
}
