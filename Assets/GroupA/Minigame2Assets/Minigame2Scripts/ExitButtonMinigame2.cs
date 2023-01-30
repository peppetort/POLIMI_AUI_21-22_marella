using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonMinigame2 : MonoBehaviour
{
    //On click event
    public void OnClick()
    {
        Progression.progressionLevelValue = 2;
        SceneSwitcher.loadCorridorScene();
    }
}
