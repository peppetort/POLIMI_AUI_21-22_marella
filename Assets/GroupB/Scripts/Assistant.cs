using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AssistantStatus
{
    Idle,
    Talking,
}
public class Assistant : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][Assistant] ";
    public AudioClip characterHelpAudio;
    public AudioClip environmentHelpAudio;

    private AssistantStatus assistantStatus = AssistantStatus.Idle;

    AudioSource audioSource;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (assistantStatus == AssistantStatus.Talking && !audioSource.isPlaying)
        {
            assistantStatus = AssistantStatus.Idle;
            Debug.Log(DEBUG_MARK + assistantStatus);
            animator.enabled = false;
        }

    }

    private AudioClip selectHelperAudio()
    {
        if (MarkerObjectsManager.instantiatedCharacter == null)
            return environmentHelpAudio;
        GameObject instantiatedCharacter = MarkerObjectsManager.instantiatedCharacter;
        Renderer renderer = instantiatedCharacter.GetComponentInChildren<Renderer>();
        if (renderer.isVisible)
        {
            return characterHelpAudio;
        }
        else
        {
            return environmentHelpAudio;
        }
    }

    public void OnMouseDown()
    {
        var helperAudio = selectHelperAudio();
        if (assistantStatus != AssistantStatus.Talking)
        {
            animator.enabled = true;
            animator.Play("Jump");
            audioSource.PlayOneShot(helperAudio);
            assistantStatus = AssistantStatus.Talking;
            Debug.Log(DEBUG_MARK + assistantStatus);
        }

    }
}
