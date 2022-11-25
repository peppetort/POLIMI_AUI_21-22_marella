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

    private Dictionary<string, GameObject> markerToCharacterInstancesMap = new Dictionary<string, GameObject>();

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
            markerToCharacterInstancesMap[markerName] = Instantiate(markerObject, trackedImage.transform);
            Debug.Log(DEBUG_MARK + markerObject.name + " instantiated!");
        }


        foreach (var trackedImage in eventArgs.updated)
        {
            var markerName = trackedImage.referenceImage.name;

            if (markerToCharacterInstancesMap[markerName] != null)
                return;

            GameObject markerObject = characheterGameObjectList.Find(item => item.name == markerName);
            markerToCharacterInstancesMap[markerName] = Instantiate(markerObject, trackedImage.transform);
            Debug.Log(DEBUG_MARK + markerObject.name + " instantiated!");
        }


        foreach (var trackedImage in eventArgs.removed)
        {
            var markerName = trackedImage.referenceImage.name;
            GameObject objectInstance = markerToCharacterInstancesMap[markerName];
            Destroy(objectInstance);
            Debug.Log(DEBUG_MARK + objectInstance.name + " destroyed!");
        }
    }
}
