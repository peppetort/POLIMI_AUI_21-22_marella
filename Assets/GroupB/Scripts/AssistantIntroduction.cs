using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AudioStatus
{
    Start,
    Intro, // introduction to the new world
    Environemnt, // explanation of which animals the kid can interact with
    Characters, // explanation of the switch camera button
    Filters, // explanation of how the filters work
    End, // There are not more info
}

[System.Serializable]
public class AssistantIntroText
{
    public string start;
    public string intro;
    public string environment;
    public string character;
    public string filters;
    public string end;

    public static AssistantIntroText CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AssistantIntroText>(jsonString);
    }

    public string getTextFromAudioStatus(AudioStatus status)
    {
        switch (status)
        {
            case AudioStatus.Start:
                return start;
            case AudioStatus.Intro:
                return intro;
            case AudioStatus.Environemnt:
                return environment;
            case AudioStatus.Characters:
                return character;
            case AudioStatus.Filters:
                return filters;
            case AudioStatus.End:
                return end;
        }
        return "";
    }
}

public class AssistantIntroduction : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][AssistantIntroduction] ";

    public AudioClip introAudio;
    public AudioClip environmentAudio;
    public AudioClip characterAudio;
    public AudioClip filterAudio;
    public AudioClip endAudio;
    public GameObject dialogPanel;
    public GameObject dialogTextObject;
    public TextAsset textAsset;
    public GameObject characters;

    private TMP_Text dialogText;
    private AssistantIntroText introText;

    AudioSource audioSource;

    private AudioStatus audioStatus = AudioStatus.Start;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dialogText = dialogTextObject.GetComponent<TMP_Text>();
        animator.Play("Idle");
        introText = AssistantIntroText.CreateFromJSON(textAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            switch (audioStatus)
            {
                case AudioStatus.Start:
                    audioSource.PlayOneShot(introAudio);
                    audioStatus = AudioStatus.Intro;
                    Debug.Log(DEBUG_MARK + audioStatus);
                    break;
                case AudioStatus.Intro:
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
                case AudioStatus.End:
                    SceneSwitcher.loadMainScene();
                    break;
            }
            dialogText.text = introText.getTextFromAudioStatus(audioStatus);
        }

    }
}
