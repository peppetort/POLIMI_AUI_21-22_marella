using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ObjectSpawnManager : MonoBehaviour
{
    private string DEBUG_MARK = "[DEBUG][ObjectSpawnManager]";

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    [SerializeField]
    List<GameObject> spawnablePrefabList;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    Camera arCam;

    // keeps track of the position of each istantiated object
    Dictionary<GameObject, Vector3> spawnedObjectPositionMap = new Dictionary<GameObject, Vector3>();

    const float OBJECT_MIN_DISTANCE = 0.1f;

    private Vector3 scaleChange = new Vector3(-0.5f, -0.5f, -0.5f);

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

        var touchPosition = Input.GetTouch(0).position;


        if (m_RaycastManager.Raycast(touchPosition, m_Hits, TrackableType.PlaneWithinPolygon))
        {

            foreach (var hit in m_Hits)
            {
                ARPlane plane = hit.trackable as ARPlane;
                bool res = SpawnRandomPrefab(hit.pose.position, plane.alignment);

                if (res)
                    return;


            }

        }
    }


    private bool SpawnRandomPrefab(Vector3 spawnPosition, PlaneAlignment planeAlignment)
    {
        // pick random prefab from spawnable list
        var filterTag = "";
        if (planeAlignment == PlaneAlignment.HorizontalUp)
            filterTag = "horizontal";
        else if (planeAlignment == PlaneAlignment.Vertical)
            filterTag = "vertical";
        else
            return false;




        var filteredPrefabs = spawnablePrefabList.Where(o => o.tag == filterTag).ToList();
        if (filteredPrefabs.Count == 0)
            return;
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
        newPrefab.transform.Rotate(0f, 180f, 0f);
        spawnedObjectPositionMap[selectedPrefab] = spawnPosition;
        spawnablePrefabList.RemoveAll(o => o.name == selectedPrefab.name);
        return true;
    }
}

