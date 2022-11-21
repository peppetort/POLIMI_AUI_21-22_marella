using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// keep the status of the interaction
enum InteractionStatus
{
    Ready,
    Talking,
    ShowingVideo,
    TalkingEnded,
    VideoEnded,
}

public class Character : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG]";

    private Animator animator;
    private AudioSource audioSource;
    private GameObject instance;
    private new Renderer renderer;

    private InteractionStatus interactionStatus = InteractionStatus.Ready;

    void Start()
    {
        DEBUG_MARK = DEBUG_MARK + "[" + gameObject.name + "] ";

        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        renderer = gameObject.GetComponentInChildren<Renderer>();
    }

    public void OnMouseDown()
    {
        if (interactionStatus != InteractionStatus.Talking)
            animator.Play("Clicked");
    }

    void Update()
    {
        if (renderer.isVisible)
        {
            if ((interactionStatus == InteractionStatus.Ready || interactionStatus == InteractionStatus.TalkingEnded) && audioSource.isPlaying)
            {
                // dialog.enabled = false;
                interactionStatus = InteractionStatus.Talking;
                Debug.Log(DEBUG_MARK + interactionStatus);
            }
            if (interactionStatus == InteractionStatus.Talking && !audioSource.isPlaying)
            {
                interactionStatus = InteractionStatus.TalkingEnded;
                Debug.Log(DEBUG_MARK + interactionStatus);
                //dialog.enabled = true;
            }
        }
        else
        {
            // reset the interaction state if the object is no visible anymore
            if (interactionStatus != InteractionStatus.Ready)
            {
                interactionStatus = InteractionStatus.Ready;
                Debug.Log(DEBUG_MARK + interactionStatus);
            }

        }
    }
}
