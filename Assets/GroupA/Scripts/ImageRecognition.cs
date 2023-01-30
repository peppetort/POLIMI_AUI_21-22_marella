using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognition : MonoBehaviour
{
    //prefabs to spawn
    [SerializeField]
    private GameObject[] placablePrefabs;

    //camera position
    [SerializeField]
    private GameObject arCameraObject;

    //window image
    [SerializeField]
    private GameObject windowImage;

    //nemo image
    [SerializeField]
    private GameObject nemoImage;

    //squid image
    [SerializeField]
    private GameObject squidImage;

    //killerwhale image
    [SerializeField]
    private GameObject killerwhaleImage;

    //seahorse image
    [SerializeField]
    private GameObject seahorseImage;

    //seagull image
    [SerializeField]
    private GameObject seagullImage;

    //turtle image
    [SerializeField]
    private GameObject turtleImage;

    [SerializeField]
    private AudioSource[] audioSources;

    private ARTrackedImageManager _aRTrackedImageManager;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    private TrackingState submarineState;




    private void Awake()
    {
        _aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        _aRTrackedImageManager.requestedMaxNumberOfMovingImages = 4;

        foreach (GameObject prefab in placablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.SetActive(false);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }

    }

    public void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += OnChanged;
    }

    public void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= OnChanged;
    }


    public void OnChanged(ARTrackedImagesChangedEventArgs args)
    {

        foreach (ARTrackedImage trackedImage in args.added)
        {
            print("args.added");

            //the image is the submarine sticker
            if (trackedImage.referenceImage.name.Equals("Submarine"))
            {
                AddSubmarine(trackedImage);
            }

            //the image is the window sticker
            else if (trackedImage.referenceImage.name.Equals("Window"))
            {
                //add window frame to the UI
                AddWindowFrame();
            }

            //the image is for the first minigame change of scene            
            else if (trackedImage.referenceImage.name.Equals("FirstMinigame"))
            {
                SceneSwitcher.loadFirstMinigameScene();
            }

            //the image is for the second minigame change of scene           
            else if (trackedImage.referenceImage.name.Equals("SecondMinigame"))
            {
                SceneSwitcher.loadSecondMinigameScene();
            }

            //the image is for the final change of scene
            else if (trackedImage.referenceImage.name.Equals("NextScene"))
            {
                SceneSwitcher.loadIntroductionScene();
            }

            //the image is an animal sticker
            else
            {
                PlaceAnimal(trackedImage);
                UpdateAnimalImage(trackedImage);
            }
                
        }


        foreach (ARTrackedImage trackedImage in args.updated)
        {
            print("args.updated " + trackedImage.referenceImage.name + " " + trackedImage.trackingState);

            //the image is the submarine sticker
            if (trackedImage.referenceImage.name.Equals("Submarine"))
            {
                print("1 " + trackedImage.trackingState);
                UpdateSubmarine(trackedImage);
            }

            //the image is the window sticker
            else if (trackedImage.referenceImage.name.Equals("Window"))
            {
                //add window frame to the UI (ma c'è già perché basta farlo una volta?)
                //AddWindowFrame();
                ;
            }

            //the image is an animal sticker
            // else
            //     UpdateAnimalImage(trackedImage);
        }
    }


    //spwans submarine 3D model on top of sticker
    private void AddSubmarine(ARTrackedImage trackedImage)
    {
        GameObject prefab = spawnedPrefabs["Submarine"];
        prefab.transform.SetParent(trackedImage.transform);

        prefab.transform.localPosition = Vector3.zero;
        prefab.SetActive(true);

        audioSources[0].PlayDelayed(0);
    }


    //shows the submarine 3D model when the sticker is in frame and removes it when the sticker gets out of frame
    private void UpdateSubmarine(ARTrackedImage trackedImage)
    {
        GameObject prefab = spawnedPrefabs["Submarine"];
        TrackingState newSubmarineState = trackedImage.trackingState;
        print("arriva in RemoveSubmarine");

        if (newSubmarineState.Equals(TrackingState.Tracking))
        {
            prefab.SetActive(true);
        }

        //true the moment the TrackingState goes from Tracking to limited
        if (submarineState.Equals(TrackingState.Tracking) && newSubmarineState.Equals(TrackingState.Limited))
        {
            prefab.SetActive(false);
        }

        submarineState = newSubmarineState;
    }

    //places the animal at a fixed position from the camera
    private void PlaceAnimal(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        GameObject prefab = spawnedPrefabs[name];
        Quaternion rotation = prefab.transform.rotation;
        Vector3 position = prefab.transform.position;

        prefab.transform.SetParent(arCameraObject.transform);
        prefab.transform.localPosition = position;
        prefab.transform.localRotation = rotation;

        AddImage(name);
        playAudio(name);
    }


    //spawn animal 3D model and make it swim around
    private void UpdateAnimalImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        GameObject prefab = spawnedPrefabs[name];
        prefab.SetActive(true);

        //hides old 3D asset if looking at new image
        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (!go.name.Equals(name))
            {
                go.SetActive(false);
            }
        }

        //wait 8 seconds and remove animal from screen
        StartCoroutine(Waiter(prefab));
    }

    //adds the window frame to the UI
    private void AddWindowFrame()
    {
        //changes the image transparency from 0 to 255 to show it
        windowImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void playAudio(string name)
    {
        if (name.Equals("Seagull"))
        {
            audioSources[1].PlayDelayed(0);
        }
        else if (name.Equals("Turtle"))
        {
            audioSources[2].PlayDelayed(0);
        }
        else if (name.Equals("Squid"))
        {
            audioSources[3].PlayDelayed(0);
        }
        else if (name.Equals("Killerwhale"))
        {
            audioSources[4].PlayDelayed(0);
        }
        else if (name.Equals("Nemo"))
        {
            audioSources[5].PlayDelayed(0);
        }
        else if (name.Equals("Seahorse"))
        {
            audioSources[6].PlayDelayed(0);
        }
    }

    private void AddImage(string name)
    {
        if (name.Equals("Nemo"))
        {
            AddNemo();
        }
        else if (name.Equals("Squid"))
        {
            AddSquid();
        }
        else if (name.Equals("Killerwhale"))
        {
            AddKillerwhale();
        }

        else if (name.Equals("Seagull"))
        {
            AddSeagull();
        }

        else if (name.Equals("Turtle"))
        {
            AddTurtle();
        }
        else
            AddSeahorse();
    }

    private void AddNemo()
    {
        nemoImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void AddKillerwhale()
    {
        killerwhaleImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void AddSquid()
    {
        squidImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void AddSeahorse()
    {
        seahorseImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void AddSeagull()
    {
        seagullImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void AddTurtle()
    {
        turtleImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }


    IEnumerator Waiter(GameObject gameObject)
    {
        //wait 8 seconds
        yield return new WaitForSeconds(8);

        //remove animal from screen
        gameObject.SetActive(false);
    }

}
