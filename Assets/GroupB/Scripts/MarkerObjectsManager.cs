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
    [SerializeField]
    public List<GameObject> characheterGameObjectList;

    //private Dictionary<string, GameObject> markerToCharacterInstancesMap = new Dictionary<string, GameObject>();
    public static GameObject instantiatedCharacter;
    public static string instantiatedMarkerName;

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
            var markerName = trackedImage.referenceImage.name;
            GameObject markerObject = characheterGameObjectList.Find(item => item.name == markerName);
            if (markerObject == null)
                return;

            if (instantiatedCharacter != null)
                Destroy(instantiatedCharacter);

            instantiatedCharacter = Instantiate(markerObject, trackedImage.transform);
            instantiatedMarkerName = markerName;
            Debug.Log(DEBUG_MARK + markerObject.name + " instantiated!");
        }


        foreach (var trackedImage in eventArgs.updated)
        {
            var markerName = trackedImage.referenceImage.name;

            if (instantiatedMarkerName == markerName)
                return;

            if (instantiatedCharacter != null)
                Destroy(instantiatedCharacter);

            GameObject markerObject = characheterGameObjectList.Find(item => item.name == markerName);
            instantiatedCharacter = Instantiate(markerObject, trackedImage.transform);
            instantiatedMarkerName = markerName;

            Debug.Log(DEBUG_MARK + markerObject.name + " instantiated!");
        }


        foreach (var trackedImage in eventArgs.removed)
        {
            Destroy(instantiatedCharacter);
            instantiatedMarkerName = null;

            Debug.Log(DEBUG_MARK + instantiatedCharacter.name + " destroyed!");
        }
    }
}
