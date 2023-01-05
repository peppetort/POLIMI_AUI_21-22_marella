using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AudioStatus
{
    Intro, // introduction to the new world
    Environemnt, // explanation of which animals the kid can interact with
    Characters, // explanation of the switch camera button
    Filters, // explanation of how the filters work
    End, // There are not more info
}

[System.Serializable]
public class AssistantIntroText
{
    public List<string> start;
    public List<string> intro;
    public List<string> environment;
    public List<string> character;
    public List<string> filters;
    public List<string> end;

    public static AssistantIntroText CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AssistantIntroText>(jsonString);
    }
}

public class AssistantIntroduction : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][AssistantIntroduction] ";

    public List<AudioClip> introAudio;
    public List<AudioClip> environmentAudio;
    public List<AudioClip> characterAudio;
    public List<AudioClip> filterAudio;
    public List<AudioClip> endAudio;
    public GameObject dialogPanel;
    public GameObject dialogTextObject;
    public TextAsset textAsset;
    public GameObject characters;
    public GameObject arrow;

    private TMP_Text dialogText;
    private AssistantIntroText introText;

    AudioSource audioSource;

    private AudioStatus audioStatus = AudioStatus.Intro;
    private Animator animator;

    private int audioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dialogText = dialogTextObject.GetComponent<TMP_Text>();
        animator.Play("Idle");
        introText = AssistantIntroText.CreateFromJSON(textAsset.text);
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            switch (audioStatus)
            {
                case AudioStatus.Intro:
                    audioSource.PlayOneShot(introAudio[audioIndex]);
                    dialogText.text = introText.intro[audioIndex];
                    Debug.Log(DEBUG_MARK + audioStatus + " " + audioIndex);

                    if (audioIndex < introAudio.Count - 1)
                    {
                        audioIndex++;
                        break;
                    }
                    audioIndex = 0;
                    audioStatus = AudioStatus.Environemnt;
                    break;
                case AudioStatus.Environemnt:
                    audioSource.PlayOneShot(environmentAudio[audioIndex]);
                    dialogText.text = introText.environment[audioIndex];
                    Debug.Log(DEBUG_MARK + audioStatus + " " + audioIndex);

                    if (audioIndex < environmentAudio.Count - 1)
                    {
                        audioIndex++;
                        break;
                    }
                    audioIndex = 0;
                    audioStatus = AudioStatus.Characters;
                    break;
                case AudioStatus.Characters:
                    characters.SetActive(true);
                    audioSource.PlayOneShot(characterAudio[audioIndex]);
                    dialogText.text = introText.character[audioIndex];
                    Debug.Log(DEBUG_MARK + audioStatus + " " + audioIndex);

                    if (audioIndex < characterAudio.Count - 1)
                    {
                        audioIndex++;
                        break;
                    }
                    characters.SetActive(false);
                    audioIndex = 0;
                    audioStatus = AudioStatus.Filters;
                    break;
                case AudioStatus.Filters:
                    audioSource.PlayOneShot(filterAudio[audioIndex]);
                    dialogText.text = introText.filters[audioIndex];
                    Debug.Log(DEBUG_MARK + audioStatus + " " + audioIndex);

                    if (audioIndex == 1)
                    {
                        arrow.SetActive(true);
                    }

                    if (audioIndex < filterAudio.Count - 1)
                    {
                        audioIndex++;
                        break;
                    }
                    audioIndex = 0;
                    arrow.SetActive(false);
                    audioStatus = AudioStatus.End;
                    break;
                case AudioStatus.End:
                    if (audioIndex == endAudio.Count)
                    {
                        SceneSwitcher.loadWaitingRoom();
                        break;
                    }
                    audioSource.PlayOneShot(endAudio[audioIndex]);
                    dialogText.text = introText.end[audioIndex];
                    Debug.Log(DEBUG_MARK + audioStatus + " " + audioIndex);

                    if (audioIndex < endAudio.Count)
                    {
                        audioIndex++;
                    }
                    break;
            }
        }

    }
}
