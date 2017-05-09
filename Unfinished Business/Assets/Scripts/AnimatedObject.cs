using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents ANY object which can be animated, with adjustments defined in inspector
public class AnimatedObject : MonoBehaviour {

    //Basic Attributes
    public Vector3 Translation;
    public Vector3 Rotation;
    public Vector3 AxisOfRotation;
    public GameObject PivotPoint;
    public float AnimationDuration;

    private float _timer;               //used for internal timing of animations
    private bool _animating;            //set when animating so that we cannot "double tap"
    private bool _atRest;               //"at rest" meaning in the state which it begins, should be true by default
    private Vector3 _translationStep;   //How much we want to translate each tick
    private float _rotationStep;        // ^ but with rotation

    public bool Activated
    {
        get { return _atRest; }
    }

	// Use this for initialization
	void Start () {
        _atRest = true;
        _animating = false;
        _timer = 0;

        //check for bad values
        if (Rotation != Vector3.zero && PivotPoint == null)
        {
            Debug.LogError(this.gameObject.name + " has nothing to rotate around, setting to its own pivot point.");
            PivotPoint = this.gameObject;
        }

        if (Rotation != Vector3.zero && AxisOfRotation == Vector3.zero)
        {
            Debug.LogError(this.gameObject.name + " has nothing to rotate around, setting to Vector3.up.");
            AxisOfRotation = Vector3.up;
        }

        //Normalize AOR
        AxisOfRotation.Normalize();

        //calculate step
        float durationSq = AnimationDuration * AnimationDuration;
        _translationStep = Translation / AnimationDuration;
        _rotationStep = Rotation.magnitude / AnimationDuration;
    }
	


    //called once per physics tick
    void FixedUpdate()
    {
        //animate if we need to
        if(_animating)
        {
            if (Translation != Vector3.zero) TranslateObject();
            if (Rotation != Vector3.zero) RotateObject();

            _timer -= (Time.fixedDeltaTime * 1000);

            //done animating
            if (_timer <= 0)
            {
                _animating = false;
                _atRest = !_atRest;
            }
        }
    }

    //submethod for rotation
    private void RotateObject()
    {
        this.transform.RotateAround(PivotPoint.transform.position,
            AxisOfRotation,
            (_atRest ? _rotationStep : _rotationStep * -1)
            * Time.fixedDeltaTime * 1000);

        //maybe add: check if accidentally rotated over desired amount?
    }

    //submethod for translation
    private void TranslateObject()
    {
        //probably should move the rigidbody, but just moving the object for now
        //this.gameObject.GetComponent<Rigidbody>().position += (_atRest ? _translationStep : _translationStep * -1) * Time.fixedDeltaTime * 1000;
        
        this.transform.position += (_atRest ? _translationStep : _translationStep * -1) * Time.fixedDeltaTime * 1000;
    }

    // Method which can be called to trigger animation
    public void Animate()
    {
        if(!_animating)
        {
            _animating = true;
            _timer = AnimationDuration;
        }
    }
}
