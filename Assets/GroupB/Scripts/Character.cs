using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Video;

// keep the status of the interaction
public enum InteractionStatus
{
    Ready,
    Talking,
    TalkingEnded,
}

/*
    manage the status of the character, is attached to each character gameobject
*/
public class Character : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG]";

    // reference to dialog object that let the user to choose id
    // hear a story or not
    [SerializeField]
    public GameObject dialog;
    // name of the video associated to the character
    [SerializeField]
    public string storyVideoPath;

    private Animator animator;
    private AudioSource audioSource;
    private VideoPlayer videoPlayer;
    private new Renderer renderer;


    public InteractionStatus interactionStatus = InteractionStatus.Ready;

    void Start()
    {
        DEBUG_MARK = DEBUG_MARK + "[" + gameObject.name + "] ";

        // check if the video has already been extracted from the app package
        // otherwhise extract it and copy to the folder of the app
        //
        // that's because is not allowed to access resurces into the compressed package
        var videoPath = Path.Combine(Application.persistentDataPath, storyVideoPath);
        if (!File.Exists(videoPath))
        {
            string fileURL = "jar:file://" + Application.dataPath + "!/assets/" + storyVideoPath;
            StartCoroutine(CopyMP4File(fileURL));
        }

        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        renderer = gameObject.GetComponentInChildren<Renderer>();

        Debug.Log(DEBUG_MARK + videoPlayer);

        dialog.SetActive(false);

        // palce and adjust the position of the dialog 
        // relative to the character one
        dialog = Instantiate(dialog, gameObject.transform);
        var z = renderer.bounds.size.z;
        dialog.transform.Translate(0, -0.02f, z * 0.7f);

        var dialogCanvas = dialog.GetComponentInChildren(typeof(Canvas)) as Canvas;
        var camera = GameObject.Find("AR Camera").GetComponent<Camera>();

        // set the camera of the dialog in orther to enable raycast
        dialogCanvas.worldCamera = camera;

        var buttons = dialogCanvas.GetComponentsInChildren<UnityEngine.UI.Button>();

        // add listener to buttons of the dialog
        buttons[0].onClick.AddListener(onYesButtonClickedCallback);
        buttons[1].onClick.AddListener(onNoButtonClickedCallback);
    }

    // copy file from compresed package to persistent data folder
    private IEnumerator CopyMP4File(string fileURL)
    {
        Debug.Log(DEBUG_MARK + " coping video from " + fileURL);
        WWW www = new WWW(fileURL);
        yield return www;
        string targetFile = Application.persistentDataPath + "/" + storyVideoPath;
        using (BinaryWriter writer = new BinaryWriter(File.Open(targetFile, FileMode.Create)))
        {
            writer.Write(www.bytes);
        }
        Debug.Log(DEBUG_MARK + " copied video to " + targetFile);
    }

    public void OnMouseDown()
    {
        // start animation when clicked
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
        interactionStatus = InteractionStatus.Ready;
        dialog.SetActive(false);

        var videoPath = Path.Combine(Application.persistentDataPath, storyVideoPath);
        print(DEBUG_MARK + videoPath);

        if (File.Exists(videoPath))
        {
            // play video in full screen using the device built-in player
            Handheld.PlayFullScreenMovie("file://" + videoPath, Color.black, FullScreenMovieControlMode.Full);
            Debug.Log(DEBUG_MARK + "Video playback completed.");
        }
        else
        {
            Debug.Log(DEBUG_MARK + "FILE NOT EXIST!");
        }
    }

    private void onNoButtonClickedCallback()
    {
        // reset the status
        Debug.Log(DEBUG_MARK + "dailog NO button clicked!");
        interactionStatus = InteractionStatus.Ready;
        dialog.SetActive(false);
    }
}
