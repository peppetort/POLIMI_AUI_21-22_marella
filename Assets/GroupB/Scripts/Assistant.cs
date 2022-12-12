using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

enum AssistantStatus
{
    Idle,
    Talking,
}

public enum HelpStatus
{
    Character,
    Environment,
    Filters
}

[System.Serializable]
public class AssistantHelpText
{
    public string environment;
    public string character;
    public string filters;

    public static AssistantHelpText CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AssistantHelpText>(jsonString);
    }
}

public class Assistant : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][Assistant] ";
    public AudioClip characterHelpAudio;
    public AudioClip environmentHelpAudio;
    public AudioClip filterHelpAudio;
    public GameObject dialogPanel;
    public GameObject dialogTextObject;
    public TextAsset textAsset;

    private TMP_Text dialogText;
    private AssistantIntroText introText;

    private AssistantStatus assistantStatus = AssistantStatus.Idle;

    AudioSource audioSource;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        audioSource = GetComponent<AudioSource>();
        dialogText = dialogTextObject.GetComponent<TMP_Text>();
        introText = AssistantIntroText.CreateFromJSON(textAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (assistantStatus == AssistantStatus.Talking && !audioSource.isPlaying)
        {
            dialogPanel.SetActive(false);
            assistantStatus = AssistantStatus.Idle;
            Debug.Log(DEBUG_MARK + assistantStatus);
            animator.enabled = false;
        }

    }

    private HelpStatus selectHelperStatus()
    {
        Scene scene = SceneSwitcher.getCurrentScene();

        if (scene.buildIndex == (int)Scenes.MAIN)
        {
            if (MarkerObjectsManager.instantiatedCharacter == null)
                return HelpStatus.Environment;
            GameObject instantiatedCharacter = MarkerObjectsManager.instantiatedCharacter;
            Renderer renderer = instantiatedCharacter.GetComponentInChildren<Renderer>();
            if (renderer.isVisible)
            {
                return HelpStatus.Character;
            }
            else
            {
                return HelpStatus.Environment;
            }
        }
        else if (scene.buildIndex == (int)Scenes.FILTER)
        {
            return HelpStatus.Filters;
        }
        return HelpStatus.Environment;
    }

    private void startHelping(HelpStatus helpStatus)
    {
        switch (helpStatus)
        {
            case HelpStatus.Character:
                audioSource.PlayOneShot(characterHelpAudio);
                dialogText.text = introText.character;
                break;
            case HelpStatus.Environment:
                audioSource.PlayOneShot(environmentHelpAudio);
                dialogText.text = introText.environment;
                break;
            case HelpStatus.Filters:
                audioSource.PlayOneShot(filterHelpAudio);
                dialogText.text = introText.filters;
                break;
        }
    }

    public void OnMouseDown()
    {
        var helperStatus = selectHelperStatus();

        if (assistantStatus != AssistantStatus.Talking)
        {
            dialogPanel.SetActive(true);
            animator.enabled = true;
            animator.Play("Jump");
            startHelping(helperStatus);
            assistantStatus = AssistantStatus.Talking;
            Debug.Log(DEBUG_MARK + assistantStatus);
        }

    }
}
