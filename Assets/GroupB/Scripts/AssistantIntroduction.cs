using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AudioStatus
{
    Start, // introduction to the new world
    Environemnt, // explanation of which animals the kid can interact with
    Characters, // explanation of the switch camera button
    Filters, // explanation of how the filters work
    End, // There are not more info
}

public class AssistantIntroduction : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][AssistantIntroduction] ";
    public AudioClip introAudio;
    public AudioClip environmentAudio;
    public AudioClip characterAudio;
    public AudioClip filterAudio;
    public AudioClip endAudio;

    public GameObject characters;

    AudioSource audioSource;

    private AudioStatus audioStatus = AudioStatus.Start;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(introAudio);
        animator.Play("Idle");
        Debug.Log(DEBUG_MARK + audioStatus);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            switch (audioStatus)
            {
                case AudioStatus.Start:
                    audioSource.PlayOneShot(environmentAudio);
                    audioStatus = AudioStatus.Environemnt;
                    Debug.Log(DEBUG_MARK + audioStatus);
                    break;
                case AudioStatus.Environemnt:
                    characters.SetActive(true);
                    audioSource.PlayOneShot(characterAudio);
                    audioStatus = AudioStatus.Characters;
                    Debug.Log(DEBUG_MARK + audioStatus);
                    break;
                case AudioStatus.Characters:
                    characters.SetActive(false);
                    audioSource.PlayOneShot(filterAudio);
                    audioStatus = AudioStatus.Filters;
                    Debug.Log(DEBUG_MARK + audioStatus);
                    break;
                case AudioStatus.Filters:
                    audioSource.PlayOneShot(endAudio);
                    audioStatus = AudioStatus.End;
                    Debug.Log(DEBUG_MARK + audioStatus);
                    break;
                    //TODO: case AudioStatus.End dialog 
            }
        }

    }
}
