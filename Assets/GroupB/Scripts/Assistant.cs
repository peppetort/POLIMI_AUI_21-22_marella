using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// enum AssistantStatus
// {
//     Idle,
//     Talking,
// }

public enum HelpStatus
{
    Character,
    Environment,
    Filters,
    Idle
}

[System.Serializable]
public class AssistantHelpText
{
    public List<string> environment;
    public List<string> character;
    public List<string> filters;

    public static AssistantHelpText CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AssistantHelpText>(jsonString);
    }
}

public class Assistant : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][Assistant] ";
    public List<AudioClip> characterHelpAudio;
    public List<AudioClip> environmentHelpAudio;
    public List<AudioClip> filterHelpAudio;
    public GameObject dialogPanel;
    public GameObject dialogTextObject;
    public TextAsset textAsset;
    public GameObject arrow;

    private TMP_Text dialogText;
    private AssistantIntroText introText;

    //private AssistantStatus assistantStatus = AssistantStatus.Idle;
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

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && helpStatus != HelpStatus.Idle)
        {
            switch (helpStatus)
            {
                case HelpStatus.Environment:
                    if (audioIndex == environmentHelpAudio.Count)
                    {
                        dialogPanel.SetActive(false);
                        animator.enabled = false;
                        audioIndex = 0;
                        helpStatus = HelpStatus.Idle;
                        break;
                    }

                    audioSource.PlayOneShot(environmentHelpAudio[audioIndex]);
                    dialogText.text = introText.environment[audioIndex];
                    Debug.Log(DEBUG_MARK + helpStatus + " " + audioIndex);

                    if (audioIndex < environmentHelpAudio.Count)
                    {
                        audioIndex++;
                    }
                    break;
                case HelpStatus.Character:
                    if (audioIndex == characterHelpAudio.Count)
                    {
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
                    if (audioIndex == filterHelpAudio.Count)
                    {
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

    public void OnMouseDown()
    {
        dialogPanel.SetActive(true);
        animator.enabled = true;
        animator.Play("Jump");
        audioIndex = 0;
        helpStatus = selectHelperStatus();
    }
}
