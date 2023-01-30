using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class FilterManager : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][FilterManager] ";
    private ARFaceManager arFaceManager;
    public GameObject faceFilterGameObject;
    // keep reference to instantited mask gameobject
    private GameObject instantiatedFaceFilter;


    void Awake()
    {
        arFaceManager = GetComponent<ARFaceManager>();
    }

    void OnEnable()
    {
        // attach event handler when tracked images change
        arFaceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        // remove event handler
        arFaceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        if (faceFilterGameObject == null)
            return;

        foreach (ARFace trackedface in eventArgs.added)
        {
            instantiatedFaceFilter = Instantiate(faceFilterGameObject, trackedface.transform);
            Debug.Log(DEBUG_MARK + faceFilterGameObject.name + " instantiated on face!");
        }


        foreach (ARFace trackedface in eventArgs.updated)
        {
            // need to destroy and recreate object at every update
            // update the position and rotation is not enoght 
            Destroy(instantiatedFaceFilter);
            instantiatedFaceFilter = Instantiate(faceFilterGameObject, trackedface.transform);
        }


        foreach (ARFace trackedface in eventArgs.removed)
        {
            if (instantiatedFaceFilter != null)
            {
                Destroy(instantiatedFaceFilter);
                Debug.Log(DEBUG_MARK + faceFilterGameObject.name + " destroyed!");
            }
        }
    }

    // allow to change the filter gameobject
    // it's actually colled by the button on the UI
    public void changeFaceFilter(GameObject filterGameObject)
    {
        if (instantiatedFaceFilter != null)
            Destroy(instantiatedFaceFilter);
        faceFilterGameObject = filterGameObject;
    }
}
