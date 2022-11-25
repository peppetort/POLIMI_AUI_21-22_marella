using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

// keep the status of the interaction
enum InteractionStatus
{
    Ready,
    Talking,
    TalkingEnded,
}

public class Character : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG]";

    [SerializeField]
    public GameObject dialog;
    [SerializeField]
    public string storyVideoPath;

    private Animator animator;
    private AudioSource audioSource;
    private VideoPlayer videoPlayer;
    private new Renderer renderer;


    private InteractionStatus interactionStatus = InteractionStatus.Ready;

    void Start()
    {
        DEBUG_MARK = DEBUG_MARK + "[" + gameObject.name + "] ";

        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        renderer = gameObject.GetComponentInChildren<Renderer>();

        Debug.Log(DEBUG_MARK + videoPlayer);

        dialog.SetActive(false);
        dialog = Instantiate(dialog, gameObject.transform);

        var dialogCanvas = dialog.GetComponentInChildren(typeof(Canvas)) as Canvas;
        var camera = GameObject.Find("AR Camera").GetComponent<Camera>();
        dialogCanvas.worldCamera = camera;

        var buttons = dialogCanvas.GetComponentsInChildren<UnityEngine.UI.Button>();

        buttons[0].onClick.AddListener(onYesButtonClickedCallback);
        buttons[1].onClick.AddListener(onNoButtonClickedCallback);
    }

    public void OnMouseDown()
    {
        if (interactionStatus != InteractionStatus.Talking)
            animator.Play("Clicked");
        dialog.SetActive(false);

    }

    void Update()
    {
        if (renderer.isVisible)
        {
            if ((interactionStatus == InteractionStatus.Ready || interactionStatus == InteractionStatus.TalkingEnded) && audioSource.isPlaying)
            {
                interactionStatus = InteractionStatus.Talking;
                Debug.Log(DEBUG_MARK + interactionStatus);
            }
            if (interactionStatus == InteractionStatus.Talking && !audioSource.isPlaying)
            {
                interactionStatus = InteractionStatus.TalkingEnded;
                Debug.Log(DEBUG_MARK + interactionStatus);
                dialog.SetActive(true);
            }
        }
        else
        {
            // reset the interaction state if the object is no visible anymore

            if (dialog.activeSelf)
            {
                dialog.SetActive(false);
            }

            if (interactionStatus != InteractionStatus.Ready)
            {
                interactionStatus = InteractionStatus.Ready;
                Debug.Log(DEBUG_MARK + interactionStatus);
            }

        }

    }

    private void onYesButtonClickedCallback()
    {
        Debug.Log(DEBUG_MARK + "dailog YES button clicked!");
        var videoPath = Path.Combine(Application.streamingAssetsPath, storyVideoPath);

        if (File.Exists(videoPath))
        {
            Handheld.PlayFullScreenMovie("file://" + videoPath);
            Destroy(gameObject);
        }
    }

    private void onNoButtonClickedCallback()
    {
        Debug.Log(DEBUG_MARK + "dailog NO button clicked!");
        dialog.SetActive(false);
    }
}
