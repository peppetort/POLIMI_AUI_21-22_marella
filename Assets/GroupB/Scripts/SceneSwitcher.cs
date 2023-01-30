using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

// associate the index of each secene to a enum
enum Scenes
{
    WELCOME = 0,
    CORRIDOR = 1,
    INTRODUCTION = 2,
    WAITINGROOM = 3,
    FILTER = 4,
    FIRSTMINIGAME = 5,
    SECONDMINIGAME = 6,
}

/*
    class to change scene globally based on an Scene enum
*/
public class SceneSwitcher : MonoBehaviour
{
    public static void loadWelcomeScene()
    {
        SceneManager.LoadScene((int)Scenes.WELCOME);
    }
    public static void loadCorridorScene()
    {
        SceneManager.LoadScene((int)Scenes.CORRIDOR);
    }
    public static void loadIntroductionScene()
    {
        SceneManager.LoadScene((int)Scenes.INTRODUCTION);
    }
    public static void loadWaitingRoom()
    {
        LoaderUtility.Deinitialize();
        LoaderUtility.Initialize();
        SceneManager.LoadScene((int)Scenes.WAITINGROOM);
    }

    public static void loadFilterScene()
    {
        LoaderUtility.Deinitialize();
        LoaderUtility.Initialize();
        SceneManager.LoadScene((int)Scenes.FILTER);
    }

    public static void loadFirstMinigameScene()
    {
        SceneManager.LoadScene((int)Scenes.FIRSTMINIGAME);
    }

    public static void loadSecondMinigameScene()
    {
        SceneManager.LoadScene((int)Scenes.SECONDMINIGAME);
    }

    public static Scene getCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}