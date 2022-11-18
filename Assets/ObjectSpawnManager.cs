using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;


public class ObjectSpawnManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    [SerializeField]
    List<GameObject> spawnablePrefabList;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    Camera arCam;

    // keeps track of the position of each istantiated object
    Dictionary<GameObject, Vector3> spawnedObjectPositionMap = new Dictionary<GameObject, Vector3>();
    // keeps track of how many object of different type are near in order to create groups of different
    // object types
    Dictionary<GameObject, List<string>> spawnedObjectNearTagMap = new Dictionary<GameObject, List<string>>();

    const float OBJECT_MIN_DISTANCE_CATEGORY = 0.15f;
    const float OBJECT_MIN_DISTANCE_GROUP = 1f;
    private Vector3 scaleChange = new Vector3(-0.95f, -0.95f, -0.95f);

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

        // pick random screen coordinates
        Vector2 randomScreenPosition = new Vector2(Random.Range(0.0f, Screen.height), Random.Range(0.0f, Screen.width));

        RaycastHit hit;
        if (m_RaycastManager.Raycast(randomScreenPosition, m_Hits))
        {
            // // check distance between selected position and the ones already in the scene in order to distriute them all over the scene
            // foreach (KeyValuePair<GameObject, Vector3> entry in spawnedObjectPositionMap)
            // {
            //     float distance = Vector3.Distance(m_Hits[m_Hits.Count - 1].pose.position, entry.Value);
            //     Debug.Log(distance);
            //     if (distance < OBJECT_MIN_DISTANCE)
            //         return;
            // }

            Ray ray = arCam.ScreenPointToRay(m_Hits[m_Hits.Count - 1].pose.position);
            if (Physics.Raycast(ray, out hit))
            {
                SpawnRandomPrefab(m_Hits[m_Hits.Count - 1].pose.position);
                // var random = new System.Random();
                // int index = random.Next(spawnablePrefabList.Count);
                // SpawnPrefab(spawnablePrefabList[index], m_Hits[m_Hits.Count - 1].pose.position);
                // spawnablePrefabList.RemoveAt(index);

            }

        }
    }

    private void SpawnRandomPrefab(Vector3 spawnPosition)
    {
        // pick random prefab from spawnable list
        var random = new System.Random();
        int index = random.Next(spawnablePrefabList.Count);
        GameObject selectedPrefab = spawnablePrefabList[index];

        // check distance between selected position and the ones already in the scene in order to distriute them all over the scene
        foreach (KeyValuePair<GameObject, Vector3> entry in spawnedObjectPositionMap)
        {
            float distance = Vector3.Distance(m_Hits[m_Hits.Count - 1].pose.position, entry.Value);

            if (distance < OBJECT_MIN_DISTANCE_GROUP)
            {
                // discard if distance is lower than the minum need inside a group
                if (distance < OBJECT_MIN_DISTANCE_CATEGORY)
                {
                    return;
                }
                if (spawnedObjectNearTagMap[entry.Key].Contains(selectedPrefab.tag))
                {
                    return;
                }
            }
        }


        spawnedObjectNearTagMap[selectedPrefab] = new List<string>();
        spawnedObjectNearTagMap[selectedPrefab].Add(selectedPrefab.tag);
        spawnedObjectPositionMap[selectedPrefab] = spawnPosition;
        var newPrefab = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        newPrefab.transform.localScale += scaleChange;
        spawnablePrefabList.RemoveAt(index);
    }
}

