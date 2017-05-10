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
    private bool startRun;
    private int startTalkTimer;

    public EventHandler eScript;
    public Animator anim;
    public string state;
    public UVTextureAnim animScript;

	// Use this for initialization
	void Start () {
        startPosition = new Vector3(27.46f, 0.28f, 31.5f);
        transform.Rotate(new Vector3(0.0f, transform.rotation.y + 180.0f, 0.0f));
        speed = 2f;
        rotSpeed = .5f;
        anim = GetComponent<Animator>();
        track = false;
        startRun = false;
        startTalkTimer = 0;
        currentEvent = 0;
        delayTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //start scene

        //ProcessEvent(1, 0);

       
        //StartCoroutine(BeerSequence());

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
        eventPlaying = false;

        //decide what to do
        switch(currentEvent)
        {
            case 1:     //opening scene
                StartCoroutine(StartSequence());
                StartCoroutine(TalkSequence());
                StartCoroutine(BeerSequence());
                break;
            case 2:     //after trying to get beer
                StartCoroutine(TalkSequence());
                break;
            case 3:     //memory animatic
                StartCoroutine(TalkSequence());
                break;
            case 4:     //after memory animatic completed
                StartCoroutine(TalkSequence());
                break;
            case 5:     //Paul's message and conversation after
                StartCoroutine(TalkSequence());
                break;
            case 6:     //Second conversation
                StartCoroutine(TalkSequence());
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

    IEnumerator StartSequence()
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
    }

    IEnumerator TalkSequence()
    {
        yield return (0);

        if (startRun == false)
        {
            yield break;
        }
        else
        {
            SetAnimation("talking");
            track = true;
            startTalkTimer++;

            if (startTalkTimer == 2400)
            {
                SetAnimation("idle");
                track = false;
                startRun = false;
            }
        }
    }
    IEnumerator BeerSequence()
    {
        yield return (TalkSequence());
        //move to beer can
        
        MoveToPosition(new Vector3(24.7f, 0.28f, 34.66f), 7.758f);
        if (transform.position == new Vector3(24.7f, 0.28f, 34.66f))
        {
            SetAnimation("beer");
        }

    }
}
