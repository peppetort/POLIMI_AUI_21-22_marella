using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

/*
    attached to filter UI element (button)
*/
public class OnFilterButtonTap : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][OnFilterButtonTap]";

    public GameObject filterGameObject;
    public ARFaceManager ARFaceManager;


    void Start()
    {
        DEBUG_MARK = DEBUG_MARK + "[" + filterGameObject.name + "] ";
    }
    public void OnMouseDown()
    {
        // call function of FilterManager
        ARFaceManager.GetComponent<FilterManager>().changeFaceFilter(filterGameObject);
    }
}
