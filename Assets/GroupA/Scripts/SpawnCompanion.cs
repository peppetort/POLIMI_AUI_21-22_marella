using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class SpawnCompanion : MonoBehaviour
{
    //public ARRaycastManager m_RaycastManager;
    //List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    public GameObject octopusGO;

    //camera position
    [SerializeField]
    private GameObject arCameraObject;

    //Audio
    [SerializeField]
    private AudioSource[] audioSources;

    private GameObject spawnedObject;

    public void PlaceOctopus()
    {
        Quaternion rotation = octopusGO.transform.rotation;
        Vector3 position = octopusGO.transform.position;

        octopusGO.transform.SetParent(arCameraObject.transform);

        octopusGO.transform.localPosition = position;
        octopusGO.transform.localRotation = rotation;
    }

    public void UpdateOctopusPose(GameObject aRCameraObject)
    {
        /*
        //check companion not already on screen
        if(!spawnedObject.Equals(null))
        {
            return;
        }

        spawnedObject = Instantiate(octopusGO, Vector3.zero, Quaternion.identity);

        spawnedObject.transform.SetParent(aRCameraObject.transform);

        spawnedObject.transform.localPosition = Vector3.zero;
        */

        octopusGO.SetActive(true);
        System.Random random = new System.Random();
        int i = random.Next(6);
        audioSources[i-1].PlayDelayed(0);
        

        //position in front of the camera
        //Vector3 position = aRCameraObject.transform.position += new Vector3(0, 0, 1);

        //Instantiate octopus and show him on screen
        //spawnPrefab(position);
        

        //wait 8 seconds and remove octopus from screen
        StartCoroutine(Waiter());
    }


    IEnumerator Waiter()
    {
        //wait 8 seconds
        yield return new WaitForSeconds(8);

        //remove octopus from screen
        octopusGO.SetActive(false);
    }

    void Start()
    {
        spawnedObject = null;
    }

}
