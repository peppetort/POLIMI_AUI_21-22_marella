using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AudioStatus
{
    Intro, // introduction to the new world
    Animals, // explanation of which animals the kid can interact with
    FiltersIntro, // explanation of the switch camera button
    FiltersExp, // explanation of how the filters work
    Finish, // There are not more info
}

public class olly_introduction : MonoBehaviour
{
    public AudioClip[] audioClips;
    public GameObject CanvasAnimals;
    public GameObject filters; // button to switch camera 

    AudioSource audioSource;
    private IEnumerator olly_routine_introduction;

    private AudioStatus audioStatus = AudioStatus.Intro;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        olly_routine_introduction = ReproduceAudio();
        StartCoroutine(olly_routine_introduction);
    }

    // Update is called once per frame
    void Update()
    {
        if (audioStatus == AudioStatus.Animals)
            CanvasAnimals.SetActive(true);
        else
        {
            CanvasAnimals.SetActive(false);
            if(audioStatus == AudioStatus.FiltersIntro)
            {
                filters.SetActive(true); // to show the button
            }

            
        }
            
    }
    private IEnumerator ReproduceAudio()
    {
        for (var i = 0; i < audioClips.Length; i++)
        {
            // print(audioClips[i].length);
            audioSource.PlayOneShot(audioClips[i]);
            yield return new WaitForSeconds(audioClips[i].length + 1);
            audioStatus = audioStatus + 1;
        }
    }
}
