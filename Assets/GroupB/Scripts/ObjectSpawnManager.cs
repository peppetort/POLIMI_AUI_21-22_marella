using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class ObjectSpawnManager : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][ObjectSpawnManager]";

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    // list containing all available decoration prefabs
    [SerializeField]
    List<GameObject> spawnablePrefabList;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    Camera arCam;

    // keeps track of the position of each istantiated object
    Dictionary<GameObject, Vector3> spawnedObjectPositionMap = new Dictionary<GameObject, Vector3>();

    // minimun distacne to instantitate an object 
    const float OBJECT_MIN_DISTANCE = 0.15f;

    private Vector3 scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);

    // Start is called before the first frame update
    void Start()
    {
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // check first if I have object to place 
        if (spawnablePrefabList.Count == 0)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Ended)
            return;

        // ignore when clicked on UI element
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        var touchPosition = touch.position;

        // raycast onnly on planes
        if (m_RaycastManager.Raycast(touchPosition, m_Hits, TrackableType.PlaneWithinPolygon))
        {

            foreach (var hit in m_Hits)
            {
                ARPlane plane = hit.trackable as ARPlane;

                // spawn a random prefab and try until one has been instantiated correctly
                bool res = SpawnRandomPrefab(hit.pose.position, plane.alignment);

                if (res)
                {
                    return;
                }


            }

        }
    }


    private bool SpawnRandomPrefab(Vector3 spawnPosition, PlaneAlignment planeAlignment)
    {
        // select tag according to the detected plane 
        var filterTag = "";
        if (planeAlignment == PlaneAlignment.HorizontalUp)
            filterTag = "horizontal";
        else if (planeAlignment == PlaneAlignment.Vertical)
            filterTag = "vertical";
        else
            return false;



        // get all prefab that respect the selected tag
        var filteredPrefabs = spawnablePrefabList.Where(o => o.tag == filterTag).ToList();
        if (filteredPrefabs.Count == 0)
            return false;

        // generate a random index and pick a random object from list
        var random = new System.Random();
        int index = random.Next(filteredPrefabs.Count);
        GameObject selectedPrefab = filteredPrefabs[index];

        // check distance between selected position and the ones already in the scene in order to distriute them all over the scene
        foreach (KeyValuePair<GameObject, Vector3> entry in spawnedObjectPositionMap)
        {
            float distance = Vector3.Distance(spawnPosition, entry.Value);

            if (distance < OBJECT_MIN_DISTANCE)
                return false;
        }

        var newPrefab = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        newPrefab.transform.localScale += scaleChange;
        spawnedObjectPositionMap[selectedPrefab] = spawnPosition;

        // remove object from original list when instantiated 
        // in order to prevent a same object to be instantiated twice
        spawnablePrefabList.RemoveAll(o => o.name == selectedPrefab.name);
        return true;
    }
}

