using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour {
    //enumeration for type of sound
    public enum SoundType
    {
        Dialogue = 0,
        FoleySound = 1,
        AmbientSound = 2,
        Music = 3
    };


    //static instance
    public static AudioHandler handler;

    //attributes
    private AudioClip currentClip;                  //set by the PlaySoundEffect method - the clip that is currently playing
    public AudioClip[] DialogueInteractions;        //dialogue interactions - must be added in the inspector
    private AudioSource currentSource;

	// Use this for initialization
	void Start () {
        handler = this;
	}

    //stops a sound immediately
    public void ForceStopSound()
    {
        if(currentSource != null)
            currentSource.Stop();
    }

    //plays a sound effect
    //takes file name as argument (from texthandler)
    public void PlaySound(string fileName, AudioSource targetAudioSource, SoundType type)
    {
        AudioClip sound;
        //decide what to do based on what sound it's coming from
        //hook this up with sound mixing settings that get set in the main menus
        switch(type)
        {
            case SoundType.Dialogue:
                if ((sound = Resources.Load(fileName) as AudioClip) != null)
                {
                    targetAudioSource.clip = sound;
                }
                else Debug.LogError("Error loading Audio Clip " + fileName);
                break;
            default:
                Debug.LogError("Unhandled Sound Type");
                break;
        }

        //finally, play the sound
        //might change to playoneshot with volume modifier
        currentSource = targetAudioSource;
        targetAudioSource.Play();
    }
}
