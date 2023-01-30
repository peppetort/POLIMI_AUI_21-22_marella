using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame1Scripts : MonoBehaviour
{
    //buttons
    //[SerializeField]
    //private GameObject seagullButton;

    //[SerializeField]
    //private GameObject ollyButton;

    //images
    [SerializeField]
    private GameObject helmetImage;

    //audiosources
    [SerializeField]
    private AudioSource[] audioclips;


    // Start is called before the first frame update
    void Awake()
    {
        //waits 2 seconds, first audioclip, sets seagull button active
        FirstWaiter();
    }

    
    //clicking on seagull
    public void OnSeagullClick()
    {
        //adds helmet, plays audioclip 2 and 3, sets olly active
        StartCoroutine(SecondWaiter());   
    }
    


    //On X button click event
    public void OnClick()
    {
        Progression.progressionLevelValue = 1;
        SceneSwitcher.loadCorridorScene();
    }


    IEnumerator FirstWaiter()
    {
        //wait 2 seconds
        yield return new WaitForSeconds(2);
        audioclips[0].PlayDelayed(0);
        yield return new WaitForSeconds(8);

    }

    IEnumerator SecondWaiter()
    {
        yield return new WaitForSeconds(2);
        helmetImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        audioclips[1].PlayDelayed(0);
        yield return new WaitForSeconds(6);
        audioclips[2].PlayDelayed(0);
        yield return new WaitForSeconds(8);
    }
    

}
