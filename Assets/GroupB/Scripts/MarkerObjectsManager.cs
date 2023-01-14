using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class MarkerObjectsManager : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][MarkerObjectsManager] ";

    [SerializeField]
    public ARTrackedImageManager trackedImagesManager;

    // list containing all Charachers available
    [SerializeField]
    public List<GameObject> characheterGameObjectList;

    // global reference to the character actually instantieted
    // at most one at a time
    public static GameObject instantiatedCharacter;
    // keep the name of the isntatieted character in order to 
    // retrieve the gemobject from the list
    private string instantiatedCharacterName;

    void Awake()
    {
        trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        // attach event handler when tracked images change
        trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // remove event handler
        trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;

        // to be sure that the instantited character has been destroied
        if (instantiatedCharacter != null)
            Destroy(instantiatedCharacter);
        instantiatedCharacterName = null;

    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log(DEBUG_MARK + "tracked image added!");
            var markerName = trackedImage.referenceImage.name;

            // retoreve gameobject given name (gameobject and marker have the same name)
            GameObject markerObject = characheterGameObjectList.Find(item => item.name == markerName);


            if (instantiatedCharacter != null)
                Destroy(instantiatedCharacter);

            instantiatedCharacter = Instantiate(markerObject, trackedImage.transform);
            instantiatedCharacterName = markerName;
            Debug.Log(DEBUG_MARK + markerObject.name + " instantiated!");
        }


        foreach (var trackedImage in eventArgs.updated)
        {
            var markerName = trackedImage.referenceImage.name;

            // check if the traking is good enought
            if (trackedImage.trackingState == TrackingState.Limited)
            {
                if (instantiatedCharacter == null)
                    continue;

                if (markerName != instantiatedCharacterName)
                    continue;

                // reset the status 
                Character character = instantiatedCharacter.GetComponent<Character>();
                if (character.interactionStatus == InteractionStatus.Ready)
                {
                    instantiatedCharacter.SetActive(false);
                    continue;
                }

            }
            else
            {
                // check if a different character has been istantiated and update the position of
                // the right one
                if (markerName != instantiatedCharacterName)
                {
                    Destroy(instantiatedCharacter);

                    GameObject markerObject = characheterGameObjectList.Find(item => item.name == markerName);
                    instantiatedCharacter = Instantiate(markerObject, trackedImage.transform);
                    instantiatedCharacterName = markerName;
                }

                instantiatedCharacter.SetActive(true);
                instantiatedCharacter.transform.position = trackedImage.transform.position;
                instantiatedCharacter.transform.rotation = trackedImage.transform.rotation * Quaternion.Euler(-90f, 180f, 0f);
            }
        }


        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log(DEBUG_MARK + "tracked image removed!");
            if (instantiatedCharacter != null)
                Destroy(instantiatedCharacter);
            instantiatedCharacterName = null;

            Debug.Log(DEBUG_MARK + instantiatedCharacter.name + " destroyed!");
        }
    }
}
