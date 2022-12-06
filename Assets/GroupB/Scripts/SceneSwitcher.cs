using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Scenes
{
    WELCOME = 0,
    INTRODUCTION = 1,
    MAIN = 2,
    FILTER = 3,

}

public class SceneSwitcher : MonoBehaviour
{
    public static void loadIntroductionScene()
    {
        SceneManager.LoadScene((int)Scenes.INTRODUCTION);
    }
    public static void loadMainScene()
    {
        SceneManager.LoadScene((int)Scenes.MAIN);
    }

    public static void loadFilterScene()
    {
        SceneManager.LoadScene((int)Scenes.FILTER);
    }
}