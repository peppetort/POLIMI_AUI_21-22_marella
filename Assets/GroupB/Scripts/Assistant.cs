using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/*
    keep the state of the assistant about giving helps on
    charachers, environment, face mask filters or doing nothing
*/
public enum HelpStatus
{
    Character,
    Environment,
    Filters,
    Idle
}

/*
    class to deserialize JSON file containing
    text of assistant's help audio.

    Each JSON object is a list of string. The list is ordered according to the
    sequence of the audio in order to keep them syncronized.
*/
// [System.Serializable]
// public class AssistantHelpText
// {
//     public List<string> environment;
//     public List<string> character;
//     public List<string> filters;

//     public static AssistantHelpText CreateFromJSON(string jsonString)
//     {
//         return JsonUtility.FromJson<AssistantHelpText>(jsonString);
//     }
// }

public class Assistant : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][Assistant] ";
    // audio list of charcter helps 
    public List<AudioClip> characterHelpAudio;
    // audio list of environemnt helps 
    public List<AudioClip> environmentHelpAudio;
    // audio list of filter helps 
    public List<AudioClip> filterHelpAudio;

    // reference to panel where the text will appear
    public GameObject dialogPanel;
    public GameObject dialogTextObject;
    // reference to the JSON file that contains text
    public TextAsset textAsset;
    // reference to arrow object that will displayed 
    // to point things 
    public GameObject arrow;

    private TMP_Text dialogText;
    // reference to the class that deserialize the JSON into an object
    private AssistantIntroText introText;

    private HelpStatus helpStatus = HelpStatus.Idle;

    AudioSource audioSource;
    private Animator animator;

    private int audioIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (arrow != null)
            arrow.SetActive(false);
        dialogPanel.SetActive(false);
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        audioSource = GetComponent<AudioSource>();
        dialogText = dialogTextObject.GetComponent<TMP_Text>();
        introText = AssistantIntroText.CreateFromJSON(textAsset.text);
    }

    void Update()
    {
        // check if is the case to update status or ignore update
        if (!audioSource.isPlaying && helpStatus != HelpStatus.Idle)
        {
            switch (helpStatus)
            {
                case HelpStatus.Environment:

                    // check if all audios about have been reproduced
                    if (audioIndex == environmentHelpAudio.Count)
                    {
                        // reset the status to idle
                        dialogPanel.SetActive(false);
                        animator.enabled = false;
                        audioIndex = 0;
                        helpStatus = HelpStatus.Idle;
                        break;
                    }

                    // play the audio according to the index
                    audioSource.PlayOneShot(environmentHelpAudio[audioIndex]);
                    dialogText.text = introText.environment[audioIndex];
                    Debug.Log(DEBUG_MARK + helpStatus + " " + audioIndex);

                    if (audioIndex < environmentHelpAudio.Count)
                    {
                        audioIndex++;
                    }
                    break;
                case HelpStatus.Character:

                    // check if all audios about have been reproduced
                    if (audioIndex == characterHelpAudio.Count)
                    {
                        // reset the status to idle
                        dialogPanel.SetActive(false);
                        animator.enabled = false;
                        audioIndex = 0;
                        helpStatus = HelpStatus.Idle;
                        break;
                    }

                    audioSource.PlayOneShot(characterHelpAudio[audioIndex]);
                    dialogText.text = introText.character[audioIndex];
                    Debug.Log(DEBUG_MARK + helpStatus + " " + audioIndex);

                    if (audioIndex < characterHelpAudio.Count)
                    {
                        audioIndex++;
                    }
                    break;
                case HelpStatus.Filters:

                    // check if all audios about have been reproduced
                    if (audioIndex == filterHelpAudio.Count)
                    {
                        // reset the status to idle
                        helpStatus = HelpStatus.Idle;
                        arrow.SetActive(false);
                        dialogPanel.SetActive(false);
                        animator.enabled = false;
                        audioIndex = 0;
                        break;
                    }

                    audioSource.PlayOneShot(filterHelpAudio[audioIndex]);
                    dialogText.text = introText.filters[audioIndex];
                    arrow.SetActive(true);
                    Debug.Log(DEBUG_MARK + helpStatus + " " + audioIndex);


                    if (audioIndex < filterHelpAudio.Count)
                    {
                        audioIndex++;
                    }
                    break;
            }
        }

    }

    /*
        return the status to be setted according to the context
    */
    private HelpStatus selectHelperStatus()
    {
        Scene scene = SceneSwitcher.getCurrentScene();

        // check the current scene
        if (scene.buildIndex == (int)Scenes.WAITINGROOM)
        {
            // check id there is a character available on the view
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

    public void OnMouseDown()
    {
        // change status when tapped
        dialogPanel.SetActive(true);
        animator.enabled = true;
        animator.Play("Idle");
        audioIndex = 0;
        helpStatus = selectHelperStatus();
    }
}
