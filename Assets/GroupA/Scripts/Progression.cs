using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progression : MonoBehaviour
{
    public static int progressionLevelValue;

    //window image
    [SerializeField]
    private GameObject windowImage;

    //nemo image
    [SerializeField]
    private GameObject nemoImage;

    //squid image
    [SerializeField]
    private GameObject squidImage;

    //seagull image
    [SerializeField]
    private GameObject seagullImage;

    //turtle image
    [SerializeField]
    private GameObject turtleImage;

    //checks progression
    public void Start()
    {
        if(progressionLevelValue == 1)
        {
            //shows submarine window and seagull cutout
            windowImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            seagullImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        else if(progressionLevelValue == 2)
        {
            //shows submarine window, seagull, squid, turtle and nemo
            windowImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            seagullImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            squidImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            turtleImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nemoImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}
