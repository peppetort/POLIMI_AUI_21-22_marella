using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
public class MarkerObjectsManager : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][MarkerObjectsManager] ";
    public ARTrackedImageManager trackedImagesManager;

    public List<GameObject> ARGameObjects;

    private List<GameObject> showedCharacters = new List<GameObject>();

    private Dictionary<string, GameObject> markerToCharacterMap = new Dictionary<string, GameObject>();

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
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (ARGameObjects.Count == 0)
                return;


            // pick random element from available Characters and instantiate it 
            int randomCharacterIndex = Random.Range(0, ARGameObjects.Count - 1);
            var selectedCharacter = ARGameObjects[randomCharacterIndex];
            var instance = Instantiate(selectedCharacter, trackedImage.transform);


            //selectedCharacter.instantiate(trackedImage.transform);

            // associate character to the marker
            var markerName = trackedImage.referenceImage.name;
            markerToCharacterMap[markerName] = instance;

            Debug.Log(DEBUG_MARK + instance.name + " associated to " + markerName);

            // remove character from avaialble
            ARGameObjects.RemoveAt(randomCharacterIndex);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            var markerName = trackedImage.referenceImage.name;
            Destroy(markerToCharacterMap[markerName]);
        }
    }
}
